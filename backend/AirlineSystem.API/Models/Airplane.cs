using System.ComponentModel.DataAnnotations;

namespace AirlineSystem.API.Models;

public class Airplane
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string TailNumber { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Model { get; set; } = string.Empty; // e.g. "Boeing 737-800"

    public int EconomySeats { get; set; }
    public int BusinessSeats { get; set; }
    public int FirstClassSeats { get; set; }

    public int TotalSeats => EconomySeats + BusinessSeats + FirstClassSeats;

    public ICollection<Flight> Flights { get; set; } = new List<Flight>();
}
