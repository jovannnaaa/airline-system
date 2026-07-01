using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineSystem.API.Models;

public enum SeatClass
{
    Economy = 0,
    Business = 1,
    FirstClass = 2
}

public enum FlightStatus
{
    Scheduled = 0,
    Boarding = 1,
    Departed = 2,
    Landed = 3,
    Cancelled = 4,
    Delayed = 5
}

public class Flight
{
    public int Id { get; set; }

    [Required, MaxLength(10)]
    public string FlightNumber { get; set; } = string.Empty; // e.g. "AS204"

    public int DepartureAirportId { get; set; }
    public Airport? DepartureAirport { get; set; }

    public int ArrivalAirportId { get; set; }
    public Airport? ArrivalAirport { get; set; }

    public int AirplaneId { get; set; }
    public Airplane? Airplane { get; set; }

    public DateTime DepartureTimeUtc { get; set; }
    public DateTime ArrivalTimeUtc { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal EconomyPrice { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal BusinessPrice { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal FirstClassPrice { get; set; }

    public int EconomySeatsAvailable { get; set; }
    public int BusinessSeatsAvailable { get; set; }
    public int FirstClassSeatsAvailable { get; set; }

    public FlightStatus Status { get; set; } = FlightStatus.Scheduled;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public decimal GetPriceForClass(SeatClass seatClass) => seatClass switch
    {
        SeatClass.Economy => EconomyPrice,
        SeatClass.Business => BusinessPrice,
        SeatClass.FirstClass => FirstClassPrice,
        _ => throw new ArgumentOutOfRangeException(nameof(seatClass))
    };

    public int GetAvailableSeatsForClass(SeatClass seatClass) => seatClass switch
    {
        SeatClass.Economy => EconomySeatsAvailable,
        SeatClass.Business => BusinessSeatsAvailable,
        SeatClass.FirstClass => FirstClassSeatsAvailable,
        _ => throw new ArgumentOutOfRangeException(nameof(seatClass))
    };
}
