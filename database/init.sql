-- ============================================================
-- Airline System - Reference schema (PostgreSQL)
-- NOTE: The .NET backend creates this schema automatically on
-- first run via EF Core (DbContext.Database.EnsureCreated()).
-- This script is provided for reference / manual setup, e.g.
-- if you want to inspect or seed the DB without running the API.
-- ============================================================

CREATE TABLE IF NOT EXISTS "Airports" (
    "Id" SERIAL PRIMARY KEY,
    "Code" VARCHAR(3) NOT NULL UNIQUE,
    "Name" VARCHAR(150) NOT NULL,
    "City" VARCHAR(100) NOT NULL,
    "Country" VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS "Airplanes" (
    "Id" SERIAL PRIMARY KEY,
    "TailNumber" VARCHAR(50) NOT NULL UNIQUE,
    "Model" VARCHAR(100) NOT NULL,
    "EconomySeats" INT NOT NULL DEFAULT 0,
    "BusinessSeats" INT NOT NULL DEFAULT 0,
    "FirstClassSeats" INT NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS "Flights" (
    "Id" SERIAL PRIMARY KEY,
    "FlightNumber" VARCHAR(10) NOT NULL,
    "DepartureAirportId" INT NOT NULL REFERENCES "Airports"("Id"),
    "ArrivalAirportId" INT NOT NULL REFERENCES "Airports"("Id"),
    "AirplaneId" INT NOT NULL REFERENCES "Airplanes"("Id"),
    "DepartureTimeUtc" TIMESTAMP NOT NULL,
    "ArrivalTimeUtc" TIMESTAMP NOT NULL,
    "EconomyPrice" DECIMAL(10,2) NOT NULL DEFAULT 0,
    "BusinessPrice" DECIMAL(10,2) NOT NULL DEFAULT 0,
    "FirstClassPrice" DECIMAL(10,2) NOT NULL DEFAULT 0,
    "EconomySeatsAvailable" INT NOT NULL DEFAULT 0,
    "BusinessSeatsAvailable" INT NOT NULL DEFAULT 0,
    "FirstClassSeatsAvailable" INT NOT NULL DEFAULT 0,
    "Status" VARCHAR(20) NOT NULL DEFAULT 'Scheduled'
);

CREATE INDEX IF NOT EXISTS idx_flights_flightnumber ON "Flights" ("FlightNumber");

CREATE TABLE IF NOT EXISTS "Passengers" (
    "Id" SERIAL PRIMARY KEY,
    "FirstName" VARCHAR(100) NOT NULL,
    "LastName" VARCHAR(100) NOT NULL,
    "PassportNumber" VARCHAR(20) NOT NULL,
    "DateOfBirth" TIMESTAMP NOT NULL,
    "Email" VARCHAR(150),
    "Phone" VARCHAR(30)
);

CREATE TABLE IF NOT EXISTS "Users" (
    "Id" SERIAL PRIMARY KEY,
    "Email" VARCHAR(150) NOT NULL UNIQUE,
    "PasswordHash" TEXT NOT NULL,
    "FirstName" VARCHAR(100) NOT NULL,
    "LastName" VARCHAR(100) NOT NULL,
    "Role" VARCHAR(20) NOT NULL DEFAULT 'Customer',
    "CreatedAtUtc" TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS "Bookings" (
    "Id" SERIAL PRIMARY KEY,
    "BookingReference" VARCHAR(12) NOT NULL UNIQUE,
    "FlightId" INT NOT NULL REFERENCES "Flights"("Id"),
    "UserId" INT REFERENCES "Users"("Id") ON DELETE SET NULL,
    "Status" VARCHAR(20) NOT NULL DEFAULT 'Pending',
    "CreatedAtUtc" TIMESTAMP NOT NULL DEFAULT NOW(),
    "TotalPrice" DECIMAL(10,2) NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS "BookingPassengers" (
    "Id" SERIAL PRIMARY KEY,
    "BookingId" INT NOT NULL REFERENCES "Bookings"("Id") ON DELETE CASCADE,
    "PassengerId" INT NOT NULL REFERENCES "Passengers"("Id"),
    "SeatClass" VARCHAR(20) NOT NULL,
    "SeatNumber" VARCHAR(5),
    "PricePaid" DECIMAL(10,2) NOT NULL DEFAULT 0
);

-- Sample reference data (optional - the API also seeds this automatically)
INSERT INTO "Airports" ("Code", "Name", "City", "Country") VALUES
    ('JFK', 'John F. Kennedy International', 'New York', 'USA'),
    ('LAX', 'Los Angeles International', 'Los Angeles', 'USA'),
    ('LHR', 'Heathrow Airport', 'London', 'UK'),
    ('SKP', 'Skopje International', 'Skopje', 'North Macedonia'),
    ('CDG', 'Charles de Gaulle', 'Paris', 'France')
ON CONFLICT ("Code") DO NOTHING;
