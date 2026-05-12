using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;
using Guia.API.Data;
using Guia.API.Models;
using System.ComponentModel.DataAnnotations;

namespace Guia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PersonasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // MÉTODO PRIVADO: El "cerebro" que calcula todo lo diario
            private async Task<object> ObtenerPerfilDiario(Persona usuario)
            {
                int añoPers = CalcularAñoPersonal(usuario.FechaNacimiento);
                int añoUniv = ReducirNumeroSimple(DateTime.Now.Year);
                int vibDia = CalcularVibracionDia(usuario.FechaNacimiento);
                string mensajeVib = ObtenerMensajeVibracion(vibDia);
                var gratitudInfo = await ObtenerGratitudAleatoria();

                return new
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombres + " " + usuario.Apellidos,
                    FechaNacimiento = usuario.FechaNacimiento.ToString("yyyy-MM-dd"),
                    DatosAstro = new {
                        Signo = usuario.Detalle?.SignoZodiaco,
                        Luna = usuario.Detalle?.FaseLunar,
                        Elemento = usuario.Detalle?.Elemento,
                        Gratitud = gratitudInfo.Texto,
                        GratitudCategoria = gratitudInfo.Categoria
                    },
                    DatosNumerologia = new {
                        Mision = usuario.Numerologia?.MisionVida,
                        Alma = usuario.Numerologia?.NumeroAlma,
                        Destino = usuario.Numerologia?.NumeroDestino,
                        Personalidad = usuario.Numerologia?.NumeroPersonalidad,
                        AñoPersonal = añoPers,
                        AñoUniversal = añoUniv,
                        VibracionHoy = vibDia,
                        MensajeVibracion = mensajeVib
                    }
                };
            }

            // ENDPOINT DE REFRESCO: Solo para actualizar cuando cambia el día
            [HttpGet("refresh-data/{id}")]
            public async Task<ActionResult> RefreshData(int id)
            {
                var usuario = await _context.Personas
                    .Include(p => p.Detalle)
                    .Include(p => p.Numerologia)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (usuario == null) return NotFound();

                var perfil = await ObtenerPerfilDiario(usuario);
                return Ok(perfil);
            }

            // EL LOGIN: Ahora queda súper corto
            [HttpPost("login")]
            public async Task<ActionResult> Login([FromBody] LoginRequest login)
            {
                // 1. Validar modelo (letras y números)
                if (!ModelState.IsValid) return BadRequest(new { Mensaje = "Formato inválido" });

                // 2. Buscar usuario (usamos AsNoTracking para que sea más rápido y consuma menos RAM)
                var usuario = await _context.Personas
                    .AsNoTracking() 
                    .Include(p => p.Detalle)
                    .Include(p => p.Numerologia)
                    .FirstOrDefaultAsync(p => p.Username == login.Username && p.Password == login.Password);
                // var usuario = await _context.Personas
                //     .Include(p => p.Detalle)
                //     .Include(p => p.Numerologia)
                //     .FirstOrDefaultAsync(p => p.Username == login.Username && p.Password == login.Password);

                if (usuario == null) return Unauthorized(new { Mensaje = "Usuario o contraseña incorrectos" });

                // 3. El cerebro diario
                var perfil = await ObtenerPerfilDiario(usuario);
                return Ok(perfil);
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<Persona>> GetPersona(int id)
            {
                // Buscamos la persona en la base de datos por su ID principal
                var persona = await _context.Personas.FindAsync(id);

                if (persona == null)
                {
                    return NotFound(new { mensaje = "Persona no encontrada" });
                }

                return persona;
            }

        [HttpPost("loginInicial")]
        public async Task<ActionResult> LoginInicial([FromBody] LoginRequest login)
        {
            // 1. Buscamos al usuario por su Username e incluimos sus tablas relacionadas
            var usuario = await _context.Personas
                .Include(p => p.Detalle)
                .Include(p => p.Numerologia)
                .FirstOrDefaultAsync(p => p.Username == login.Username && p.Password == login.Password);

            if (usuario == null)
            {
                return Unauthorized(new { Mensaje = "Usuario o contraseña incorrectos" });
            }

            // 2. Si el usuario existe, devolvemos su "Perfil Espiritual" completo
            // --- CÁLCULOS EN TIEMPO REAL ---
            int añoPersonal = CalcularAñoPersonal(usuario.FechaNacimiento);
            int vibracionDia = CalcularVibracionDia(usuario.FechaNacimiento);
            int añoPers = CalcularAñoPersonal(usuario.FechaNacimiento);
            int añoUniv = ReducirNumeroSimple(DateTime.Now.Year); // 2026 = 1
            int vibDia = CalcularVibracionDia(usuario.FechaNacimiento);
            string mensajeVib = ObtenerMensajeVibracion(vibDia); // <-- Nueva línea
            var gratitudInfo = await ObtenerGratitudAleatoria();
            return Ok(new
            {
                Id = usuario.Id,
                Nombre = usuario.Nombres + " " + usuario.Apellidos,
                Apellido = usuario.Apellidos,
                Nomb= usuario.Nombres,
                Email = usuario.Email,
                FechaNacimiento = usuario.FechaNacimiento.ToString("yyyy-MM-dd"),
                DatosAstro = new {
                    Signo = usuario.Detalle?.SignoZodiaco,
                    Luna = usuario.Detalle?.FaseLunar,
                    Elemento = usuario.Detalle?.Elemento,
                    Gratitud = gratitudInfo.Texto,
                    GratitudCategoria = gratitudInfo.Categoria,
                    Notas = usuario.Detalle?.NotasAstrologicas
                },
                DatosNumerologia = new {
                    Mision = usuario.Numerologia?.MisionVida,
                    Alma = usuario.Numerologia?.NumeroAlma,
                    Destino = usuario.Numerologia?.NumeroDestino,
                    Personalidad = usuario.Numerologia?.NumeroPersonalidad,
                    AñoPersonal = añoPers,
                    AñoUniversal = añoUniv,
                    VibracionHoy = vibDia,
                    MensajeVibracion= mensajeVib
                }
            });
        }

// Clase de apoyo para recibir los datos del login
        public class LoginRequest
        {
            [Required]
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Solo letras y números")]
            public string Username { get; set; } = string.Empty;
            [Required]
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Solo letras y números")]
            public string Password { get; set; } = string.Empty;
        }

        [HttpPost("registrarPersonaAntes")]
        public async Task<ActionResult> RegistrarPersonaL([FromBody] JsonElement body)
        {
            try 
            {
                var personaInput = new Persona
                {
                    Nombres = LeerCampo(body, "nombres", "nombre"),
                    Apellidos = LeerCampo(body, "apellidos", "apellido"),
                    Email = LeerCampo(body, "email", "correo"),
                    Username = LeerCampo(body, "username", "usuario"),
                    Password = LeerCampo(body, "password", "contrasena")
                };

                var fechaRaw = LeerCampo(body, "fechaNacimiento", "fechaNac");
                if (TryParseFechaFlexible(fechaRaw, out var fechaNacimiento))
                {
                    personaInput.FechaNacimiento = fechaNacimiento;
                }

                personaInput.Nombres = (personaInput.Nombres ?? string.Empty).Trim();
                personaInput.Apellidos = (personaInput.Apellidos ?? string.Empty).Trim();
                personaInput.Email = (personaInput.Email ?? string.Empty).Trim();
                personaInput.Username = (personaInput.Username ?? string.Empty).Trim();
                personaInput.Password = (personaInput.Password ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(personaInput.Nombres))
                    return BadRequest("El nombre es obligatorio.");
                if (string.IsNullOrWhiteSpace(personaInput.Apellidos))
                    return BadRequest("El apellido es obligatorio.");
                if (string.IsNullOrWhiteSpace(personaInput.Email))
                    return BadRequest("El correo electrónico es obligatorio.");
                if (string.IsNullOrWhiteSpace(personaInput.Username))
                    return BadRequest("El nombre de usuario es obligatorio.");
                if (string.IsNullOrWhiteSpace(personaInput.Password))
                    return BadRequest("La contraseña es obligatoria.");
                if (personaInput.FechaNacimiento == default)
                    return BadRequest("La fecha de nacimiento no es válida.");

                // Validar si el usuario o email ya existen (Prevención de errores)
                if (await _context.Personas.AnyAsync(p => p.Username == personaInput.Username))
                    return BadRequest("El nombre de usuario ya está en uso.");
                
                if (await _context.Personas.AnyAsync(p => p.Email == personaInput.Email))
                    return BadRequest("El correo electrónico ya está registrado.");

                // --- CÁLCULOS ---
                string nombreCompleto = (personaInput.Nombres + personaInput.Apellidos).ToUpper();
                
                int numAlma = CalcularNumerologiaNombre(nombreCompleto, soloVocales: true);
                int numPersonalidad = CalcularNumerologiaNombre(nombreCompleto, soloConsonantes: true);
                int numDestino = CalcularNumerologiaNombre(nombreCompleto); // Todas las letras
                int misionVida = CalcularMisionVida(personaInput.FechaNacimiento);
                string signo = CalcularZodiaco(personaInput.FechaNacimiento);

                

                // --- CREACIÓN DE REGISTRO ---
                var nuevaPersona = new Persona
                {
                    Nombres = personaInput.Nombres,
                    Apellidos = personaInput.Apellidos,
                    Email = personaInput.Email,
                    Username = personaInput.Username,
                    Password = personaInput.Password, // Nota: En el futuro usaremos Hash
                    FechaNacimiento = personaInput.FechaNacimiento,
                    FechaRegistro = DateTime.Now
                };

                nuevaPersona.Detalle = new PersonaDetalle
                {
                    SignoZodiaco = signo,
                    FaseLunar = CalcularFaseLunar(personaInput.FechaNacimiento), // ¡Ahora sí tiene valor!
                    Elemento = ObtenerElemento(signo),
                    NotasAstrologicas = "Tu camino inicia aquí...", // El campo que no olvidamos
                    MensajeGratitud = "Hoy agradezco por la oportunidad de un nuevo despertar." // Dinámico luego
                };

                nuevaPersona.Numerologia = new PersonaNumerologia
                {
                    MisionVida = misionVida,
                    NumeroAlma = numAlma,
                    NumeroPersonalidad = numPersonalidad,
                    NumeroDestino = numDestino,
                    LeccionVida = misionVida // A veces coinciden, se puede ajustar la lógica

                };

                // --- CÁLCULO DEL ÁRBOL DE LA VIDA (VERDAD MÍSTICA - BASE 22) ---
                int dia = personaInput.FechaNacimiento.Day;
                int mes = personaInput.FechaNacimiento.Month;
                int anio = personaInput.FechaNacimiento.Year;
                int sumaTotalFecha = dia + mes + anio; // 13 + 1 + 1985 = 1999

                var elArbol = new ArbolVida
                {
                    // --- LAS 10 SEFIROT ---
                    Kether_Valor = ReducirBase22(sumaTotalFecha),          // Resultado: 19 (El Sol)
                    Chokmah_Valor = ReducirBase22(SumarDigitos(anio)),     // Resultado: 1 (El Mago)
                    Binah_Valor = ReducirBase22(anio),                     // Resultado: 5 (El Hierofante)
                    Chesed_Valor = ReducirBase22(dia),                      // Resultado: 13 (La Muerte) - Pasado
                    Gevurah_Valor = ReducirBase22(mes),                     // Resultado: 1 (El Mago) - Karma
                    Tiferet_Valor = ReducirBase22(dia + mes),               // Resultado: 14 (La Templanza) - El Ser
                    Netzach_Valor = ReducirBase22(dia + anio),              // Resultado: 18 (La Luna)
                    Hod_Valor = ReducirBase22(mes + anio),                  // Resultado: 6 (Los Enamorados)
                    Yesod_Valor = ReducirBase22((dia + mes) + sumaTotalFecha), // Resultado: 11 (La Fuerza)
                    Malchut_Valor = ReducirBase22(sumaTotalFecha),          // Resultado: 19 (El Sol)

                    // --- LOS 22 SENDEROS (CONEXIONES ENTRE LOS VALORES ANTERIORES) ---
                    Sendero_1_2 = ReducirBase22(ReducirBase22(sumaTotalFecha) + ReducirBase22(SumarDigitos(anio))),
                    Sendero_1_3 = ReducirBase22(ReducirBase22(sumaTotalFecha) + ReducirBase22(anio)),
                    Sendero_1_6 = ReducirBase22(ReducirBase22(sumaTotalFecha) + ReducirBase22(dia + mes)),
                    Sendero_2_3 = ReducirBase22(ReducirBase22(SumarDigitos(anio)) + ReducirBase22(anio)),
                    Sendero_2_4 = ReducirBase22(ReducirBase22(SumarDigitos(anio)) + ReducirBase22(dia)),
                    Sendero_2_6 = ReducirBase22(ReducirBase22(SumarDigitos(anio)) + ReducirBase22(dia + mes)),
                    Sendero_3_5 = ReducirBase22(ReducirBase22(anio) + ReducirBase22(mes)),
                    Sendero_3_6 = ReducirBase22(ReducirBase22(anio) + ReducirBase22(dia + mes)),
                    Sendero_4_5 = ReducirBase22(ReducirBase22(dia) + ReducirBase22(mes)),
                    Sendero_4_6 = ReducirBase22(ReducirBase22(dia) + ReducirBase22(dia + mes)),
                    Sendero_4_7 = ReducirBase22(ReducirBase22(dia) + ReducirBase22(dia + anio)),
                    Sendero_5_6 = ReducirBase22(ReducirBase22(mes) + ReducirBase22(dia + mes)),
                    Sendero_5_8 = ReducirBase22(ReducirBase22(mes) + ReducirBase22(mes + anio)),
                    Sendero_6_7 = ReducirBase22(ReducirBase22(dia + mes) + ReducirBase22(dia + anio)),
                    Sendero_6_8 = ReducirBase22(ReducirBase22(dia + mes) + ReducirBase22(mes + anio)),
                    Sendero_6_9 = ReducirBase22(ReducirBase22(dia + mes) + ReducirBase22((dia + mes) + sumaTotalFecha)),
                    Sendero_7_8 = ReducirBase22(ReducirBase22(dia + anio) + ReducirBase22(mes + anio)),
                    Sendero_7_9 = ReducirBase22(ReducirBase22(dia + anio) + ReducirBase22((dia + mes) + sumaTotalFecha)),
                    Sendero_7_10 = ReducirBase22(ReducirBase22(dia + anio) + ReducirBase22(sumaTotalFecha)),
                    Sendero_8_9 = ReducirBase22(ReducirBase22(mes + anio) + ReducirBase22((dia + mes) + sumaTotalFecha)),
                    Sendero_8_10 = ReducirBase22(ReducirBase22(mes + anio) + ReducirBase22(sumaTotalFecha)),
                    Sendero_9_10 = ReducirBase22(ReducirBase22((dia + mes) + sumaTotalFecha) + ReducirBase22(sumaTotalFecha)),
                    
                    FechaCalculo = DateTime.Now
                };

                // VINCULACIÓN
                nuevaPersona.ArbolVida = elArbol;
                _context.Personas.Add(nuevaPersona);
                await _context.SaveChangesAsync();

                return Ok(new { Mensaje = "Registro completo", Usuario = nuevaPersona.Username });
            }
            // catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx &&
            //                                   (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            // {
            //     return BadRequest("El usuario o correo ya existe.");
            // }
            // Por esto (Versión para MySQL/MariaDB):
            catch (DbUpdateException ex) when (ex.InnerException is MySqlConnector.MySqlException mySqlEx && (mySqlEx.Number == 1062))
            {
                return BadRequest("El usuario o correo ya existe.");
            }
            catch (DbUpdateException ex)
            {
                var detalle = ex.InnerException?.Message ?? ex.Message;
                return BadRequest($"Error en base de datos: {detalle}");
            }
            catch (Exception ex)
            {
                var detalle = ex.InnerException?.Message ?? ex.Message;
                return BadRequest($"Error en el sistema: {detalle}");
            }
        }

        [HttpPost("registrar")]
        public async Task<ActionResult> RegistrarPersona([FromBody] JsonElement body)
        {
                try 
                {
                    // 1. Lectura inicial de campos
                    var personaInput = new Persona
                    {
                        Nombres = LeerCampo(body, "nombres", "nombre"),
                        Apellidos = LeerCampo(body, "apellidos", "apellido"),
                        Email = LeerCampo(body, "email", "correo"),
                        Username = LeerCampo(body, "username", "usuario"),
                        Password = LeerCampo(body, "password", "contrasena")
                    };

                    // 2. Procesamiento de fecha
                    var fechaRaw = LeerCampo(body, "fechaNacimiento", "fechaNac");
                    if (TryParseFechaFlexible(fechaRaw, out var fechaNacimiento))
                    {
                        personaInput.FechaNacimiento = fechaNacimiento;
                    }

                    // 3. Limpieza y validación de campos vacíos
                    personaInput.Nombres = (personaInput.Nombres ?? string.Empty).Trim();
                    personaInput.Apellidos = (personaInput.Apellidos ?? string.Empty).Trim();
                    personaInput.Email = (personaInput.Email ?? string.Empty).Trim();
                    personaInput.Username = (personaInput.Username ?? string.Empty).Trim();
                    personaInput.Password = (personaInput.Password ?? string.Empty).Trim();

                    if (string.IsNullOrWhiteSpace(personaInput.Nombres) || 
                        string.IsNullOrWhiteSpace(personaInput.Apellidos) ||
                        string.IsNullOrWhiteSpace(personaInput.Email) ||
                        string.IsNullOrWhiteSpace(personaInput.Username) || 
                        string.IsNullOrWhiteSpace(personaInput.Password))
                    {
                        return BadRequest("Todos los campos son obligatorios.");
                    }

                    if (personaInput.FechaNacimiento == default)
                        return BadRequest("La fecha de nacimiento no es válida.");

                    // 4. VALIDACIÓN ALFANUMÉRICA (Solo letras y números)
                    // Usamos Regex para asegurar que no haya símbolos ni espacios en credenciales
                    var regexAlfanumerico = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9]+$");

                    if (!regexAlfanumerico.IsMatch(personaInput.Username))
                        return BadRequest("El nombre de usuario solo debe contener letras y números.");

                    if (!regexAlfanumerico.IsMatch(personaInput.Password))
                        return BadRequest("La contraseña solo debe contener letras y números.");

                    // 5. Verificación de existencia (Evita duplicados en DigitalOcean)
                    if (await _context.Personas.AnyAsync(p => p.Username == personaInput.Username))
                        return BadRequest("El nombre de usuario ya está en uso.");
                    
                    // if (await _context.Personas.AnyAsync(p => p.Email == personaInput.Email))
                    //     return BadRequest("El correo electrónico ya está registrado.");

                    // 6. CÁLCULOS NUMEROLÓGICOS Y ASTROLÓGICOS
                    string nombreCompleto = (personaInput.Nombres + personaInput.Apellidos).ToUpper();
                    
                    int numAlma = CalcularNumerologiaNombre(nombreCompleto, soloVocales: true);
                    int numPersonalidad = CalcularNumerologiaNombre(nombreCompleto, soloConsonantes: true);
                    int numDestino = CalcularNumerologiaNombre(nombreCompleto); 
                    int misionVida = CalcularMisionVida(personaInput.FechaNacimiento);
                    string signo = CalcularZodiaco(personaInput.FechaNacimiento);

                    // 7. CREACIÓN DEL OBJETO PERSONA
                    var nuevaPersona = new Persona
                    {
                        Nombres = personaInput.Nombres,
                        Apellidos = personaInput.Apellidos,
                        Email = personaInput.Email,
                        Username = personaInput.Username,
                        Password = personaInput.Password, // Nota: Se recomienda Hash en producción
                        FechaNacimiento = personaInput.FechaNacimiento,
                        FechaRegistro = DateTime.Now
                    };

                    nuevaPersona.Detalle = new PersonaDetalle
                    {
                        SignoZodiaco = signo,
                        FaseLunar = CalcularFaseLunar(personaInput.FechaNacimiento),
                        Elemento = ObtenerElemento(signo),
                        NotasAstrologicas = "Tu camino inicia aquí...",
                        MensajeGratitud = "Hoy agradezco por la oportunidad de un nuevo despertar."
                    };

                    nuevaPersona.Numerologia = new PersonaNumerologia
                    {
                        MisionVida = misionVida,
                        NumeroAlma = numAlma,
                        NumeroPersonalidad = numPersonalidad,
                        NumeroDestino = numDestino,
                        LeccionVida = misionVida
                    };

                    // 8. CÁLCULO DEL ÁRBOL DE LA VIDA (Base 22)
                    int dia = personaInput.FechaNacimiento.Day;
                    int mes = personaInput.FechaNacimiento.Month;
                    int anio = personaInput.FechaNacimiento.Year;
                    int sumaTotalFecha = dia + mes + anio;

                    nuevaPersona.ArbolVida = new ArbolVida
                    {
                        Kether_Valor = ReducirBase22(sumaTotalFecha),
                        Chokmah_Valor = ReducirBase22(SumarDigitos(anio)),
                        Binah_Valor = ReducirBase22(anio),
                        Chesed_Valor = ReducirBase22(dia),
                        Gevurah_Valor = ReducirBase22(mes),
                        Tiferet_Valor = ReducirBase22(dia + mes),
                        Netzach_Valor = ReducirBase22(dia + anio),
                        Hod_Valor = ReducirBase22(mes + anio),
                        Yesod_Valor = ReducirBase22((dia + mes) + sumaTotalFecha),
                        Malchut_Valor = ReducirBase22(sumaTotalFecha),

                        // Senderos (Lógica simplificada para el registro rápido)
                        Sendero_1_2 = ReducirBase22(ReducirBase22(sumaTotalFecha) + ReducirBase22(SumarDigitos(anio))),
                        Sendero_1_3 = ReducirBase22(ReducirBase22(sumaTotalFecha) + ReducirBase22(anio)),
                        Sendero_1_6 = ReducirBase22(ReducirBase22(sumaTotalFecha) + ReducirBase22(dia + mes)),
                        Sendero_2_3 = ReducirBase22(ReducirBase22(SumarDigitos(anio)) + ReducirBase22(anio)),
                        Sendero_2_4 = ReducirBase22(ReducirBase22(SumarDigitos(anio)) + ReducirBase22(dia)),
                        Sendero_2_6 = ReducirBase22(ReducirBase22(SumarDigitos(anio)) + ReducirBase22(dia + mes)),
                        Sendero_3_5 = ReducirBase22(ReducirBase22(anio) + ReducirBase22(mes)),
                        Sendero_3_6 = ReducirBase22(ReducirBase22(anio) + ReducirBase22(dia + mes)),
                        Sendero_4_5 = ReducirBase22(ReducirBase22(dia) + ReducirBase22(mes)),
                        Sendero_4_6 = ReducirBase22(ReducirBase22(dia) + ReducirBase22(dia + mes)),
                        Sendero_4_7 = ReducirBase22(ReducirBase22(dia) + ReducirBase22(dia + anio)),
                        Sendero_5_6 = ReducirBase22(ReducirBase22(mes) + ReducirBase22(dia + mes)),
                        Sendero_5_8 = ReducirBase22(ReducirBase22(mes) + ReducirBase22(mes + anio)),
                        Sendero_6_7 = ReducirBase22(ReducirBase22(dia + mes) + ReducirBase22(dia + anio)),
                        Sendero_6_8 = ReducirBase22(ReducirBase22(dia + mes) + ReducirBase22(mes + anio)),
                        Sendero_6_9 = ReducirBase22(ReducirBase22(dia + mes) + ReducirBase22((dia + mes) + sumaTotalFecha)),
                        Sendero_7_8 = ReducirBase22(ReducirBase22(dia + anio) + ReducirBase22(mes + anio)),
                        Sendero_7_9 = ReducirBase22(ReducirBase22(dia + anio) + ReducirBase22((dia + mes) + sumaTotalFecha)),
                        Sendero_7_10 = ReducirBase22(ReducirBase22(dia + anio) + ReducirBase22(sumaTotalFecha)),
                        Sendero_8_9 = ReducirBase22(ReducirBase22(mes + anio) + ReducirBase22((dia + mes) + sumaTotalFecha)),
                        Sendero_8_10 = ReducirBase22(ReducirBase22(mes + anio) + ReducirBase22(sumaTotalFecha)),
                        Sendero_9_10 = ReducirBase22(ReducirBase22((dia + mes) + sumaTotalFecha) + ReducirBase22(sumaTotalFecha)),
                        
                        FechaCalculo = DateTime.Now
                    };

                    // 9. Guardar en Base de Datos
                    _context.Personas.Add(nuevaPersona);
                    await _context.SaveChangesAsync();

                    return Ok(new { Mensaje = "Registro completo", Usuario = nuevaPersona.Username });
                }
                catch (Exception ex)
                {
                    var detalle = ex.InnerException?.Message ?? ex.Message;
                    return BadRequest($"Error en el sistema: {detalle}");
                }
        }
        
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> ActualizarPersona(int id, [FromBody] JsonElement body)
        {
            try
            {
                // 1. Buscar la persona existente con todas sus relaciones
                var persona = await _context.Personas
                    .Include(p => p.Detalle)
                    .Include(p => p.Numerologia)
                    .Include(p => p.ArbolVida)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (persona == null) return NotFound("Usuario no encontrado.");

                // 2. Lectura de campos del body (Reutilizando tu lógica de LeerCampo)
                string nuevosNombres = LeerCampo(body, "nombres", "nombre");
                string nuevosApellidos = LeerCampo(body, "apellidos", "apellido");
                string nuevoEmail = LeerCampo(body, "email", "correo");
                var fechaRaw = LeerCampo(body, "fechaNacimiento", "fechaNac");

                // 3. Procesamiento de fecha y limpieza
                if (TryParseFechaFlexible(fechaRaw, out var fechaNacimiento))
                {
                    persona.FechaNacimiento = fechaNacimiento;
                }

                persona.Nombres = (nuevosNombres ?? string.Empty).Trim();
                persona.Apellidos = (nuevosApellidos ?? string.Empty).Trim();
                persona.Email = (nuevoEmail ?? string.Empty).Trim();

                // Validaciones básicas
                if (string.IsNullOrWhiteSpace(persona.Nombres) || string.IsNullOrWhiteSpace(persona.Apellidos))
                    return BadRequest("Nombres y apellidos son obligatorios.");

                if (persona.FechaNacimiento == default)
                    return BadRequest("La fecha de nacimiento no es válida.");

                // 4. RE-EJECUTAR CÁLCULOS NUMEROLÓGICOS Y ASTROLÓGICOS (Igual a tu Registro)
                string nombreCompleto = (persona.Nombres + persona.Apellidos).ToUpper();

                // Actualizar Numerología
                persona.Numerologia.NumeroAlma = CalcularNumerologiaNombre(nombreCompleto, soloVocales: true);
                persona.Numerologia.NumeroPersonalidad = CalcularNumerologiaNombre(nombreCompleto, soloConsonantes: true);
                persona.Numerologia.NumeroDestino = CalcularNumerologiaNombre(nombreCompleto);
                persona.Numerologia.MisionVida = CalcularMisionVida(persona.FechaNacimiento);
                persona.Numerologia.LeccionVida = persona.Numerologia.MisionVida;

                // Actualizar Detalles Astrológicos
                string signo = CalcularZodiaco(persona.FechaNacimiento);
                persona.Detalle.SignoZodiaco = signo;
                persona.Detalle.FaseLunar = CalcularFaseLunar(persona.FechaNacimiento);
                persona.Detalle.Elemento = ObtenerElemento(signo);

                // 5. RE-CÁLCULO DEL ÁRBOL DE LA VIDA (Base 22)
                int dia = persona.FechaNacimiento.Day;
                int mes = persona.FechaNacimiento.Month;
                int anio = persona.FechaNacimiento.Year;
                int sumaTotalFecha = dia + mes + anio;

                var arbol = persona.ArbolVida;
                arbol.Kether_Valor = ReducirBase22(sumaTotalFecha);
                arbol.Chokmah_Valor = ReducirBase22(SumarDigitos(anio));
                arbol.Binah_Valor = ReducirBase22(anio);
                arbol.Chesed_Valor = ReducirBase22(dia);
                arbol.Gevurah_Valor = ReducirBase22(mes);
                arbol.Tiferet_Valor = ReducirBase22(dia + mes);
                arbol.Netzach_Valor = ReducirBase22(dia + anio);
                arbol.Hod_Valor = ReducirBase22(mes + anio);
                arbol.Yesod_Valor = ReducirBase22((dia + mes) + sumaTotalFecha);
                arbol.Malchut_Valor = ReducirBase22(sumaTotalFecha);

                // Senderos (Lógica replicada de tu registro)
                arbol.Sendero_1_2 = ReducirBase22(arbol.Kether_Valor + arbol.Chokmah_Valor);
                arbol.Sendero_1_3 = ReducirBase22(arbol.Kether_Valor + arbol.Binah_Valor);
                arbol.Sendero_1_6 = ReducirBase22(arbol.Kether_Valor + arbol.Tiferet_Valor);
                arbol.Sendero_2_3 = ReducirBase22(arbol.Chokmah_Valor + arbol.Binah_Valor);
                arbol.Sendero_2_4 = ReducirBase22(arbol.Chokmah_Valor + arbol.Chesed_Valor);
                arbol.Sendero_2_6 = ReducirBase22(arbol.Chokmah_Valor + arbol.Tiferet_Valor);
                arbol.Sendero_3_5 = ReducirBase22(arbol.Binah_Valor + arbol.Gevurah_Valor);
                arbol.Sendero_3_6 = ReducirBase22(arbol.Binah_Valor + arbol.Tiferet_Valor);
                arbol.Sendero_4_5 = ReducirBase22(arbol.Chesed_Valor + arbol.Gevurah_Valor);
                arbol.Sendero_4_6 = ReducirBase22(arbol.Chesed_Valor + arbol.Tiferet_Valor);
                arbol.Sendero_4_7 = ReducirBase22(arbol.Chesed_Valor + arbol.Netzach_Valor);
                arbol.Sendero_5_6 = ReducirBase22(arbol.Gevurah_Valor + arbol.Tiferet_Valor);
                arbol.Sendero_5_8 = ReducirBase22(arbol.Gevurah_Valor + arbol.Hod_Valor);
                arbol.Sendero_6_7 = ReducirBase22(arbol.Tiferet_Valor + arbol.Netzach_Valor);
                arbol.Sendero_6_8 = ReducirBase22(arbol.Tiferet_Valor + arbol.Hod_Valor);
                arbol.Sendero_6_9 = ReducirBase22(arbol.Tiferet_Valor + arbol.Yesod_Valor);
                arbol.Sendero_7_8 = ReducirBase22(arbol.Netzach_Valor + arbol.Hod_Valor);
                arbol.Sendero_7_9 = ReducirBase22(arbol.Netzach_Valor + arbol.Yesod_Valor);
                arbol.Sendero_7_10 = ReducirBase22(arbol.Netzach_Valor + arbol.Malchut_Valor);
                arbol.Sendero_8_9 = ReducirBase22(arbol.Hod_Valor + arbol.Yesod_Valor);
                arbol.Sendero_8_10 = ReducirBase22(arbol.Hod_Valor + arbol.Malchut_Valor);
                arbol.Sendero_9_10 = ReducirBase22(arbol.Yesod_Valor + arbol.Malchut_Valor);

                arbol.FechaCalculo = DateTime.Now;

                // 6. Guardar cambios
                await _context.SaveChangesAsync();

                return Ok(new { Mensaje = "Actualización y recálculo completo", Usuario = persona.Username });
            }
            catch (Exception ex)
            {
                var detalle = ex.InnerException?.Message ?? ex.Message;
                return BadRequest($"Error al actualizar: {detalle}");
            }
        }
        private static string LeerCampo(JsonElement body, params string[] nombres)
        {
            if (body.ValueKind != JsonValueKind.Object) return string.Empty;

            foreach (var propiedad in body.EnumerateObject())
            {
                foreach (var nombre in nombres)
                {
                    if (string.Equals(propiedad.Name, nombre, StringComparison.OrdinalIgnoreCase))
                    {
                        return propiedad.Value.ValueKind == JsonValueKind.String
                            ? (propiedad.Value.GetString() ?? string.Empty)
                            : propiedad.Value.ToString();
                    }
                }
            }

            return string.Empty;
        }

        private static bool TryParseFechaFlexible(string valor, out DateTime fecha)
        {
            fecha = default;
            if (string.IsNullOrWhiteSpace(valor)) return false;

            var formatos = new[] { "yyyy-MM-dd", "dd/MM/yyyy", "d/M/yyyy", "MM/dd/yyyy", "M/d/yyyy" };
            if (DateTime.TryParseExact(valor, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
            {
                fecha = parsed.Date;
                return true;
            }

            if (DateTime.TryParse(valor, new CultureInfo("es-EC"), DateTimeStyles.None, out parsed) ||
                DateTime.TryParse(valor, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
            {
                fecha = parsed.Date;
                return true;
            }

            return false;
        }

        // --- LÓGICA PRIVADA DE CÁLCULO ---
        private int CalcularNumerologiaNombre_V1(string texto, bool soloVocales = false, bool soloConsonantes = false)
        {
            string vocales = "AEIOU";
            int suma = 0;

            foreach (char c in texto)
            {
                if (!char.IsLetter(c)) continue;

                bool esVocal = vocales.Contains(c);
                
                if (soloVocales && !esVocal) continue;
                if (soloConsonantes && esVocal) continue;

                suma += ObtenerValorPitagorico(c);
            }

            return ReducirNumero(suma);
        }
        private int CalcularNumerologiaNombre_V2(string texto, bool soloVocales = false, bool soloConsonantes = false)
        {
            texto = texto.ToUpper().Replace(" ", "");
            string vocalesEstandar = "AEIOU";
            int suma = 0;

            for (int i = 0; i < texto.Length; i++)
            {
                char c = texto[i];
                if (!char.IsLetter(c)) continue;

                // --- LÓGICA DE LA 'Y' PITAGÓRICA ---
                bool esVocal;
                if (c == 'Y')
                {
                    // Es vocal si: está al final, o entre dos consonantes
                    bool alFinal = (i == texto.Length - 1);
                    bool entreConsonantes = (i > 0 && i < texto.Length - 1 && 
                                            !vocalesEstandar.Contains(texto[i-1]) && 
                                            !vocalesEstandar.Contains(texto[i+1]));
                    
                    esVocal = alFinal || entreConsonantes;
                }
                else
                {
                    esVocal = vocalesEstandar.Contains(c);
                }

                // --- FILTRADO ---
                if (soloVocales && !esVocal) continue;
                if (soloConsonantes && esVocal) continue;

                suma += ObtenerValorPitagorico(c);
            }

            return ReducirNumero(suma);
        }
        private int CalcularNumerologiaNombre(string texto, bool soloVocales = false, bool soloConsonantes = false)
        {
            // NO quitemos los espacios todavía para no pegar letras de nombres distintos
            texto = texto.ToUpper(); 
            string vocalesEstandar = "AEIOU";
            int suma = 0;

            for (int i = 0; i < texto.Length; i++)
            {
                char c = texto[i];
                if (!char.IsLetter(c)) continue;

                bool esVocal;
                if (c == 'Y')
                {
                    // REGLA MEJORADA PARA LA 'Y':
                    // 1. Al final de una palabra (seguida de espacio o fin de cadena)
                    bool alFinal = (i == texto.Length - 1 || texto[i + 1] == ' ');
                    
                    // 2. Entre dos consonantes (ignorando espacios)
                    // En "Taylor", la Y está entre 'A' y 'L'. Sigue siendo vocal por sonido.
                    // Una regla más segura para nombres comunes: 
                    // Si NO le sigue una vocal, casi siempre suena como vocal (Taylor, Wayne, Dylan)
                    bool sigueConsonante = (i < texto.Length - 1 && !vocalesEstandar.Contains(texto[i+1]) && char.IsLetter(texto[i+1]));

                    esVocal = alFinal || sigueConsonante;
                }
                else
                {
                    esVocal = vocalesEstandar.Contains(c);
                }

                // --- FILTRADO ---
                if (soloVocales && !esVocal) continue;
                if (soloConsonantes && esVocal) continue;

                suma += ObtenerValorPitagorico(c);
            }

            return ReducirNumero(suma);
        }
        private int ObtenerValorPitagorico_V1(char c)
        {
            // Lógica: (Letra - 'A') % 9 + 1
            return (c - 'A') % 9 + 1;
        }
        private int ObtenerValorPitagorico(char c)
        {
            // Mapeo exacto Pitagórico
            return c switch
            {
                'A' or 'J' or 'S' => 1,
                'B' or 'K' or 'T' => 2,
                'C' or 'L' or 'U' => 3,
                'D' or 'M' or 'V' => 4,
                'E' or 'N' or 'W' or 'Ñ' => 5, // Ñ se suma como 5
                'F' or 'O' or 'X' => 6,
                'G' or 'P' or 'Y' => 7,
                'H' or 'Q' or 'Z' => 8,
                'I' or 'R' => 9,
                _ => 0
            };
        }
        private int ReducirNumero(int n)
        {
            while (n > 9 && n != 11 && n != 22 && n != 33 && n != 44)
            {
                n = n.ToString().Select(c => int.Parse(c.ToString())).Sum();
            }
            return n;
        }
        private int CalcularMisionVida(DateTime fecha)
        {
            string numeros = fecha.ToString("ddMMyyyy");
            int suma = numeros.Select(c => int.Parse(c.ToString())).Sum();
            while (suma > 9 && suma != 11 && suma != 22) // Respetamos números maestros
            {
                suma = suma.ToString().Select(c => int.Parse(c.ToString())).Sum();
            }
            return suma;
        }

        private string CalcularZodiaco(DateTime fecha)
        {
            int mes = fecha.Month;
            int dia = fecha.Day;

            return mes switch
            {
                1 => dia <= 19 ? "Capricornio" : "Acuario",
                2 => dia <= 18 ? "Acuario" : "Piscis",
                3 => dia <= 20 ? "Piscis" : "Aries",
                4 => dia <= 19 ? "Aries" : "Tauro",
                5 => dia <= 20 ? "Tauro" : "Géminis",
                6 => dia <= 20 ? "Géminis" : "Cáncer",
                7 => dia <= 22 ? "Cáncer" : "Leo",
                8 => dia <= 22 ? "Leo" : "Virgo",
                9 => dia <= 22 ? "Virgo" : "Libra",
                10 => dia <= 22 ? "Libra" : "Escorpio",
                11 => dia <= 21 ? "Escorpio" : "Sagitario",
                12 => dia <= 21 ? "Sagitario" : "Capricornio",
                _ => "Desconocido"
            };
        }

        private string ObtenerElemento(string signo)
        {
            if (new[] { "Aries", "Leo", "Sagitario" }.Contains(signo)) return "Fuego";
            if (new[] { "Tauro", "Virgo", "Capricornio" }.Contains(signo)) return "Tierra";
            if (new[] { "Géminis", "Libra", "Acuario" }.Contains(signo)) return "Aire";
            return "Agua"; // Cáncer, Escorpio, Piscis
        }
        private string ObtenerMensajeVibracion(int num)
            {
                return num switch
                {
                    1 => "Día de inicios, liderazgo y sembrar nuevas semillas. Planta la semilla de un proyecto importante.",
                    2 => "Día de diplomacia, paciencia y trabajo en equipo. La paciencia es tu mejor aliada hoy.",
                    3 => "Día de autoexpresión, alegría y comunicación creativa. Comparte tu luz con los demás.",
                    4 => "Día de orden, disciplina y construir bases sólidas. Pon orden en tus asuntos prácticos.",
                    5 => "Día de libertad, cambios inesperados y aventura. Fluye con los cambios inesperados.",
                    6 => "Día de responsabilidad, amor familiar y armonía. Enfócate en el servicio y el amor incondicional.",
                    7 => "Día de introspección, estudio y conexión espiritual. Busca el silencio para escuchar tu voz interna.",
                    8 => "Día de poder personal, justicias, abundancia y logros materiales. Excelente para decisiones financieras.",
                    9 => "Día de cierres y perdón. Suelta lo que ya no sirve para evolucionar.",
                    _ => "Fluye con la energía del universo hoy."
                };
            }
        private string CalcularFaseLunar(DateTime fecha)
        {
            // Algoritmo simple de aproximación lunar
            double lp = 2551443; 
            DateTime newMoon = new DateTime(1970, 1, 7, 20, 35, 0);
            double phase = ((fecha - newMoon).TotalSeconds) % lp;
            double days = phase / (24 * 3600);

            if (days < 1.84) return "Luna Nueva";
            if (days < 5.53) return "Luna Creciente";
            if (days < 9.22) return "Cuarto Creciente";
            if (days < 12.91) return "Gibosa Creciente";
            if (days < 16.61) return "Luna Llena";
            if (days < 20.30) return "Gibosa Menguante";
            if (days < 23.99) return "Cuarto Menguante";
            if (days < 27.68) return "Luna Menguante";
            return "Luna Nueva";
        }

        private int CalcularVibracionDia(DateTime fechaNacimiento)
        {
            DateTime hoy = DateTime.Now;
            // Sumamos: Día Nac + Mes Nac + Día Hoy + Mes Hoy + Año Hoy
            string cadena = $"{fechaNacimiento.Day}{fechaNacimiento.Month}{hoy.Day}{hoy.Month}{hoy.Year}";
            
            int suma = cadena.Select(c => int.Parse(c.ToString())).Sum();
            
            // Reducimos a un solo dígito (en vibración diaria no solemos usar maestros)
            while (suma > 9)
            {
                suma = suma.ToString().Select(c => int.Parse(c.ToString())).Sum();
            }
            return suma;
        }

        private int CalcularAñoPersonal(DateTime fechaNac)
        {
            DateTime hoy = DateTime.Now;
            
            // Determinamos el "Año del último cumpleaños"
            // Si hoy es Marzo y cumplo en Octubre, mi último cumpleaños fue en 2025.
            // Si hoy es Marzo y cumplí en Enero, mi último cumpleaños fue en 2026.
            int añoUltimoCumpleaños;
            
            DateTime cumpleEsteAño = new DateTime(hoy.Year, fechaNac.Month, fechaNac.Day);
            
            if (hoy >= cumpleEsteAño) 
            {
                añoUltimoCumpleaños = hoy.Year; // Ya cumplió años este 2026
            }
            else 
            {
                añoUltimoCumpleaños = hoy.Year - 1; // Aún no cumple en 2026, usamos 2025
            }

            // Sumamos Día + Mes + Año del último cumpleaños
            int suma = ReducirNumeroSimple(fechaNac.Day + fechaNac.Month + añoUltimoCumpleaños);
            return suma;
        }
        private int ReducirNumeroSimple(int n)
        {
            while (n > 9)
            {
                n = n.ToString().Select(c => int.Parse(c.ToString())).Sum();
            }
            return n;
        }

        private async Task<string> ObtenerGratitudAleatoriaSimple()
        {
            // Buscamos todas las frases y tomamos una al azar
            var frases = await _context.Set<FraseGratitud>().ToListAsync();
            if (frases.Count == 0 || frases == null || !frases.Any()) 
                return ("Hoy agradezco por un nuevo amanecer.");
            
            var random = new Random();
            int indice = random.Next(frases.Count);
            return frases[indice].Texto;
        }

        private async Task<(string Texto, string Categoria)> ObtenerGratitudAleatoria()
        {
            var frases = await _context.FrasesGratitud.ToListAsync();
            
            if (frases == null || !frases.Any()) 
                return ("Hoy agradezco por la oportunidad de evolucionar.", "PROPOSITO");
            
            var random = new Random();
            var fraseElegida = frases[random.Next(frases.Count)];
            
            return (fraseElegida.Texto, fraseElegida.Categoria);
        }



        [HttpGet("significado/{nombreTema}/{numero}")]
        public async Task<ActionResult> GetSignificado(string nombreTema, int numero)
        {
            // 1. Buscamos el Tema por su nombre (ej: "Lección de Vida")
            var tema = await _context.Temas.FirstOrDefaultAsync(t => t.Titulo.ToLower() == nombreTema.ToLower());
            
            if (tema == null) return NotFound(new { Mensaje = "Tema no encontrado" });

            // 2. Buscamos el Significado que combine ese TemaId con el Número del usuario
            var resultado = await _context.Significados
                .FirstOrDefaultAsync(s => s.TemaId == tema.Id && s.ValorNumero == numero);

            if (resultado == null)
            {
                return NotFound(new { Mensaje = "Sabiduría en proceso de edición..." });
            }

            // 3. Devolvemos TODAS tus columnas para que el Modal las muestre
            return Ok(new { 
                apodo = resultado.Apodo,
                mision = resultado.Mision,
                reto = resultado.Reto,
                mantra = resultado.Mantra,
                amuleto = resultado.Amuleto,
                mensajeMagico = resultado.MensajeMagico
            });
        }


        public async Task<string> RecalcularNumerologiaExistente()
        {
            var personas = await _context.Personas
                .Include(p => p.Numerologia)
                .ToListAsync();

            foreach (var p in personas)
            {
                if (p.Numerologia == null) continue;
                
                string nombreCompleto = (p.Nombres + p.Apellidos).ToUpper();
                
                // Aplicamos la nueva lógica que ya incluye la 'Y' inteligente
                p.Numerologia.NumeroAlma = CalcularNumerologiaNombre(nombreCompleto, soloVocales: true);
                p.Numerologia.NumeroPersonalidad = CalcularNumerologiaNombre(nombreCompleto, soloConsonantes: true);
                p.Numerologia.NumeroDestino = CalcularNumerologiaNombre(nombreCompleto);
            }

            await _context.SaveChangesAsync();
            return "Base de datos actualizada con éxito.";
        }
        [HttpGet("recalcular-todo")]
        public async Task<ActionResult> RecalcularTodo()
        {
            try 
            {
                var personas = await _context.Personas
                    .Include(p => p.Numerologia)
                    .ToListAsync();

                foreach (var p in personas)
                {
                    if (p.Numerologia == null) continue;
                    // Usamos tu nueva lógica de la "Y" y la "Ñ"
                    string nombreCompleto = (p.Nombres + " " + p.Apellidos).ToUpper();
                    
                    p.Numerologia.NumeroAlma = CalcularNumerologiaNombre(nombreCompleto, soloVocales: true);
                    p.Numerologia.NumeroPersonalidad = CalcularNumerologiaNombre(nombreCompleto, soloConsonantes: true);
                    p.Numerologia.NumeroDestino = CalcularNumerologiaNombre(nombreCompleto);
                }

                await _context.SaveChangesAsync();
                return Ok(new { mensaje = $"Éxito: Se han actualizado {personas.Count} registros." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        private int ReducirKabalistico(int numero)
        {
            if (numero == 0) return 22; // El Loco (a veces representado como 0 o 22)
            // Usamos el residuo de 22 para que siempre nos dé un Arcano Mayor (0-21)
            int resultado = numero % 22;
            return resultado == 0 ? 22 : resultado;
            // if (numero < 0) numero = Math.Abs(numero);
            
            // // Si el número es 22, en muchas tradiciones se trata como 0 (El Loco)
            // // o se mantiene como 22. Aquí usaremos el resto de 22 para 
            // // mantener el rango 0-21 de los Arcanos Mayores.
            // return numero % 22;
        }

        private int ReducirBase22(int numero)
        {
            if (numero <= 0) return 22; // El Loco
            int resto = numero % 22;
            return (resto == 0) ? 22 : resto;
        }

        private int SumarDigitos(int n)
        {
            int suma = 0;
            while (n > 0) { suma += n % 10; n /= 10; }
            return suma;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPersona(int id, [FromBody] Persona personaEditada)
        {
            if (id != personaEditada.Id) return BadRequest("El ID no coincide.");

            var personaDb = await _context.Personas.FindAsync(id);
            if (personaDb == null) return NotFound();

            // Actualizamos solo los campos permitidos
            personaDb.Nombres = personaEditada.Nombres;
            personaDb.Apellidos = personaEditada.Apellidos;
            personaDb.FechaNacimiento = personaEditada.FechaNacimiento;
            personaDb.Email = personaEditada.Email;
            // El Username y Password no se tocan aquí por seguridad

            try
            {
                await _context.SaveChangesAsync();
                
                // Aquí es donde sucede la magia:
                // Devolvemos un OK para que el frontend sepa que puede mandar a recalcar
                return Ok(new { mensaje = "Datos actualizados", personaId = personaDb.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al guardar: {ex.Message}");
            }
        }




    // private int ReducirKabalistico_ante(int numero)
    // {
    //     if (numero < 10) return numero;
    //     if (numero == 11 || numero == 22) return numero; // Opcional: Mantener maestros

    //     int suma = 0;
    //     while (numero > 0)
    //     {
    //         suma += numero % 10;
    //         numero /= 10;
    //     }
    //     //return ReducirKabalistico(suma);
    // }


        // [HttpGet("significado/{nombreTema}/{numero}")]
        // public async Task<ActionResult> GetSignificado_ant(string nombreTema, int numero)
        // {
        //     // Buscamos el significado que coincida con el número y el nombre del tema (Misión, Alma, etc.)
        //     var resultado = await _context.Significados
        //         .Include(s => s.Tema)
        //         .FirstOrDefaultAsync(s => s.Tema != null && s.Tema.Titulo.ToLower() == nombreTema.ToLower() 
        //                             && s.ValorNumero == numero);

        //     if (resultado == null)
        //     {
        //         return NotFound(new { Mensaje = "Sabiduría en proceso de transcripción..." });
        //     }

        //     // Devolvemos el objeto completo con tus campos especiales
        //     return Ok(new { 
        //         apodo = resultado.Apodo,
        //         mision = resultado.Mision,
        //         reto = resultado.Reto,
        //         mantra = resultado.Mantra,
        //         amuleto = resultado.Amuleto,
        //         mensaje = resultado.MensajeMagico
        //     });
        // }
    }
}



