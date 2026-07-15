using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Guia.API.Data;
using Guia.API.Models;
using System;

namespace Guia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // La URL será: api/numerologia
    public class NumerologiaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NumerologiaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/numerologia/significado/1/1
        // (Donde el primer 1 es el TemaId y el segundo es el ValorNumero)
        [HttpGet("significado/{temaId}/{numero}")]
        public async Task<ActionResult<Significado>> GetSignificado(int temaId, int numero)
        {
           // Esto imprimirá en tu terminal de VS Code cada vez que entres a la URL
            Console.WriteLine($"---> Petición recibida: Tema {temaId}, Numero {numero}"); 
            
            var resultado = await _context.Significados
                .FirstOrDefaultAsync(s => s.TemaId == temaId && s.ValorNumero == numero);
            if (resultado == null)
            {
                Console.WriteLine("---> No se encontró nada en la DB.");
                return NotFound(new { mensaje = "Aún no tenemos información para ese número." });
            }

            Console.WriteLine($"---> Datos encontrados: {resultado.Apodo}");
            return Ok(resultado);
        }

        // GET: api/numerologia/temas
        [HttpGet("temas")]
        public async Task<ActionResult<IEnumerable<Tema>>> GetTemas()
        {
            return await _context.Temas.Where(t => t.EstaActivo).ToListAsync();
        }

        [HttpPost("calcular-arbol")]
    public async Task<IActionResult> CalcularArbolDeLaVida([FromBody] ArbolVidaRequest request)
    {
        if (request == null || request.Persona == null || request.Datos == null)
        {
            return BadRequest("Datos insuficientes para el cálculo.");
        }

        // Extraemos los datos del request
        var persona = request.Persona;
        var dn = request.Datos;

        // Creamos el objeto ArbolVida
        var arbol = new ArbolVida
        {
            PersonaId = persona.Id,
            FechaCalculo = DateTime.Now,

            // --- SEFIROT ---
            Kether_Valor = dn.MisionVida,
            Kether_Nombre = "Misión de Vida",

            Chokmah_Valor = ReducirNumero(persona.FechaNacimiento.Year),
            Chokmah_Nombre = "Dones Pasados",

            Binah_Valor = ReducirNumero(persona.FechaNacimiento.Month),
            Binah_Nombre = "Karma / Tikún",

            Chesed_Valor = ReducirNumero(persona.FechaNacimiento.Day + persona.FechaNacimiento.Month),
            Chesed_Nombre = "Abundancia",

            Gevurah_Valor = ReducirNumero(Math.Abs(persona.FechaNacimiento.Day - persona.FechaNacimiento.Month)),
            Gevurah_Nombre = "Desafíos",

            Tiferet_Valor = dn.NumeroAlma,
            Tiferet_Nombre = "Esencia",

            Netzach_Valor = ReducirNumero(persona.FechaNacimiento.Day),
            Netzach_Nombre = "Victoria",

            Hod_Valor = dn.NumeroPersonalidad,
            Hod_Nombre = "Mente",

            Yesod_Valor = ReducirNumero(dn.NumeroAlma + dn.NumeroPersonalidad),
            Yesod_Nombre = "Raíz",

            Malchut_Valor = dn.RegaloDivino,
            Malchut_Nombre = "Realización",

            // --- SENDEROS COMPLETOS ---
            Sendero_1_2 = CalcularArcanoSendero(dn.MisionVida, ReducirNumero(persona.FechaNacimiento.Year)),
            Sendero_1_3 = CalcularArcanoSendero(dn.MisionVida, ReducirNumero(persona.FechaNacimiento.Month)),
            Sendero_1_6 = CalcularArcanoSendero(dn.MisionVida, dn.NumeroAlma),
            Sendero_2_3 = CalcularArcanoSendero(ReducirNumero(persona.FechaNacimiento.Year), ReducirNumero(persona.FechaNacimiento.Month)),
            Sendero_2_4 = CalcularArcanoSendero(ReducirNumero(persona.FechaNacimiento.Year), ReducirNumero(persona.FechaNacimiento.Day + persona.FechaNacimiento.Month)),
            Sendero_2_6 = CalcularArcanoSendero(ReducirNumero(persona.FechaNacimiento.Year), dn.NumeroAlma),
            Sendero_3_5 = CalcularArcanoSendero(ReducirNumero(persona.FechaNacimiento.Month), ReducirNumero(Math.Abs(persona.FechaNacimiento.Day - persona.FechaNacimiento.Month))),
            Sendero_3_6 = CalcularArcanoSendero(ReducirNumero(persona.FechaNacimiento.Month), dn.NumeroAlma),
            Sendero_4_5 = CalcularArcanoSendero(ReducirNumero(persona.FechaNacimiento.Day + persona.FechaNacimiento.Month), ReducirNumero(Math.Abs(persona.FechaNacimiento.Day - persona.FechaNacimiento.Month))),
            Sendero_4_6 = CalcularArcanoSendero(ReducirNumero(persona.FechaNacimiento.Day + persona.FechaNacimiento.Month), dn.NumeroAlma),
            Sendero_4_7 = CalcularArcanoSendero(ReducirNumero(persona.FechaNacimiento.Day + persona.FechaNacimiento.Month), ReducirNumero(persona.FechaNacimiento.Day)),
            Sendero_5_6 = CalcularArcanoSendero(ReducirNumero(Math.Abs(persona.FechaNacimiento.Day - persona.FechaNacimiento.Month)), dn.NumeroAlma),
            Sendero_5_8 = CalcularArcanoSendero(ReducirNumero(Math.Abs(persona.FechaNacimiento.Day - persona.FechaNacimiento.Month)), dn.NumeroPersonalidad),
            Sendero_6_7 = CalcularArcanoSendero(dn.NumeroAlma, ReducirNumero(persona.FechaNacimiento.Day)),
            Sendero_6_8 = CalcularArcanoSendero(dn.NumeroAlma, dn.NumeroPersonalidad),
            Sendero_6_9 = CalcularArcanoSendero(dn.NumeroAlma, ReducirNumero(dn.NumeroAlma + dn.NumeroPersonalidad)),
            Sendero_7_8 = CalcularArcanoSendero(ReducirNumero(persona.FechaNacimiento.Day), dn.NumeroPersonalidad),
            Sendero_7_9 = CalcularArcanoSendero(ReducirNumero(persona.FechaNacimiento.Day), ReducirNumero(dn.NumeroAlma + dn.NumeroPersonalidad)),
            Sendero_7_10 = CalcularArcanoSendero(ReducirNumero(persona.FechaNacimiento.Day), dn.RegaloDivino),
            Sendero_8_9 = CalcularArcanoSendero(dn.NumeroPersonalidad, ReducirNumero(dn.NumeroAlma + dn.NumeroPersonalidad)),
            Sendero_8_10 = CalcularArcanoSendero(dn.NumeroPersonalidad, dn.RegaloDivino),
            Sendero_9_10 = CalcularArcanoSendero(ReducirNumero(dn.NumeroAlma + dn.NumeroPersonalidad), dn.RegaloDivino)
        };

        // --- PERSISTENCIA EN BASE DE DATOS ---
        try
        {
            // 1. Verificamos si ya existe un árbol para esta persona para actualizarlo o crear uno nuevo
            var arbolExistente = await _context.ArbolesVida.FirstOrDefaultAsync(a => a.PersonaId == persona.Id);
            
            if (arbolExistente != null)
            {
                // Opcional: Eliminar el anterior o actualizar los campos
                _context.ArbolesVida.Remove(arbolExistente);
            }

            // 2. Agregamos el nuevo registro
            _context.ArbolesVida.Add(arbol);
            
            // 3. Guardamos cambios
            await _context.SaveChangesAsync();

            return Ok(arbol);
        }
        catch (Exception ex)
        {
            // Loguear el error si es necesario
            return StatusCode(500, $"Error interno al guardar en la base de datos: {ex.Message}");
        }
    }

        // [HttpPost("calcular-arbol")]
        // public IActionResult CalcularArbolDeLaVida([FromBody] ArbolVidaRequest request)
        // {
        //     if (request == null || request.Persona == null || request.Datos == null)
        //     {
        //         return BadRequest("Datos insuficientes para el cálculo.");
        //     }

        //     // Extraemos los datos del request
        //     var persona = request.Persona;
        //     var dn = request.Datos;

        //     var arbol = new ArbolVida
        //     {
        //         PersonaId = persona.Id,
        //         FechaCalculo = DateTime.Now,

        //         // --- SEFIROT ---
        //         Kether_Valor = dn.MisionVida,
        //         Kether_Nombre = "Misión de Vida",

        //         Chokmah_Valor = ReducirNumero(persona.FechaNacimiento.Year),
        //         Chokmah_Nombre = "Dones Pasados",

        //         Binah_Valor = ReducirNumero(persona.FechaNacimiento.Month),
        //         Binah_Nombre = "Karma / Tikún",

        //         Chesed_Valor = ReducirNumero(persona.FechaNacimiento.Day + persona.FechaNacimiento.Month),
        //         Chesed_Nombre = "Abundancia",

        //         Gevurah_Valor = ReducirNumero(Math.Abs(persona.FechaNacimiento.Day - persona.FechaNacimiento.Month)),
        //         Gevurah_Nombre = "Desafíos",

        //         Tiferet_Valor = dn.NumeroAlma,
        //         Tiferet_Nombre = "Esencia",

        //         Netzach_Valor = ReducirNumero(persona.FechaNacimiento.Day),
        //         Netzach_Nombre = "Victoria",

        //         Hod_Valor = dn.NumeroPersonalidad,
        //         Hod_Nombre = "Mente",

        //         Yesod_Valor = ReducirNumero(dn.NumeroAlma + dn.NumeroPersonalidad),
        //         Yesod_Nombre = "Raíz",

        //         Malchut_Valor = dn.RegaloDivino,
        //         Malchut_Nombre = "Realización",

        //         // --- SENDEROS COMPLETOS ---
        //         Sendero_1_2 = CalcularArcanoSendero(dn.MisionVida, ReducirNumero(persona.FechaNacimiento.Year)),
        //         Sendero_1_3 = CalcularArcanoSendero(dn.MisionVida, ReducirNumero(persona.FechaNacimiento.Month)),
        //         Sendero_1_6 = CalcularArcanoSendero(dn.MisionVida, dn.NumeroAlma),
        //         Sendero_2_3 = CalcularArcanoSendero(ReducirNumero(persona.FechaNacimiento.Year), ReducirNumero(persona.FechaNacimiento.Month)),
        //         Sendero_2_4 = CalcularArcanoSendero(ReducirNumero(persona.FechaNacimiento.Year), ReducirNumero(persona.FechaNacimiento.Day + persona.FechaNacimiento.Month)),
        //         Sendero_2_6 = CalcularArcanoSendero(ReducirNumero(persona.FechaNacimiento.Year), dn.NumeroAlma),
        //         Sendero_3_5 = CalcularArcanoSendero(ReducirNumero(persona.FechaNacimiento.Month), ReducirNumero(Math.Abs(persona.FechaNacimiento.Day - persona.FechaNacimiento.Month))),
        //         Sendero_3_6 = CalcularArcanoSendero(ReducirNumero(persona.FechaNacimiento.Month), dn.NumeroAlma),
        //         Sendero_4_5 = CalcularArcanoSendero(ReducirNumero(persona.FechaNacimiento.Day + persona.FechaNacimiento.Month), ReducirNumero(Math.Abs(persona.FechaNacimiento.Day - persona.FechaNacimiento.Month))),
        //         Sendero_4_6 = CalcularArcanoSendero(ReducirNumero(persona.FechaNacimiento.Day + persona.FechaNacimiento.Month), dn.NumeroAlma),
        //         Sendero_4_7 = CalcularArcanoSendero(ReducirNumero(persona.FechaNacimiento.Day + persona.FechaNacimiento.Month), ReducirNumero(persona.FechaNacimiento.Day)),
        //         Sendero_5_6 = CalcularArcanoSendero(ReducirNumero(Math.Abs(persona.FechaNacimiento.Day - persona.FechaNacimiento.Month)), dn.NumeroAlma),
        //         Sendero_5_8 = CalcularArcanoSendero(ReducirNumero(Math.Abs(persona.FechaNacimiento.Day - persona.FechaNacimiento.Month)), dn.NumeroPersonalidad),
        //         Sendero_6_7 = CalcularArcanoSendero(dn.NumeroAlma, ReducirNumero(persona.FechaNacimiento.Day)),
        //         Sendero_6_8 = CalcularArcanoSendero(dn.NumeroAlma, dn.NumeroPersonalidad),
        //         Sendero_6_9 = CalcularArcanoSendero(dn.NumeroAlma, ReducirNumero(dn.NumeroAlma + dn.NumeroPersonalidad)),
        //         Sendero_7_8 = CalcularArcanoSendero(ReducirNumero(persona.FechaNacimiento.Day), dn.NumeroPersonalidad),
        //         Sendero_7_9 = CalcularArcanoSendero(ReducirNumero(persona.FechaNacimiento.Day), ReducirNumero(dn.NumeroAlma + dn.NumeroPersonalidad)),
        //         Sendero_7_10 = CalcularArcanoSendero(ReducirNumero(persona.FechaNacimiento.Day), dn.RegaloDivino),
        //         Sendero_8_9 = CalcularArcanoSendero(dn.NumeroPersonalidad, ReducirNumero(dn.NumeroAlma + dn.NumeroPersonalidad)),
        //         Sendero_8_10 = CalcularArcanoSendero(dn.NumeroPersonalidad, dn.RegaloDivino),
        //         Sendero_9_10 = CalcularArcanoSendero(ReducirNumero(dn.NumeroAlma + dn.NumeroPersonalidad), dn.RegaloDivino)
        //     };

        //     return Ok(arbol);
        // }

        // private ArbolVida CalcularArbolDeLaVida(Persona persona, DatosNumerologiaDto dn)
        // {
        //     var arbol = new ArbolVida
        //     {
        //         PersonaId = persona.Id,
        //         FechaCalculo = DateTime.Now,

        //         // --- ASIGNACIÓN DE SEFIROT ---
        //         Kether_Valor = dn.MisionVida,
        //         Kether_Nombre = "Misión de Vida",

        //         Chokmah_Valor = ReducirNumero(dn.AnioNacimiento),
        //         Chokmah_Nombre = "Vidas Pasadas",

        //         Binah_Valor = ReducirNumero(dn.MesNacimiento),
        //         Binah_Nombre = "Karma / Tikún",

        //         Chesed_Valor = ReducirNumero(dn.DiaNacimiento + dn.MesNacimiento),
        //         Chesed_Nombre = "Abundancia",

        //         Gevurah_Valor = ReducirNumero(Math.Abs(dn.DiaNacimiento - dn.MesNacimiento)),
        //         Gevurah_Nombre = "Desafíos",

        //         Tiferet_Valor = dn.NumeroAlma,
        //         Tiferet_Nombre = "Esencia / Corazón",

        //         Netzach_Valor = ReducirNumero(dn.DiaNacimiento),
        //         Netzach_Nombre = "Fortaleza",

        //         Hod_Valor = dn.NumeroPersonalidad,
        //         Hod_Nombre = "Mente / Imagen",

        //         Yesod_Valor = ReducirNumero(dn.NumeroAlma + dn.NumeroPersonalidad),
        //         Yesod_Nombre = "Raíz Subconsciente",

        //         Malchut_Valor = dn.RegaloDivino,
        //         Malchut_Nombre = "Realización Física",

        //         // --- LÓGICA DE LOS 22 SENDEROS COMPLETOS ---
        //         // Se calcula sumando las Sefirot que conectan y ajustando al rango de Arcanos (0-21)
        //         Sendero_1_2 = CalcularArcanoSendero(dn.MisionVida, ReducirNumero(dn.AnioNacimiento)), // Kether-Chokmah
        //         Sendero_1_3 = CalcularArcanoSendero(dn.MisionVida, ReducirNumero(dn.MesNacimiento)),  // Kether-Binah
        //         Sendero_1_6 = CalcularArcanoSendero(dn.MisionVida, dn.NumeroAlma),                    // Kether-Tiferet
        //         Sendero_2_3 = CalcularArcanoSendero(ReducirNumero(dn.AnioNacimiento), ReducirNumero(dn.MesNacimiento)),
        //         Sendero_2_4 = CalcularArcanoSendero(ReducirNumero(dn.AnioNacimiento), ReducirNumero(dn.DiaNacimiento + dn.MesNacimiento)),
        //         Sendero_2_6 = CalcularArcanoSendero(ReducirNumero(dn.AnioNacimiento), dn.NumeroAlma),
        //         Sendero_3_5 = CalcularArcanoSendero(ReducirNumero(dn.MesNacimiento), ReducirNumero(Math.Abs(dn.DiaNacimiento - dn.MesNacimiento))),
        //         Sendero_3_6 = CalcularArcanoSendero(ReducirNumero(dn.MesNacimiento), dn.NumeroAlma),
        //         Sendero_4_5 = CalcularArcanoSendero(ReducirNumero(dn.DiaNacimiento + dn.MesNacimiento), ReducirNumero(Math.Abs(dn.DiaNacimiento - dn.MesNacimiento))),
        //         Sendero_4_6 = CalcularArcanoSendero(ReducirNumero(dn.DiaNacimiento + dn.MesNacimiento), dn.NumeroAlma),
        //         Sendero_4_7 = CalcularArcanoSendero(ReducirNumero(dn.DiaNacimiento + dn.MesNacimiento), ReducirNumero(dn.DiaNacimiento)),
        //         Sendero_5_6 = CalcularArcanoSendero(ReducirNumero(Math.Abs(dn.DiaNacimiento - dn.MesNacimiento)), dn.NumeroAlma),
        //         Sendero_5_8 = CalcularArcanoSendero(ReducirNumero(Math.Abs(dn.DiaNacimiento - dn.MesNacimiento)), dn.NumeroPersonalidad),
        //         Sendero_6_7 = CalcularArcanoSendero(dn.NumeroAlma, ReducirNumero(dn.DiaNacimiento)),
        //         Sendero_6_8 = CalcularArcanoSendero(dn.NumeroAlma, dn.NumeroPersonalidad),
        //         Sendero_6_9 = CalcularArcanoSendero(dn.NumeroAlma, ReducirNumero(dn.NumeroAlma + dn.NumeroPersonalidad)),
        //         Sendero_7_8 = CalcularArcanoSendero(ReducirNumero(dn.DiaNacimiento), dn.NumeroPersonalidad),
        //         Sendero_7_9 = CalcularArcanoSendero(ReducirNumero(dn.DiaNacimiento), ReducirNumero(dn.NumeroAlma + dn.NumeroPersonalidad)),
        //         Sendero_7_10 = CalcularArcanoSendero(ReducirNumero(dn.DiaNacimiento), dn.RegaloDivino),
        //         Sendero_8_9 = CalcularArcanoSendero(dn.NumeroPersonalidad, ReducirNumero(dn.NumeroAlma + dn.NumeroPersonalidad)),
        //         Sendero_8_10 = CalcularArcanoSendero(dn.NumeroPersonalidad, dn.RegaloDivino),
        //         Sendero_9_10 = CalcularArcanoSendero(ReducirNumero(dn.NumeroAlma + dn.NumeroPersonalidad), dn.RegaloDivino)
        //     };

        //     return arbol;
        // }

        // Función para determinar el Arcano del Sendero (Lógica Cabalística)
        private int CalcularArcanoSendero(int val1, int val2)
        {
            // Sumamos las energías de las dos Sefirot que se conectan
            int suma = val1 + val2;

            // Lógica de correspondencia de Arcanos Mayores (0-21)
            if (suma == 22) return 0; // El Loco
            if (suma > 22) {
                // Reducción mística: sumamos dígitos hasta que sea <= 21
                while (suma > 21) {
                    int s = 0;
                    foreach (var c in suma.ToString()) s += int.Parse(c.ToString());
                    suma = s;
                }
            }
            return suma;
        }

        private int ReducirNumero(int n)
        {
            if (n == 11 || n == 22 || n == 33 || n == 44) return n;
            while (n > 9)
            {
                int sum = 0;
                foreach (var c in n.ToString()) { sum += int.Parse(c.ToString()); }
                n = sum;
            }
            return n;
        }

        // [HttpGet("obtener-arbol/{personaId}")]
        // public async Task<IActionResult> GetArbol(int personaId)
        // {
        //     var arbol = await _context.ArbolesVida
        //         .FirstOrDefaultAsync(a => a.PersonaId == personaId);

        //     if (arbol == null) return NotFound();

        //     // Aquí está el truco: Devolvemos el objeto arbol tal cual.
        //     // El frontend usará los valores de Sendero_X_Y para buscar en su propia
        //     // lista de Arcanos (que cargaremos al iniciar la app).
        //     return Ok(arbol);
        // }

        [HttpGet("obtener-arbol/{personaId}")]
        public async Task<IActionResult> ObtenerArbol(int personaId)
        {
            var arbol = await _context.ArbolesVida
                .FirstOrDefaultAsync(a => a.PersonaId == personaId);

            if (arbol == null)
            {
                // Si no existe, podrías disparar el cálculo aquí mismo
                return NotFound("El árbol aún no ha sido calculado para esta persona.");
            }

            return Ok(arbol);
        }

        [HttpGet("ObtenerDetalle")]
        public async Task<IActionResult> ObtenerDetalle(string tipo, int id, string nombreSefira = "")
        {
            try
            {
                if (tipo == "sendero")
                {
                    // Consulta a la tabla de Arcanos para las líneas
                    var arcano = await _context.Arcanos
                        .FirstOrDefaultAsync(a => a.Numero == id);
                    
                    if (arcano == null) return NotFound("Información del sendero no encontrada.");
                    
                    return Ok(new {
                        exito = true,
                        headerTitulo = $"SENDERO: ARCANO {id}", // Título identificador
                        tipo = "Sendero (Proceso)",
                        nombre = arcano.Nombre,
                        letraHebrea = arcano.LetraHebrea,
                        mensaje = arcano.Mensaje,
                        significadoEsoterico = arcano.SignificadoEsoterico,
                        imagen = arcano.ImagenUrl, // Asegúrate que el campo sea ImagenUrl o UrlImagen según tu modelo
                        extra = arcano.ElementoOPlaneta
                    });
                }
                else // tipo == "sefira"
                {
                    // Consulta a la nueva tabla de Arquetipos para los círculos
                    var arquetipo = await _context.Arquetipos
                        .FirstOrDefaultAsync(a => a.Numero == id);

                    if (arquetipo == null) return NotFound("Arquetipo no encontrado en la base de datos.");

                    // Construimos un título descriptivo, ej: "SEFIRA: KETHER (Arquetipo 19)"
                    // Si nombreSefira llega vacío (""), usamos un genérico o solo el ID
                    string identificador = string.IsNullOrWhiteSpace(nombreSefira) 
                                        ? $"VIBRACIÓN {id}" 
                                        : nombreSefira.ToUpper();
                    string nombreLimpio = string.IsNullOrEmpty(nombreSefira) ? "" : nombreSefira.ToUpper() + " ";
                    string tituloContexto = $"SEFIRA: {nombreLimpio}(Arquetipo {id})";

                    return Ok(new {
                        exito = true,
                        headerTitulo = tituloContexto,
                        tipo = "Sefirá (Estado)",
                        nombre = arquetipo.Nombre, 
                        letraHebrea = arquetipo.LetraHebrea,
                        mensaje = arquetipo.Mensaje,
                        significadoEsoterico = arquetipo.SignificadoEsoterico,
                        imagen = arquetipo.ImagenUrl, 
                        extra = arquetipo.ElementoOPlaneta
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al consultar la base de datos", detalle = ex.Message });
            }
        }

        [HttpGet("ObtenerRegaloPorPersona")]
        public async Task<IActionResult> ObtenerRegaloPorPersona(int personaId)
        {
            // Buscamos directamente en la tabla que mencionas
            var arbol = await _context.ArbolesVida
                .Where(a => a.PersonaId == personaId)
                .Select(a => new { a.Kether_Valor }) // Solo traemos lo necesario
                .FirstOrDefaultAsync();

            if (arbol == null) return NotFound("Árbol no encontrado");

            return Ok(new { valor = arbol.Kether_Valor });
        }

    }
}