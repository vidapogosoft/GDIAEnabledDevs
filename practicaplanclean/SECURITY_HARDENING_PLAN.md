# Plan De Hardening De Seguridad (Objetivo: Nivel Medio)

Fecha: 2026-04-26  
Proyecto: `SistemaGuia.sln` (`Guia.API`)

## Resumen Ejecutivo
El estado actual no alcanza seguridad media para producción. Este plan define cambios mínimos, priorizados y por archivo para cerrar brechas críticas.

## Prioridad 0 (Bloqueantes De Producción)

1. Eliminar credenciales expuestas y rotarlas.
2. Implementar autenticación y autorización.
3. Migrar contraseñas a hash seguro.
4. Restringir CORS.
5. Forzar HTTPS y endurecer transporte.

---

## Cambios Exactos Por Archivo

## 1) `Guia.API/appsettings.json`
- Eliminar `User Id` y `Password` en texto plano.
- Eliminar `TrustServerCertificate=True` para producción.
- Dejar solo placeholders o valores no sensibles.
- Agregar sección de seguridad operativa:
  - `Security:AllowedOrigins` (lista de dominios permitidos).
  - `Security:RequireHttps` (`true` en prod).

Ejemplo esperado (producción):
- Connection string inyectado por variable de entorno:  
  - `ConnectionStrings__DefaultConnection`
- JWT secrets inyectados por variable de entorno:
  - `Jwt__Issuer`
  - `Jwt__Audience`
  - `Jwt__Key`

## 2) `Guia.API/Program.cs`
- Agregar autenticación JWT:
  - `builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)...`
- Agregar autorización:
  - `builder.Services.AddAuthorization();`
- Cambiar CORS abierto por política nombrada y restringida a dominios confiables.
- Activar middlewares en orden correcto:
  - `UseHttpsRedirection()`
  - `UseHsts()` (solo no-development)
  - `UseAuthentication()`
  - `UseAuthorization()`
- Agregar rate limiting global (`AddRateLimiter` + `UseRateLimiter`).
- Mantener Swagger solo en `Development` (ya está, conservarlo).
- Reemplazar inicialización de datos automática en arranque para producción (ver punto `DbInitializer`).

## 3) `Guia.API/Controllers/PersonasController.cs`
- Reemplazar login de contraseña en claro por validación con hash (`IPasswordHasher`).
- Crear DTOs de entrada para evitar overposting:
  - `RegisterRequestDto`
  - `LoginRequestDto` (ya existe base, endurecer validaciones).
- En `registrar`:
  - hashear contraseña antes de persistir.
  - validar longitud mínima y complejidad.
- En `login` y `loginInicial`:
  - emitir JWT con `sub` = `personaId`.
- Proteger endpoints sensibles con `[Authorize]`.
- Eliminar endpoint mutante por `GET`:
  - `recalcular-todo` debe pasar a `POST` y limitar a admin.
- No devolver `ex.Message` al cliente:
  - usar `Problem()` y log interno.

## 4) `Guia.API/Controllers/BitacoraController.cs`
- Agregar `[Authorize]`.
- No aceptar `PersonaId` desde cliente para operaciones del usuario autenticado.
- Obtener `personaId` desde `User` claims.
- Validar ownership en:
  - `mis-registros/{personaId}`
  - `historial/{personaId}`
  - `reto-activo/{personaId}`
  - `finalizar-reto/{id}` (verificar que el registro pertenece al usuario autenticado).
- En `registrar`, reemplazar `Bitacora entrada` por DTO explícito.

## 5) `Guia.API/Controllers/RetosController.cs`
- Agregar `[Authorize]`.
- Eliminar `personaId` del route para endpoints de “mi estado”.
- Derivar usuario desde JWT claims.
- Agregar validación de acceso por recurso.

## 6) `Guia.API/Controllers/NumerologiaController.cs`
- Proteger endpoints que leen datos por `personaId` con `[Authorize]` y ownership check.
- Eliminar mensajes internos en respuestas:
  - reemplazar `StatusCode(500, "... " + ex.Message)` por error genérico.
- Evaluar si `calcular-arbol` debe aceptar `Persona` completa desde cliente:
  - preferible enviar solo datos mínimos y resolver usuario en backend.

## 7) `Guia.API/Models/Persona.cs`
- Mantener `Password` pero migrar semántica a hash:
  - renombrar a `PasswordHash` (migración EF).
- Agregar restricciones:
  - `[Required]`, longitud razonable de hash.

## 8) `Guia.API/Data/ApplicationDbContext.cs`
- Revisar índices únicos y agregar los faltantes por negocio.
- Asegurar configuración explícita para campos de seguridad (`PasswordHash`).

## 9) `Guia.API/Data/Seeders/DbInitializer.cs`
- Evitar `EnsureCreated()` en producción.
- Usar migraciones (`Database.Migrate()`) o pipeline de despliegue dedicado.
- No ejecutar seed masivo automáticamente en cada arranque de producción.
- Controlar seed por ambiente (`Development`/flag explícito).

## 10) `Guia.API/Properties/launchSettings.json`
- Mantener solo perfiles de desarrollo local.
- No usar como fuente de configuración de producción.

---

## Cambios Nuevos Recomendados (Archivos A Crear)

1. `Guia.API/Security/JwtOptions.cs`
- Clase de opciones tipadas para configuración JWT.

2. `Guia.API/Security/CurrentUserService.cs`
- Servicio para extraer `personaId` desde `ClaimsPrincipal`.

3. `Guia.API/Models/Auth/RegisterRequestDto.cs`
4. `Guia.API/Models/Auth/LoginRequestDto.cs`
5. `Guia.API/Models/Auth/AuthResponseDto.cs`
- DTOs de autenticación y registro.

6. `Guia.API/Middleware/GlobalExceptionMiddleware.cs`
- Manejo de errores uniforme sin fuga de internals.

7. `Guia.API/Extensions/SecurityServiceCollectionExtensions.cs`
- Registro centralizado de CORS, JWT, autorización y rate limiting.

---

## Políticas De Seguridad Mínimas (Producción)

1. TLS obligatorio extremo a extremo.
2. Rotación de secretos y credenciales comprometidas.
3. Principio de mínimo privilegio para DB user (no usar `sa`).
4. Logs estructurados con correlación y sin datos sensibles.
5. Backups cifrados y prueba de restauración.
6. Política de parches de dependencias (`dotnet list package --vulnerable`).

---

## Plan De Implementación (1 Sprint)

## Semana 1 (Alta prioridad)
1. Secretos fuera de repositorio + rotación de DB credenciales.
2. JWT + `[Authorize]` + ownership checks en controladores.
3. Hash de contraseñas + migración de datos/columna.
4. CORS restringido + HTTPS/HSTS.

## Semana 2 (Estabilización)
1. Middleware global de errores.
2. Rate limiting.
3. Ajuste de endpoints `GET` mutantes a `POST/PUT`.
4. Pruebas de integración mínimas para auth y autorización por recurso.

---

## Criterios De Aceptación (Nivel Medio)

1. No existen secretos en código ni en `appsettings.json` productivo.
2. Ningún endpoint de datos personales responde sin token válido.
3. Un usuario no puede leer/modificar datos de otro cambiando IDs en URL.
4. Contraseñas almacenadas solo como hash seguro.
5. CORS y HTTPS configurados con políticas de producción.
6. Errores al cliente sin detalles internos.
