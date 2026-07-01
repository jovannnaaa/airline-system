using AirlineSystem.API.Data;
using AirlineSystem.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirlineSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AirplanesController : ControllerBase
{
    private readonly AirlineDbContext _context;

    public AirplanesController(AirlineDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Airplane>>> GetAll()
    {
        return Ok(await _context.Airplanes.ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Airplane>> GetById(int id)
    {
        var airplane = await _context.Airplanes.FindAsync(id);
        if (airplane is null) return NotFound();
        return Ok(airplane);
    }

    [HttpPost]
    public async Task<ActionResult<Airplane>> Create(Airplane airplane)
    {
        _context.Airplanes.Add(airplane);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = airplane.Id }, airplane);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var airplane = await _context.Airplanes.FindAsync(id);
        if (airplane is null) return NotFound();

        _context.Airplanes.Remove(airplane);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
