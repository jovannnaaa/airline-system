using AirlineSystem.API.Models;

namespace AirlineSystem.API.Data;

/// <summary>
/// Seeds the database with sample airports, airplanes, flights and an admin user
/// so the app is usable immediately after first run.
/// </summary>
public static class DbInitializer
{
    public static void Seed(AirlineDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Airports.Any()) return; // already seeded

        var jfk = new Airport { Code = "JFK", Name = "John F. Kennedy International", City = "New York", Country = "USA" };
        var lax = new Airport { Code = "LAX", Name = "Los Angeles International", City = "Los Angeles", Country = "USA" };
        var lhr = new Airport { Code = "LHR", Name = "Heathrow Airport", City = "London", Country = "UK" };
        var skp = new Airport { Code = "SKP", Name = "Skopje International", City = "Skopje", Country = "North Macedonia" };
        var cdg = new Airport { Code = "CDG", Name = "Charles de Gaulle", City = "Paris", Country = "France" };

        context.Airports.AddRange(jfk, lax, lhr, skp, cdg);

        var plane1 = new Airplane { TailNumber = "N101AS", Model = "Boeing 737-800", EconomySeats = 150, BusinessSeats = 20, FirstClassSeats = 8 };
        var plane2 = new Airplane { TailNumber = "N202AS", Model = "Airbus A320", EconomySeats = 140, BusinessSeats = 16, FirstClassSeats = 0 };
        var plane3 = new Airplane { TailNumber = "N303AS", Model = "Boeing 777-300ER", EconomySeats = 280, BusinessSeats = 42, FirstClassSeats = 12 };

        context.Airplanes.AddRange(plane1, plane2, plane3);
        context.SaveChanges();

        var now = DateTime.UtcNow.Date.AddDays(3).AddHours(9);

        var flights = new List<Flight>
        {
            new()
            {
                FlightNumber = "AS100", DepartureAirportId = jfk.Id, ArrivalAirportId = lax.Id, AirplaneId = plane1.Id,
                DepartureTimeUtc = now, ArrivalTimeUtc = now.AddHours(6),
                EconomyPrice = 249.99m, BusinessPrice = 699.00m, FirstClassPrice = 1299.00m,
                EconomySeatsAvailable = plane1.EconomySeats, BusinessSeatsAvailable = plane1.BusinessSeats, FirstClassSeatsAvailable = plane1.FirstClassSeats
            },
            new()
            {
                FlightNumber = "AS210", DepartureAirportId = lax.Id, ArrivalAirportId = jfk.Id, AirplaneId = plane2.Id,
                DepartureTimeUtc = now.AddDays(1), ArrivalTimeUtc = now.AddDays(1).AddHours(5),
                EconomyPrice = 259.99m, BusinessPrice = 719.00m, FirstClassPrice = 0m,
                EconomySeatsAvailable = plane2.EconomySeats, BusinessSeatsAvailable = plane2.BusinessSeats, FirstClassSeatsAvailable = plane2.FirstClassSeats
            },
            new()
            {
                FlightNumber = "AS330", DepartureAirportId = jfk.Id, ArrivalAirportId = lhr.Id, AirplaneId = plane3.Id,
                DepartureTimeUtc = now.AddDays(2), ArrivalTimeUtc = now.AddDays(2).AddHours(7),
                EconomyPrice = 449.00m, BusinessPrice = 1899.00m, FirstClassPrice = 3599.00m,
                EconomySeatsAvailable = plane3.EconomySeats, BusinessSeatsAvailable = plane3.BusinessSeats, FirstClassSeatsAvailable = plane3.FirstClassSeats
            },
            new()
            {
                FlightNumber = "AS441", DepartureAirportId = lhr.Id, ArrivalAirportId = skp.Id, AirplaneId = plane2.Id,
                DepartureTimeUtc = now.AddDays(3), ArrivalTimeUtc = now.AddDays(3).AddHours(3),
                EconomyPrice = 189.00m, BusinessPrice = 499.00m, FirstClassPrice = 0m,
                EconomySeatsAvailable = plane2.EconomySeats, BusinessSeatsAvailable = plane2.BusinessSeats, FirstClassSeatsAvailable = plane2.FirstClassSeats
            },
            new()
            {
                FlightNumber = "AS512", DepartureAirportId = cdg.Id, ArrivalAirportId = skp.Id, AirplaneId = plane1.Id,
                DepartureTimeUtc = now.AddDays(4), ArrivalTimeUtc = now.AddDays(4).AddHours(2),
                EconomyPrice = 159.00m, BusinessPrice = 399.00m, FirstClassPrice = 799.00m,
                EconomySeatsAvailable = plane1.EconomySeats, BusinessSeatsAvailable = plane1.BusinessSeats, FirstClassSeatsAvailable = plane1.FirstClassSeats
            }
        };

        context.Flights.AddRange(flights);

        var admin = new User
        {
            Email = "admin@airline.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            FirstName = "System",
            LastName = "Admin",
            Role = UserRole.Admin
        };
        context.Users.Add(admin);

        context.SaveChanges();
    }
}
