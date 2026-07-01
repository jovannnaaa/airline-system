using AirlineSystem.API.Models;

namespace AirlineSystem.API.DTOs;

public class PassengerInputDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PassportNumber { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public SeatClass SeatClass { get; set; }
}

public class CreateBookingDto
{
    public int FlightId { get; set; }
    public int? UserId { get; set; }
    public List<PassengerInputDto> Passengers { get; set; } = new();
}

public class BookingResponseDto
{
    public int Id { get; set; }
    public string BookingReference { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public decimal TotalPrice { get; set; }
    public FlightResponseDto? Flight { get; set; }
    public List<BookingPassengerDto> Passengers { get; set; } = new();
}

public class BookingPassengerDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string SeatClass { get; set; } = string.Empty;
    public string? SeatNumber { get; set; }
    public decimal PricePaid { get; set; }
}
