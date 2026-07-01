using AirlineSystem.API.Data;
using AirlineSystem.API.DTOs;
using AirlineSystem.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirlineSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly AirlineDbContext _context;

    public BookingsController(AirlineDbContext context)
    {
        _context = context;
    }

    private IQueryable<Booking> WithIncludes() =>
        _context.Bookings
            .Include(b => b.Flight!.DepartureAirport)
            .Include(b => b.Flight!.ArrivalAirport)
            .Include(b => b.Flight!.Airplane)
            .Include(b => b.BookingPassengers).ThenInclude(bp => bp.Passenger);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingResponseDto>>> GetAll([FromQuery] int? userId)
    {
        var query = WithIncludes();
        if (userId.HasValue) query = query.Where(b => b.UserId == userId.Value);

        var bookings = await query.OrderByDescending(b => b.CreatedAtUtc).ToListAsync();
        return Ok(bookings.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookingResponseDto>> GetById(int id)
    {
        var booking = await WithIncludes().FirstOrDefaultAsync(b => b.Id == id);
        if (booking is null) return NotFound();
        return Ok(ToDto(booking));
    }

    [HttpGet("reference/{reference}")]
    public async Task<ActionResult<BookingResponseDto>> GetByReference(string reference)
    {
        var booking = await WithIncludes().FirstOrDefaultAsync(b => b.BookingReference == reference.ToUpper());
        if (booking is null) return NotFound();
        return Ok(ToDto(booking));
    }

    [HttpPost]
    public async Task<ActionResult<BookingResponseDto>> Create(CreateBookingDto dto)
    {
        if (dto.Passengers is null || dto.Passengers.Count == 0)
            return BadRequest("At least one passenger is required.");

        using var transaction = await _context.Database.BeginTransactionAsync();

        var flight = await _context.Flights.FirstOrDefaultAsync(f => f.Id == dto.FlightId);
        if (flight is null) return BadRequest("Flight not found.");

        // Validate seat availability per class
        var seatCountByClass = dto.Passengers.GroupBy(p => p.SeatClass)
            .ToDictionary(g => g.Key, g => g.Count());

        foreach (var (seatClass, count) in seatCountByClass)
        {
            if (flight.GetAvailableSeatsForClass(seatClass) < count)
                return BadRequest($"Not enough {seatClass} seats available on this flight.");
        }

        var booking = new Booking
        {
            BookingReference = GenerateBookingReference(),
            FlightId = flight.Id,
            UserId = dto.UserId,
            Status = BookingStatus.Confirmed,
            CreatedAtUtc = DateTime.UtcNow
        };

        decimal total = 0;

        foreach (var p in dto.Passengers)
        {
            // Reuse existing passenger by passport number, or create a new one
            var passenger = await _context.Passengers
                .FirstOrDefaultAsync(x => x.PassportNumber == p.PassportNumber);

            if (passenger is null)
            {
                passenger = new Passenger
                {
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    PassportNumber = p.PassportNumber,
                    DateOfBirth = p.DateOfBirth,
                    Email = p.Email,
                    Phone = p.Phone
                };
                _context.Passengers.Add(passenger);
                await _context.SaveChangesAsync();
            }

            var price = flight.GetPriceForClass(p.SeatClass);
            total += price;

            booking.BookingPassengers.Add(new BookingPassenger
            {
                PassengerId = passenger.Id,
                SeatClass = p.SeatClass,
                PricePaid = price
            });

            // Decrement available seats
            switch (p.SeatClass)
            {
                case SeatClass.Economy: flight.EconomySeatsAvailable--; break;
                case SeatClass.Business: flight.BusinessSeatsAvailable--; break;
                case SeatClass.FirstClass: flight.FirstClassSeatsAvailable--; break;
            }
        }

        booking.TotalPrice = total;

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        var created = await WithIncludes().FirstAsync(b => b.Id == booking.Id);
        return CreatedAtAction(nameof(GetById), new { id = booking.Id }, ToDto(created));
    }

    [HttpPut("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var booking = await _context.Bookings
            .Include(b => b.Flight)
            .Include(b => b.BookingPassengers)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking is null) return NotFound();
        if (booking.Status == BookingStatus.Cancelled) return BadRequest("Booking already cancelled.");

        // Release the seats back to the flight
        foreach (var bp in booking.BookingPassengers)
        {
            switch (bp.SeatClass)
            {
                case SeatClass.Economy: booking.Flight!.EconomySeatsAvailable++; break;
                case SeatClass.Business: booking.Flight!.BusinessSeatsAvailable++; break;
                case SeatClass.FirstClass: booking.Flight!.FirstClassSeatsAvailable++; break;
            }
        }

        booking.Status = BookingStatus.Cancelled;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id:int}/checkin")]
    public async Task<IActionResult> CheckIn(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking is null) return NotFound();
        if (booking.Status != BookingStatus.Confirmed) return BadRequest("Only confirmed bookings can be checked in.");

        booking.Status = BookingStatus.CheckedIn;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static string GenerateBookingReference()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        var random = Random.Shared;
        return new string(Enumerable.Range(0, 6).Select(_ => chars[random.Next(chars.Length)]).ToArray());
    }

    private static BookingResponseDto ToDto(Booking b) => new()
    {
        Id = b.Id,
        BookingReference = b.BookingReference,
        Status = b.Status.ToString(),
        CreatedAtUtc = b.CreatedAtUtc,
        TotalPrice = b.TotalPrice,
        Flight = b.Flight is null ? null : FlightResponseDto.FromEntity(b.Flight),
        Passengers = b.BookingPassengers.Select(bp => new BookingPassengerDto
        {
            FirstName = bp.Passenger?.FirstName ?? "",
            LastName = bp.Passenger?.LastName ?? "",
            SeatClass = bp.SeatClass.ToString(),
            SeatNumber = bp.SeatNumber,
            PricePaid = bp.PricePaid
        }).ToList()
    };
}
