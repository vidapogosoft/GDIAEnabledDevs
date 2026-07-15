# Análisis Arquitectónico (estado actual)

Proyecto **monolítico ASP.NET Core Web API (.NET 8)** con EF Core + MySQL.  
El núcleo está en `Guia.API/Program.cs`, `Guia.API/Data/ApplicationDbContext.cs` y controladores en `Guia.API/Controllers`.

## Capas identificadas

1. **Presentación/API**
- Controllers: `Personas`, `Numerologia`, `Astro`, `Bitacora`, `Retos`, `Significados`.
- Endpoints REST con `[ApiController]` y rutas `api/[controller]`.

2. **Aplicación (lógica de negocio)**
- La lógica vive principalmente dentro de controladores (especialmente `PersonasController` y `NumerologiaController`).
- No hay capa `Services` separada.

3. **Dominio**
- Entidades en `Guia.API/Models`:  
`Persona`, `PersonaDetalle`, `PersonaNumerologia`, `ArbolVida`, `Bitacora`, `RetoSemanal`, `Significado`, `Tema`, `Arcano`, `Arquetipo`, `SignoZodiacal`, `FaseLunar`, `ElementoAstro`, `FraseGratitud`, `ArbolVidaRequest`, `DatosNumerologiaDto` (+ `ArbolVida copy`).

4. **Infraestructura/Persistencia**
- EF Core DbContext + Migrations + Seeder (`DbInitializer`).
- MySQL por Pomelo.
- Static files en `wwwroot`.

## Patrones de desarrollo usados

- **MVC Web API** (controladores).
- **Dependency Injection** (inyección de `ApplicationDbContext`).
- **Unit of Work implícito** (EF Core `DbContext`).
- **Code First + Migrations + Seed**.
- **Transaction Script** (casos de uso codificados en métodos del controller).
- **DTO parcial** (`ArbolVidaRequest`, `DatosNumerologiaDto`, respuestas anónimas).

## NuGet utilizados

- `Microsoft.AspNetCore.OpenApi` `8.0.25`
- `Microsoft.EntityFrameworkCore.Design` `8.0.10`
- `Microsoft.EntityFrameworkCore.Tools` `8.0.10`
- `Pomelo.EntityFrameworkCore.MySql` `8.0.2`
- `Swashbuckle.AspNetCore` `6.6.2`  
(archivo: `Guia.API.csproj`)

## Flujo del proyecto

1. Arranque en `Program.cs`: Kestrel puerto `1651`, DI, CORS abierto, Swagger en Development.
2. Se crea scope y corre `DbInitializer.Initialize(...)` para asegurar DB/seed.
3. Request HTTP llega al Controller.
4. Controller consulta/actualiza entidades vía `ApplicationDbContext`.
5. EF traduce a SQL MySQL y persiste.
6. Se retorna JSON al frontend (`wwwroot`/clientes externos).

## Observaciones técnicas clave

- Arquitectura funcional pero **acoplada** (lógica de negocio concentrada en controllers).
- No hay capa de servicios ni repositorios explícitos.
- Hay secreto en texto plano en `appsettings.json` (recomendable mover a Secret Manager/variables de entorno).