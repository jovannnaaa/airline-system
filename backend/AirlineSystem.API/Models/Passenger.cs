using System.ComponentModel.DataAnnotations;

namespace AirlineSystem.API.Models;

public class Passenger
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string PassportNumber { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    [MaxLength(150)]
    public string? Email { get; set; }

    [MaxLength(30)]
    public string? Phone { get; set; }

    public ICollection<BookingPassenger> BookingPassengers { get; set; } = new List<BookingPassenger>();
}
