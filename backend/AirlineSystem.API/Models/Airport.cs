using System.ComponentModel.DataAnnotations;

namespace AirlineSystem.API.Models;

public class Airport
{
    public int Id { get; set; }

    [Required, MaxLength(3)]
    public string Code { get; set; } = string.Empty; // IATA code, e.g. "JFK"

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string City { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Country { get; set; } = string.Empty;

    public ICollection<Flight> DepartingFlights { get; set; } = new List<Flight>();
    public ICollection<Flight> ArrivingFlights { get; set; } = new List<Flight>();
}
