using Guia.API.Data;
using Guia.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class AstroController : ControllerBase {
    private readonly ApplicationDbContext _context;
    public AstroController(ApplicationDbContext context) => _context = context;

    [HttpGet("detalle/{tipo}/{nombre}")]
    public async Task<IActionResult> GetDetalle(string tipo, string nombre)
    {
        if (string.IsNullOrEmpty(nombre)) return BadRequest("El nombre no puede estar vacío.");

        // Normalizamos el nombre para la búsqueda
        string nombreBusqueda = nombre.Trim();

        if (tipo == "signo")
        {
            var res = await _context.SignosZodiacales
                .FirstOrDefaultAsync(s => s.Nombre.ToLower() == nombreBusqueda.ToLower());
            
            if (res == null) return NotFound(new { mensaje = $"No se encontró el signo {nombre}" });
            return Ok(new {
                    nombre = res.Nombre,
                    icono = res.Icono,
                    descripcion = res.DescripcionLarga, // <--- Aquí le cambiamos el nombre para el JSON
                    elemento = res.Elemento
                });
        }

        if (tipo == "luna")
        {
            var res = await _context.FasesLunares
                .FirstOrDefaultAsync(f => f.Nombre.ToLower() == nombreBusqueda.ToLower());
            
            if (res == null) return NotFound(new { mensaje = $"No se encontró la fase {nombre}" });
            return Ok(res);
        }

        if (tipo == "elemento")
        {
            var res = await _context.ElementosAstro
                .FirstOrDefaultAsync(e => e.Nombre.ToLower() == nombreBusqueda.ToLower());
            
            if (res == null) return NotFound(new { mensaje = $"No se encontró el elemento {nombre}" });
            return Ok(res);
        }

        return BadRequest(new { mensaje = "Tipo de consulta no válido. Usa: signo, luna o elemento." });
    }


    // [HttpGet("detalle/{tipo}/{nombre}")]
    // public async Task<IActionResult> GetDetalleV1(string tipo, string nombre) {
    //     if (tipo == "signo") {
    //         var res = await _context.SignosZodiacales.FirstOrDefaultAsync(s => s.Nombre == nombre);
    //         return Ok(res);
    //     }
    //     if (tipo == "luna") {
    //         var res = await _context.FasesLunares.FirstOrDefaultAsync(f => f.Nombre == nombre);
    //         return Ok(res);
    //     }
    //     // ... elemento lo mismo
    //     return NotFound();
    // }

    [HttpGet("detalle/signo/{nombre}")]
    public async Task<IActionResult> GetSigno(string nombre)
    {
        var signo = await _context.SignosZodiacales
            .FirstOrDefaultAsync(s => s.Nombre.ToLower() == nombre.ToLower());
        
        if (signo == null) return NotFound();

        return Ok(new {
            nombre = signo.Nombre,
            icono = signo.Icono,
            descripcion = signo.DescripcionLarga, // <--- Verifica que el JS use 'descripcion'
            elemento = signo.Elemento,
            palabrasclave = signo.PalabrasClave
        });
    }
}