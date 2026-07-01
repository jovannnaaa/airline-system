using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineSystem.API.Models;

/// <summary>
/// Links a passenger to a specific booking, seat class and seat assignment.
/// A booking can have multiple passengers (e.g. family booking).
/// </summary>
public class BookingPassenger
{
    public int Id { get; set; }

    public int BookingId { get; set; }
    public Booking? Booking { get; set; }

    public int PassengerId { get; set; }
    public Passenger? Passenger { get; set; }

    public SeatClass SeatClass { get; set; }

    [MaxLength(5)]
    public string? SeatNumber { get; set; } // e.g. "14A"

    [Column(TypeName = "decimal(10,2)")]
    public decimal PricePaid { get; set; }
}
