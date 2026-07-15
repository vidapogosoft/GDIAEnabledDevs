# Repository Guidelines

## Project Structure & Module Organization
- `SistemaGuia.sln` is the main solution entry point.
- `Guia.API/` contains the ASP.NET Core API (`net8.0`).
- `Guia.API/Controllers/` holds HTTP endpoints; keep one domain area per controller.
- `Guia.API/Models/` defines entities/DTOs; `Data/` contains `ApplicationDbContext` and seeders.
- `Guia.API/Migrations/` stores EF Core migration history.
- `Guia.API/wwwroot/` contains static files and images served by the API.
- `Script/` includes SQL scripts for schema/data setup.
- `Guia.Web/` currently has a minimal frontend placeholder.

## Build, Test, and Development Commands
- `dotnet restore SistemaGuia.sln` restores NuGet dependencies.
- `dotnet build SistemaGuia.sln -c Debug` compiles the solution for local development.
- `dotnet run --project Guia.API/Guia.API.csproj` runs the API (configured in `Program.cs` to listen on port `1651`).
- `dotnet watch --project Guia.API/Guia.API.csproj run` enables hot-reload during API work.
- `dotnet ef database update --project Guia.API` applies pending migrations.

## Coding Style & Naming Conventions
- Use C# conventions: 4-space indentation, braces on new lines, `PascalCase` for types/methods/properties, `camelCase` for local variables.
- Keep controllers thin; move data/query logic to `Data/` or dedicated services.
- Name DTOs with `Dto` suffix and request models with `Request` suffix (for example, `ArbolVidaRequest`).
- Do not commit build artifacts from `bin/` or `obj/`.

## Testing Guidelines
- No test project is currently present. Add tests under a `tests/` directory (for example, `tests/Guia.API.Tests`).
- Prefer `xUnit` for new unit/integration tests.
- Use test names like `MethodName_State_ExpectedResult`.
- Run tests with `dotnet test` from the repository root once tests are added.

## Commit & Pull Request Guidelines
- This workspace does not include `.git` metadata, so no historical commit pattern is available here.
- Use clear, imperative commit messages (example: `Add numerology validation to Personas endpoint`).
- Keep commits focused by concern (API, migrations, scripts).
- PRs should include: purpose, affected endpoints/files, migration impact, and sample request/response when API behavior changes.

## Security & Configuration Tips
- Keep real secrets out of `appsettings*.json`; use environment variables or Secret Manager for local development.
- Review CORS and connection string changes in `Program.cs` before merging.