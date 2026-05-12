using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guia.API.Models
{
    public class Bitacora
    {
        [Key]
        public int Id { get; set; }
        
        public int PersonaId { get; set; }
        
        [Required]
        public DateTime Fecha { get; set; }

        // AQUÍ DIFERENCIAMOS: "Sincronicidad", "Reto", "Sentimiento"
        [Required]
        public string Tipo { get; set; } = string.Empty; 

        public string Contenido { get; set; } = string.Empty; // El texto que escribe el usuario
        
        public string? ValorSincronico { get; set; } // Ejemplo: "11:11" o "222"
        public string? EstadoReto { get; set; } = "Completado"; // Valores: 'En Curso', 'Completado'
        
        public int? RetoId { get; set; } // Si es de tipo 'Reto', guardamos cuál reto cumplió

        [ForeignKey("PersonaId")]
        public virtual Persona? Persona { get; set; }

        [ForeignKey("RetoId")]
        public virtual RetoSemanal? Reto { get; set; }
    }
}