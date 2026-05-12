using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Guia.API.Models
{
    // Esto le dice a EF que estos campos deben ser únicos en la DB
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Username), IsUnique = true)]    public class Persona
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombres { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Apellidos { get; set; } = string.Empty;

        // Propiedad calculada en C# (No se guarda en la DB, se genera al consultar)
        [NotMapped]
        public string NombreCompleto => $"{Nombres} {Apellidos}";

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        // Propiedades de Navegación (Relaciones)
        public virtual PersonaDetalle? Detalle { get; set; }
        public virtual PersonaNumerologia? Numerologia { get; set; }
        // Agrega esta propiedad de navegación:
        public virtual ArbolVida? ArbolVida { get; set; }
    }
}