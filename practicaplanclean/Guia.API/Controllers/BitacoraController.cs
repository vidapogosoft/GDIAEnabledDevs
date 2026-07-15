using Guia.API.Data;
using Guia.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class BitacoraController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BitacoraController(ApplicationDbContext context) { _context = context; }

    [HttpPost("registrar")]
    public async Task<ActionResult> RegistrarEntrada(Bitacora entrada)
    {
        _context.Bitacoras.Add(entrada);
        await _context.SaveChangesAsync();
        return Ok(new { id = entrada.Id, mensaje = "Entrada guardada en tu bitácora sagrada" });
    }

    // ESTA ES LA ÚNICA VERSIÓN QUE DEBES TENER
    [HttpGet("mis-registros/{personaId}")]
    public async Task<ActionResult> GetMisRegistros(int personaId)
    {
        var historial = await _context.Bitacoras
            .Where(b => b.PersonaId == personaId)
            .OrderByDescending(b => b.Fecha) 
            .Select(b => new {
                b.Id,
                b.Fecha,
                b.Tipo,
                b.Contenido,
                b.RetoId,           // CRUCIAL para agrupar en el perfil.html
                b.EstadoReto,       // Para saber si el reto está completado
                b.ValorSincronico   // Para las señales numéricas (11:11, etc)
            })
            .ToListAsync();

        if (historial == null || !historial.Any())
        {
            return Ok(new List<object>()); // Devolvemos lista vacía en lugar de error
        }

        return Ok(historial);
    }

    [HttpPut("finalizar-reto/{id}")]
    public async Task<ActionResult> FinalizarReto(int id)
    {
        
        var registro = await _context.Bitacoras.FindAsync(id);

        if (registro == null) return NotFound(new { mensaje = "Registro no encontrado" });

        // Cambiamos el estado para que deje de aparecer como 'En Curso'
        registro.EstadoReto = "Completado";
        if (registro.Reto != null)
        {
            // Asumiendo que tu campo se llama FechaFin en el modelo RetoSemanal
            registro.Reto.FechaFin = DateTime.Now; 
        }
        
        await _context.SaveChangesAsync();

        return Ok(new { 
            mensaje = "¡Felicidades! Has completado una etapa de tu transformación.",
            recompensa = "Has sembrado una semilla de luz en tu camino." 
        });
    }

    [HttpGet("historial/{personaId}")]
    public async Task<ActionResult> ObtenerHistorial(int personaId)
    {
        var historial = await _context.Bitacoras
            .Where(b => b.PersonaId == personaId)
            .OrderByDescending(b => b.Fecha) // Los más nuevos primero
            .Select(b => new {
                b.Id,
                b.Fecha,
                b.Tipo,
                b.Contenido,
                b.RetoId, // <--- CRUCIAL: Debe ir en el objeto que enviamos al JS
                b.EstadoReto
            })
            .ToListAsync();

        return Ok(historial);
    }

    [HttpGet("reto-activo/{personaId}")]
    public async Task<ActionResult> ObtenerRetoActivo(int personaId)
    {
        // Buscamos en Bitacora el último registro "En Curso" para esta persona
        var retoActivo = await _context.Bitacoras
            .Where(b => b.PersonaId == personaId && b.EstadoReto == "En Curso")
            .OrderByDescending(b => b.Fecha)
            .Select(b => new {
                b.Id, // El ID de la bitácora para finalizarlo luego
                b.RetoId,
                // Aquí podrías incluir un Join para traer el título del Reto
                Titulo = _context.RetosSemanales.Where(r => r.Id == b.RetoId).Select(r => r.Titulo).FirstOrDefault(),
                Descripcion = _context.RetosSemanales.Where(r => r.Id == b.RetoId).Select(r => r.Descripcion).FirstOrDefault(),
                Instrucciones = _context.RetosSemanales.Where(r => r.Id == b.RetoId).Select(r => r.Instrucciones).FirstOrDefault()
            })
            .FirstOrDefaultAsync();

        if (retoActivo == null) return NotFound();

        return Ok(retoActivo);
    }
    
}