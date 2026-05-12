# Copilot instructions for SistemaGuia (Guia.API)

## Quick commands
- Restore dependencies: `dotnet restore`
- Build solution: `dotnet build SistemaGuia.sln`
- Run API (Kestrel listens on 1651 as configured in Program.cs):
  - `dotnet run --project Guia.API`  (or from Visual Studio)
  - Explicit URL: `dotnet run --project Guia.API --urls http://localhost:1651`
- EF Core (migrations / DB ops):
  - List migrations: `dotnet ef migrations list --project Guia.API`
  - Apply migrations: `dotnet ef database update --project Guia.API --startup-project Guia.API`

Notes: There are no test projects in the repository. If tests are added: run a single test with
`dotnet test <ProjectOrSolution> --filter "FullyQualifiedName~Namespace.Class.Method"`.

Formatting / linting
- No repo-wide linter configured. Recommended developer tools: `dotnet format` (`dotnet tool install -g dotnet-format`).

## High-level architecture
- Solution: `SistemaGuia.sln`.
- Guia.API (ASP.NET Core Web API, net8.0)
  - Program.cs: hosting, Kestrel port (1651), CORS, static files, Swagger (Dev only) and startup seeding.
  - Data: `ApplicationDbContext` (EF Core, Pomelo MySQL provider). DbSets are declared in `Data/ApplicationDbContext.cs`.
  - Seeders: `Data/Seeders/DbInitializer.cs` — large, idempotent dataset inserted at startup via `EnsureCreated()`.
  - Controllers: `Controllers/*` expose REST endpoints (route pattern `api/[controller]`). Some controllers return anonymous/plain objects expected by the frontend.
  - Static UI: `wwwroot/` holds static HTML and assets served by the API.
- Guia.Web: a minimal static frontend (index.html).

Runtime behaviors to note
- Connection string: `Guia.API/appsettings.json` contains DefaultConnection (MySQL) and currently includes plaintext credentials — treat as sensitive.
- Seeder: extensive domain data (temas, significados, arcanos, etc.) is seeded automatically at startup. It checks for existing records but uses `EnsureCreated()` (not recommended for production).
- CORS: default policy allows any origin, header and method in Program.cs.
- Controllers and frontend: many endpoints return specific JSON shapes (field names in Spanish). Changing response shapes can break existing JS/mobile clients.

## Key conventions and repository-specific notes
- Language & messages: code comments, controller messages and user-facing strings are Spanish. Prefer Spanish for new user-visible strings.
- Route patterns: keep `api/[controller]` conventions and existing path segment styles (`detalle/{temaId}/{numero}`, `refresh-data/{id}`, etc.).
- Seeder is canonical for domain content; avoid duplicating domain constants elsewhere.
- Password handling: currently Personas use plaintext passwords seeded/stored. SECURITY_HARDENING_PLAN.md documents migration to hashed passwords and auth.
- DB provider: Pomelo.EntityFrameworkCore.MySql is used; Program.cs sets MySqlServerVersion to 8.0.x.

## Files to inspect before changing behavior
- `Guia.API/Program.cs` — hosting, CORS, seed behavior
- `Guia.API/appsettings.json` — DefaultConnection (sensitive)
- `Guia.API/Data/Seeders/DbInitializer.cs` — large seed dataset and insert logic
- `Guia.API/Data/ApplicationDbContext.cs` — DbSet declarations and model configuration
- `SECURITY_HARDENING_PLAN.md` — actionable security checklist

## Existing AI assistant / bot configs found
- None detected: (CLAUDE.md, .cursorrules, AGENTS.md, AIDER_CONVENTIONS.md, .windsurfrules, .clinerules not present).

---

Guidance for future Copilot sessions
- Preserve JSON response shapes consumed by frontends when refactoring controllers.
- Review SECURITY_HARDENING_PLAN.md before making auth/DB changes (it contains prioritized steps and file-level instructions).
- When modifying seeding behavior: migrate from `EnsureCreated()` to EF Migrations, and avoid reseeding production on every startup.

If you want the file adjusted (language, more details on controllers, or examples of typical requests/responses), say so and it will be updated.
