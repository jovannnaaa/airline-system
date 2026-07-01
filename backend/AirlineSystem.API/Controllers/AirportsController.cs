using AirlineSystem.API.Data;
using AirlineSystem.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirlineSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AirportsController : ControllerBase
{
    private readonly AirlineDbContext _context;

    public AirportsController(AirlineDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Airport>>> GetAll()
    {
        return Ok(await _context.Airports.OrderBy(a => a.City).ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Airport>> GetById(int id)
    {
        var airport = await _context.Airports.FindAsync(id);
        if (airport is null) return NotFound();
        return Ok(airport);
    }

    [HttpPost]
    public async Task<ActionResult<Airport>> Create(Airport airport)
    {
        _context.Airports.Add(airport);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = airport.Id }, airport);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Airport updated)
    {
        if (id != updated.Id) return BadRequest();
        var existing = await _context.Airports.FindAsync(id);
        if (existing is null) return NotFound();

        existing.Code = updated.Code;
        existing.Name = updated.Name;
        existing.City = updated.City;
        existing.Country = updated.Country;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var airport = await _context.Airports.FindAsync(id);
        if (airport is null) return NotFound();

        _context.Airports.Remove(airport);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
