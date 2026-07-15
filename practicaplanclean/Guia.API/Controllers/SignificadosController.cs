using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Guia.API.Data; 
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class SignificadosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SignificadosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ESTA ES LA RUTA QUE LLAMA TU JAVASCRIPT: api/significados/detalle/{temaId}/{numero}
    [HttpGet("detalle/{temaId}/{numero}")]
    public async Task<IActionResult> GetDetalle(int temaId, int numero)
    {
        // Buscamos el significado y traemos también el Tema para la descripción general
        var resultado = await _context.Significados
            .Include(s => s.Tema) 
            .FirstOrDefaultAsync(s => s.TemaId == temaId && s.ValorNumero == numero);

        if (resultado == null)
        {
            return NotFound("No se encontró sabiduría para este número.");
        }

        // Devolvemos un objeto plano que el JavaScript entienda fácilmente
        return Ok(new
        {
            apodo = resultado.Apodo,
            mision = resultado.Mision,
            reto = resultado.Reto,
            mantra = resultado.Mantra,
            amuleto = resultado.Amuleto,
            mensajeMagico = resultado.MensajeMagico,
            // Aquí enviamos la frase del DbInitializer que querías mostrar
            descripcionTema = resultado.Tema?.DescripcionGeneral ?? "Descripción no disponible"
        });
    }
}