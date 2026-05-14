# SistemaGuiaTaylor - Project Context

This project is a **.NET 8.0 Web API** designed to provide spiritual and personal growth guidance. It integrates esoteric systems like Numerology, Astrology, and Kabbalah (Tree of Life) into a cohesive digital experience.

## Project Overview

- **Core Technology:** .NET 8.0 (C#)
- **Database:** SQL Server using Entity Framework Core.
- **Architecture:** Controller-based REST API with a focused domain model.
- **Frontend:** A lightweight web interface using HTML/Vanilla JS, served directly from the `Guia.API/wwwroot` folder.
- **Key Domains:**
  - **Numerology:** Pythagorean system calculations (Mission, Soul, Destiny, Personality).
  - **Astrology:** Zodiac signs, Lunar phases, and Elements based on birth data.
  - **Kabbalah:** Calculation of the Tree of Life (10 Sefirot and 22 Paths) using a Base 22 reduction system.
  - **Daily Engagement:** Gratitude practices, daily vibration tracking, and weekly challenges.

## Directory Structure

- `Guia.API/`: Main project folder.
  - `Controllers/`: API endpoints handling business logic (e.g., `PersonasController`, `AstroController`).
  - `Models/`: Data structures and domain entities.
  - `Data/`: DB Context (`ApplicationDbContext`) and seeders.
  - `wwwroot/`: Frontend assets (HTML, CSS, JS).
- `Guia.Web/`: Placeholder or auxiliary web components (mostly empty, use `wwwroot` for the active frontend).
- `Script/`: SQL scripts for initial table creation and data population.
- `Migrations/`: EF Core database migration history.

## Building and Running

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or instance specified in `appsettings.json`)

### Common Commands
- **Restore Dependencies:** `dotnet restore`
- **Build Project:** `dotnet build`
- **Run API:** `dotnet run --project Guia.API`
- **Watch Mode:** `dotnet watch --project Guia.API`
- **Database Update:** `dotnet ef database update --project Guia.API`
- **Add Migration:** `dotnet ef migrations add <MigrationName> --project Guia.API`

## Development Conventions

- **Calculation Logic:** Most esoteric calculations are implemented as private methods within controllers (specifically `PersonasController.cs`).
- **Data Integrity:** Relationships between `Persona` and its details (`PersonaDetalle`, `PersonaNumerologia`, `ArbolVida`) are typically 1-to-1.
- **Naming:** Follows standard C# / .NET conventions (PascalCase for properties/methods).
- **Frontend:** Uses `fetch` to interact with the API. The `wwwroot/index.html` is the primary entry point for registration/login.

## Security
- `SECURITY_HARDENING_PLAN.md` contains ongoing security tasks.
- Authentication currently uses simple username/password comparison (future plans for hashing are noted in the code).

## Project Notes
- The API is configured to listen on port `1651` by default (see `Program.cs`).
- CORS is enabled to allow all origins, headers, and methods.
- The project includes a `DbInitializer` to automatically seed the database on startup if configured.
