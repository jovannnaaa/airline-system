using AirlineSystem.API.Models;

namespace AirlineSystem.API.DTOs;

public class FlightSearchQuery
{
    public string? DepartureCode { get; set; }
    public string? ArrivalCode { get; set; }
    public DateTime? Date { get; set; }
    public SeatClass? SeatClass { get; set; }
    public int Passengers { get; set; } = 1;
}

public class FlightResponseDto
{
    public int Id { get; set; }
    public string FlightNumber { get; set; } = string.Empty;
    public string DepartureAirportCode { get; set; } = string.Empty;
    public string DepartureCity { get; set; } = string.Empty;
    public string ArrivalAirportCode { get; set; } = string.Empty;
    public string ArrivalCity { get; set; } = string.Empty;
    public DateTime DepartureTimeUtc { get; set; }
    public DateTime ArrivalTimeUtc { get; set; }
    public string AirplaneModel { get; set; } = string.Empty;
    public decimal EconomyPrice { get; set; }
    public decimal BusinessPrice { get; set; }
    public decimal FirstClassPrice { get; set; }
    public int EconomySeatsAvailable { get; set; }
    public int BusinessSeatsAvailable { get; set; }
    public int FirstClassSeatsAvailable { get; set; }
    public string Status { get; set; } = string.Empty;

    public static FlightResponseDto FromEntity(Flight f) => new()
    {
        Id = f.Id,
        FlightNumber = f.FlightNumber,
        DepartureAirportCode = f.DepartureAirport?.Code ?? "",
        DepartureCity = f.DepartureAirport?.City ?? "",
        ArrivalAirportCode = f.ArrivalAirport?.Code ?? "",
        ArrivalCity = f.ArrivalAirport?.City ?? "",
        DepartureTimeUtc = f.DepartureTimeUtc,
        ArrivalTimeUtc = f.ArrivalTimeUtc,
        AirplaneModel = f.Airplane?.Model ?? "",
        EconomyPrice = f.EconomyPrice,
        BusinessPrice = f.BusinessPrice,
        FirstClassPrice = f.FirstClassPrice,
        EconomySeatsAvailable = f.EconomySeatsAvailable,
        BusinessSeatsAvailable = f.BusinessSeatsAvailable,
        FirstClassSeatsAvailable = f.FirstClassSeatsAvailable,
        Status = f.Status.ToString()
    };
}

public class CreateFlightDto
{
    public string FlightNumber { get; set; } = string.Empty;
    public int DepartureAirportId { get; set; }
    public int ArrivalAirportId { get; set; }
    public int AirplaneId { get; set; }
    public DateTime DepartureTimeUtc { get; set; }
    public DateTime ArrivalTimeUtc { get; set; }
    public decimal EconomyPrice { get; set; }
    public decimal BusinessPrice { get; set; }
    public decimal FirstClassPrice { get; set; }
}
