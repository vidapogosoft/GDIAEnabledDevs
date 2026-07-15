using System.ComponentModel.DataAnnotations;

namespace Guia.API.Models
{
    public class RetoSemanal
    {
        [Key]
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        // Instrucciones detalladas para que el usuario sepa CÓMO hacerlo
        public string Instrucciones { get; set; } = string.Empty;
        public int? NumeroAsociado { get; set; } // Vincula con Misión, Alma, etc. (1-9)
        // Relación con el Tema (1 = Misión, 2 = Alma, etc.)
        [Required]
        public int TemaId { get; set; }
        public virtual Tema? Tema { get; set; }
        public DateTime FechaInicio { get; set; } = DateTime.Now;
        public DateTime? FechaFin { get; set; } 
        public bool Activo { get; set; } = true;
        public bool EsGlobal { get; set; } // <--- NUEVO: Para retos de suscripción
        // Nivel de dificultad o intensidad espiritual
        public string Dificultad { get; set; } = "Básico"; // Básico, Intermedio, Avanzado
    }
}