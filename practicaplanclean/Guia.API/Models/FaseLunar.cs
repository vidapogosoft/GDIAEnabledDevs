using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guia.API.Models
{   
    // Tabla para Fases Lunares
    public class FaseLunar {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty; // Ej: "Cuarto Menguante"
        public string Icono { get; set; } = string.Empty; // Ej: "🌗"
        public string SignificadoEspiritual { get; set; } = string.Empty; // Tu libro
        public string Recomendacion { get; set; } = string.Empty; // "Es tiempo de soltar..."
    }
    
}