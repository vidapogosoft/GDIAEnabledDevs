using Microsoft.EntityFrameworkCore;
using Guia.API.Data;
using Guia.API.Data.Seeders;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de Red y Puerto
builder.WebHost.ConfigureKestrel(options =>
{
    
    options.Listen(System.Net.IPAddress.Any, 1651);
});

// 2. Registro de Servicios
builder.Services.AddControllers(); // ¡ESTA ES LA QUE FALTABA!
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
/*builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
*/
/* sl server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => {
            // Esto soluciona el error de "Transient Failure"
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
        }));
        */

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString,
        sqlOptions => {
            // Esto soluciona el error de "Transient Failure"
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
        }));
// --- IMPORTANTE: Configurar CORS ---
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// 3. Ejecución del Seeder (Base de Datos)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error al sembrar la base de datos.");
    }
}


// 3.1 Middlewares
app.UseCors(); // Activar CORS
app.UseDefaultFiles(); // Busca index.html por defecto
app.UseStaticFiles();  // Permite servir archivos de la carpeta wwwroot

// 4. Pipeline de la Aplicación
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 5. Activación de Rutas
app.MapControllers(); // ¡ESTA TAMBIÉN ES VITAL!

// Puedes dejar o borrar el weatherforecast, no estorba.
app.MapGet("/", () => "API Guía de Transformación Activa"); 

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
