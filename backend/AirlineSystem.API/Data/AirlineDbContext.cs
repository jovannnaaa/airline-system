using AirlineSystem.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AirlineSystem.API.Data;

public class AirlineDbContext : DbContext
{
    public AirlineDbContext(DbContextOptions<AirlineDbContext> options) : base(options) { }

    public DbSet<Airport> Airports => Set<Airport>();
    public DbSet<Airplane> Airplanes => Set<Airplane>();
    public DbSet<Flight> Flights => Set<Flight>();
    public DbSet<Passenger> Passengers => Set<Passenger>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BookingPassenger> BookingPassengers => Set<BookingPassenger>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Airport>()
            .HasIndex(a => a.Code)
            .IsUnique();

        modelBuilder.Entity<Airplane>()
            .HasIndex(a => a.TailNumber)
            .IsUnique();

        modelBuilder.Entity<Flight>()
            .HasIndex(f => f.FlightNumber);

        modelBuilder.Entity<Flight>()
            .HasOne(f => f.DepartureAirport)
            .WithMany(a => a.DepartingFlights)
            .HasForeignKey(f => f.DepartureAirportId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Flight>()
            .HasOne(f => f.ArrivalAirport)
            .WithMany(a => a.ArrivingFlights)
            .HasForeignKey(f => f.ArrivalAirportId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Flight>()
            .HasOne(f => f.Airplane)
            .WithMany(a => a.Flights)
            .HasForeignKey(f => f.AirplaneId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasIndex(b => b.BookingReference)
            .IsUnique();

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Flight)
            .WithMany(f => f.Bookings)
            .HasForeignKey(b => b.FlightId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<BookingPassenger>()
            .HasOne(bp => bp.Booking)
            .WithMany(b => b.BookingPassengers)
            .HasForeignKey(bp => bp.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BookingPassenger>()
            .HasOne(bp => bp.Passenger)
            .WithMany(p => p.BookingPassengers)
            .HasForeignKey(bp => bp.PassengerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Store enums as strings for readability in the DB
        modelBuilder.Entity<Flight>().Property(f => f.Status).HasConversion<string>();
        modelBuilder.Entity<Booking>().Property(b => b.Status).HasConversion<string>();
        modelBuilder.Entity<BookingPassenger>().Property(bp => bp.SeatClass).HasConversion<string>();
        modelBuilder.Entity<User>().Property(u => u.Role).HasConversion<string>();
    }
}
