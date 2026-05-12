using Guia.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class RetosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RetosController(ApplicationDbContext context) { _context = context; }

    

    [HttpGet("verificar-activo/{personaId}")]
    public async Task<ActionResult> VerificarRetoActivo(int personaId)
    {
        // Buscamos si hay algún registro de tipo 'Reto' que esté 'En Curso'
        var retoActivo = await _context.Bitacoras
            .Include(b => b.Reto)
            .Where(b => b.PersonaId == personaId && b.EstadoReto == "En Curso")
            .OrderByDescending(b => b.Fecha)
            .FirstOrDefaultAsync();

        if (retoActivo != null && retoActivo.Reto != null)
        {
            return Ok(new { 
                id = retoActivo.RetoId,
                titulo = retoActivo.Reto.Titulo,
                descripcion = retoActivo.Reto.Descripcion,
                estado = "En Curso"
            });
        }

        return NotFound(new { mensaje = "No tienes retos activos." });
    }

    [HttpGet("obtener-estado/{personaId}")]
    public async Task<ActionResult> ObtenerEstadoReto(int personaId)
    {   
        var hoy = DateTime.Today;
        int limiteMaximo = 1; // <--- Aquí defines cuántos retos quieres permitir
        // 1. Buscamos si hay un reto "En Curso" en la bitácora
        var registroActivo = await _context.Bitacoras
            .Include(b => b.Reto) // Carga los datos de la tabla RetosSemanales
            .Where(b => b.PersonaId == personaId && b.EstadoReto == "En Curso")
            .OrderByDescending(b => b.Fecha)
            .FirstOrDefaultAsync();

        if (registroActivo != null && registroActivo.Reto != null)
        {
            return Ok(new {
                tieneRetos= true, // <--- Control para el JS
                id = registroActivo.RetoId,
                titulo = registroActivo.Reto.Titulo,
                descripcion = registroActivo.Reto?.Descripcion ?? "Continúa con tu actividad",
                estado = "En Curso",
                instrucciones= _context.RetosSemanales.Where(r => r.Id == registroActivo.RetoId).Select(r => r.Instrucciones).FirstOrDefault(),
                bitacoraId = registroActivo.Id
            });
        }

        // --- NUEVO BLOQUE DE CONTROL DE LÍMITE ---
            // Si llegamos aquí es porque no tiene retos activos. 
            // Contamos cuántos completó u omitió hoy para ver si le sugerimos otro.
            int retosHechosHoy = await _context.Bitacoras
                .CountAsync(b => b.PersonaId == personaId 
                            && b.Tipo == "Reto" 
                            && b.Fecha >= hoy 
                            && (b.EstadoReto == "Completado" || b.EstadoReto == "Omitido"));

            if (retosHechosHoy >= limiteMaximo)
            {
                // Al devolver NotFound, tu JavaScript ocultará el contenedor automáticamente
                //return NotFound(new { mensaje = "Has cumplido tus retos por hoy. ¡Mañana más!" });
                // Ya no es NotFound, es un Ok que dice "ya no más por hoy"
                return Ok(new { tieneRetos = false, mensaje = "Has cumplido tus retos por hoy. ¡Mañana más!" });
            }
            // ----------------------------------------

//el reto global aun esta en desarrollo, por eso lo comento para no generar confusión. La idea es que el sistema revise primero si hay un reto "En Curso" (prioridad máxima), luego revise si hay un reto global activo (prioridad media) y finalmente use la IA por sentimientos (prioridad baja).
        // // 2. SEGUNDA PRIORIDAD: ¿Hay un Reto Global (Suscripción) activo?
        // // Esto es lo que tú creas como administrador para todos
        // var retoGlobal = await _context.RetosSemanales
        //     .Where(r => r.Activo && r.EsGlobal)
        //     .OrderByDescending(r => r.Id) // El más reciente
        //     .FirstOrDefaultAsync();

        // if (retoGlobal != null)
        // {
        //     return Ok(new { 
        //         id = retoGlobal.Id,
        //         titulo = retoGlobal.Titulo,
        //         descripcion = retoGlobal.Descripcion,
        //         estado = "Sugerido" 
        //     });
        // }

        // 3. TERCERA PRIORIDAD: IA por sentimientos ( lógica original por sentimientos)
        var ultimoSentir = await _context.Bitacoras
            .Where(b => b.PersonaId == personaId && b.Tipo == "Sentimiento")
            .OrderByDescending(b => b.Fecha)
            .FirstOrDefaultAsync();

        if (ultimoSentir == null) {
            //return NotFound(new { mensaje = "Escribe cómo te sientes en tu bitácora para sugerirte un reto.",requiereEscritura = true});
            return Ok(new { tieneRetos = false, requiereEscritura = true, mensaje = "Escribe cómo te sientes en tu bitácora para sugerirte un reto." });
        }
        // Lógica de palabras clave (la que ya teníamos)
        string contenido = ultimoSentir.Contenido.ToLower();
        string busqueda = "Paz"; // Valor por defecto
        // string busqueda = ultimoSentir.Contenido.ToLower().Contains("paz") ? "Paz" : "Alegría"; 
        if (contenido.Contains("triste") || contenido.Contains("solo") || contenido.Contains("llorar"))
            busqueda = "Gratitud"; 

        else if (contenido.Contains("estrés") || contenido.Contains("ansiedad") || contenido.Contains("miedo"))
            busqueda = "Tierra"; 

        else if (contenido.Contains("cansado") || contenido.Contains("aburrido") || contenido.Contains("bloqueo"))
            busqueda = "Movimiento";

        else if (contenido.Contains("feliz") || contenido.Contains("bien") || contenido.Contains("excelente"))
            busqueda = "Silencio"; // Reto de mantenimiento de vibración alta
        
        else if (contenido.Contains("perdido") || contenido.Contains("desorientado") || contenido.Contains("no sé qué hacer"))
            busqueda = "Faro"; 

        else if (contenido.Contains("falta algo") || contenido.Contains("vacío") || contenido.Contains("busco"))
            busqueda = "Futuro";

        else if (contenido.Contains("algo más") || contenido.Contains("misterio") || contenido.Contains("señal"))
            busqueda = "Sincronicidad";

        else if (contenido.Contains("enojado") || contenido.Contains("furia") || contenido.Contains("molesto"))
            busqueda = "Escudo";

        var sugerencia = await _context.RetosSemanales
            .Where(r => r.Activo && (r.Titulo.Contains(busqueda) || r.Descripcion.Contains(busqueda)))
            .FirstOrDefaultAsync();

        if (sugerencia == null) 
        {
            //return NotFound(new { mensaje = "No hay retos disponibles en este momento." });
            return Ok(new { tieneRetos = false, mensaje = "No hay retos disponibles en este momento." });
        }
        return Ok(new { 
            tieneRetos = true, // <--- Control para el JS
            id = sugerencia.Id,
            titulo = sugerencia.Titulo,
            descripcion = sugerencia.Descripcion,
            estado = "Sugerido",
            instrucciones= sugerencia.Instrucciones,
            esGlobal = false
        });
    }

}