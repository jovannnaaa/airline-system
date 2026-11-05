# Skyward Air — Airline Booking System

A full-stack airline booking system.

## Contents

- [Features](#features)
- [Tech stack](#tech-stack)
- [Project structure](#project-structure)
- [Quick start (Docker)](#quick-start-docker)
- [Running locally without Docker](#running-locally-without-docker)
- [API reference](#api-reference)
- [Environment variables](#environment-variables)
- [Roadmap / ideas for contributors](#roadmap--ideas-for-contributors)
- [License](#license)

---

## Features

- **Flight search** by route, date, and cabin class (Economy / Business / First)
- **Multi-passenger booking** with live per-cabin seat availability and pricing
- **Booking reference lookup**, cancellation (seats are released back to inventory)
- **JWT authentication** — register / login, bookings linked to accounts
- **Auto-seeded data** — sample airports, aircraft, flights, and an admin user on first run
- **One-command Docker Compose** setup for the full stack

## Tech stack

| Layer      | Technology                                             |
|------------|---------------------------------------------------------|
| Frontend   | React 18, React Router, plain CSS (no UI framework)      |
| Backend    | ASP.NET Core 8 Web API, Entity Framework Core, JWT auth  |
| Database   | PostgreSQL 16 (via Npgsql)                               |
| Auth       | BCrypt password hashing + JWT bearer tokens              |
| Tooling    | Docker & Docker Compose, Swagger / OpenAPI               |

## Project structure

```
airline-system/
├── backend/                    ASP.NET Core Web API
│   └── AirlineSystem.API/
│       ├── Controllers/        Airports, Flights, Bookings, Airplanes, Auth
│       ├── Models/             EF Core entities
│       ├── DTOs/                Request/response contracts
│       ├── Data/               DbContext + seed data
│       └── Program.cs
├── frontend/                   React app
│   └── src/
│       ├── api/                Fetch-based API client
│       ├── components/         Search form, flight list, boarding pass, etc.
│       ├── pages/               Home, Booking, Confirmation, Manage, Auth
│       └── context/            Auth context
├── database/
│   └── init.sql                Reference schema (EF Core creates this automatically)
├── docker-compose.yml           Runs db + api + frontend together
└── README.md
```

## Quick start (Docker)

```bash
git clone https://github.com/<your-username>/airline-system.git
cd airline-system
docker compose up --build
```

| Service   | URL                              |
|-----------|-----------------------------------|
| Frontend  | http://localhost:3000             |
| API       | http://localhost:5000/swagger     |
| Postgres  | localhost:5432 (`airline_db` / `airline_user` / `airline_pass`) |

The API creates the schema and seeds sample data automatically on first run.
Demo admin login: **admin@airline.com** / **Admin123!**

## Running locally without Docker

### 1. PostgreSQL

```sql
CREATE USER airline_user WITH PASSWORD 'airline_pass';
CREATE DATABASE airline_db OWNER airline_user;
```

### 2. Backend

Requires the [.NET 8 SDK](https://dotnet.microsoft.com/download).

```bash
cd backend/AirlineSystem.API
dotnet restore
dotnet run
```

Update `appsettings.json` → `ConnectionStrings:DefaultConnection` if your
Postgres credentials differ. Swagger UI is available at `/swagger` in
Development mode.

### 3. Frontend

Requires [Node.js 18+](https://nodejs.org/).

```bash
cd frontend
cp .env.example .env.local   # set REACT_APP_API_URL if needed
npm install
npm start
```

## API reference

| Method | Endpoint                          | Description                        |
|--------|-------------------------------------|--------------------------------------|
| GET    | `/api/airports`                    | List airports                        |
| GET    | `/api/flights/search`              | Search flights (route/date/cabin)    |
| GET    | `/api/flights/{id}`                | Flight details                       |
| POST   | `/api/bookings`                    | Create a booking (with passengers)   |
| GET    | `/api/bookings/reference/{ref}`    | Look up a booking by reference       |
| PUT    | `/api/bookings/{id}/cancel`        | Cancel a booking (releases seats)    |
| POST   | `/api/auth/register`               | Create a customer account            |
| POST   | `/api/auth/login`                  | Sign in, returns a JWT               |

Full interactive docs are available via Swagger at `/swagger` when running
the API in Development mode.

## Environment variables

**Backend** (`appsettings.json` or environment overrides):

| Key                                   | Description                          |
|-----------------------------------------|----------------------------------------|
| `ConnectionStrings__DefaultConnection`  | Postgres connection string             |
| `Jwt__Key`                              | JWT signing secret — **change before deploying** |
| `Cors__AllowedOrigins__0`               | Allowed frontend origin for CORS       |

**Frontend** (`.env.local`):

| Key                    | Description                     |
|--------------------------|----------------------------------|
| `REACT_APP_API_URL`     | Base URL of the backend API      |

## Roadmap / ideas for contributors

- [ ] Real EF Core migrations (`dotnet ef migrations add InitialCreate`) instead of `EnsureCreated`
- [ ] Seat map UI with actual seat selection (not just cabin class)
- [ ] Payment integration (Stripe test mode)
- [ ] Admin dashboard for managing flights/airplanes
- [ ] Unit/integration tests (xUnit for API, React Testing Library for frontend)
- [ ] Rate limiting and stricter input validation
- [ ] CI pipeline (GitHub Actions) for build/test on PRs
