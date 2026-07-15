using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guia.API.Models
{
    public class PersonaDetalle
    {
        [Key]
        public int Id { get; set; }

        // Clave Foránea hacia Persona
        [Required]
        public int PersonaId { get; set; }

        [StringLength(50)]
        public string SignoZodiaco { get; set; } = string.Empty;

        [StringLength(50)]
        public string FaseLunar { get; set; } = string.Empty;

        [StringLength(50)]
        public string Elemento { get; set; } = string.Empty; // Fuego, Tierra, etc.
        public string VibracionDia { get; set; } = string.Empty; // Se calculará al entrar
        public string MensajeGratitud { get; set; } = string.Empty; // Mensaje único
        public string NotasAstrologicas { get; set; } = string.Empty;

        [ForeignKey("PersonaId")]
        public virtual Persona? Persona { get; set; }
    }
}