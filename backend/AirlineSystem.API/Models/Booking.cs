using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineSystem.API.Models;

public enum BookingStatus
{
    Pending = 0,
    Confirmed = 1,
    CheckedIn = 2,
    Cancelled = 3,
    Completed = 4
}

public class Booking
{
    public int Id { get; set; }

    [Required, MaxLength(12)]
    public string BookingReference { get; set; } = string.Empty; // e.g. "AB12CD"

    public int FlightId { get; set; }
    public Flight? Flight { get; set; }

    public int? UserId { get; set; }
    public User? User { get; set; }

    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; }

    public ICollection<BookingPassenger> BookingPassengers { get; set; } = new List<BookingPassenger>();
}
