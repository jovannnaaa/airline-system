using AirlineSystem.API.Data;
using AirlineSystem.API.DTOs;
using AirlineSystem.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirlineSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly AirlineDbContext _context;

    public FlightsController(AirlineDbContext context)
    {
        _context = context;
    }

    private IQueryable<Flight> WithIncludes() =>
        _context.Flights
            .Include(f => f.DepartureAirport)
            .Include(f => f.ArrivalAirport)
            .Include(f => f.Airplane);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FlightResponseDto>>> GetAll()
    {
        var flights = await WithIncludes().OrderBy(f => f.DepartureTimeUtc).ToListAsync();
        return Ok(flights.Select(FlightResponseDto.FromEntity));
    }

    // GET api/flights/search?departureCode=JFK&arrivalCode=LAX&date=2026-07-04&seatClass=Economy&passengers=2
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<FlightResponseDto>>> Search(
        [FromQuery] string? departureCode,
        [FromQuery] string? arrivalCode,
        [FromQuery] DateTime? date,
        [FromQuery] SeatClass? seatClass,
        [FromQuery] int passengers = 1)
    {
        var query = WithIncludes();

        if (!string.IsNullOrWhiteSpace(departureCode))
            query = query.Where(f => f.DepartureAirport!.Code == departureCode.ToUpper());

        if (!string.IsNullOrWhiteSpace(arrivalCode))
            query = query.Where(f => f.ArrivalAirport!.Code == arrivalCode.ToUpper());

        if (date.HasValue)
            query = query.Where(f => f.DepartureTimeUtc.Date == date.Value.Date);

        var flights = await query.OrderBy(f => f.DepartureTimeUtc).ToListAsync();

        if (seatClass.HasValue)
        {
            flights = flights
                .Where(f => f.GetAvailableSeatsForClass(seatClass.Value) >= passengers)
                .ToList();
        }

        return Ok(flights.Select(FlightResponseDto.FromEntity));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FlightResponseDto>> GetById(int id)
    {
        var flight = await WithIncludes().FirstOrDefaultAsync(f => f.Id == id);
        if (flight is null) return NotFound();
        return Ok(FlightResponseDto.FromEntity(flight));
    }

    [HttpPost]
    public async Task<ActionResult<FlightResponseDto>> Create(CreateFlightDto dto)
    {
        var airplane = await _context.Airplanes.FindAsync(dto.AirplaneId);
        if (airplane is null) return BadRequest("Airplane not found.");

        var flight = new Flight
        {
            FlightNumber = dto.FlightNumber,
            DepartureAirportId = dto.DepartureAirportId,
            ArrivalAirportId = dto.ArrivalAirportId,
            AirplaneId = dto.AirplaneId,
            DepartureTimeUtc = dto.DepartureTimeUtc,
            ArrivalTimeUtc = dto.ArrivalTimeUtc,
            EconomyPrice = dto.EconomyPrice,
            BusinessPrice = dto.BusinessPrice,
            FirstClassPrice = dto.FirstClassPrice,
            EconomySeatsAvailable = airplane.EconomySeats,
            BusinessSeatsAvailable = airplane.BusinessSeats,
            FirstClassSeatsAvailable = airplane.FirstClassSeats
        };

        _context.Flights.Add(flight);
        await _context.SaveChangesAsync();

        var created = await WithIncludes().FirstAsync(f => f.Id == flight.Id);
        return CreatedAtAction(nameof(GetById), new { id = flight.Id }, FlightResponseDto.FromEntity(created));
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] FlightStatus status)
    {
        var flight = await _context.Flights.FindAsync(id);
        if (flight is null) return NotFound();

        flight.Status = status;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var flight = await _context.Flights.FindAsync(id);
        if (flight is null) return NotFound();

        _context.Flights.Remove(flight);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
