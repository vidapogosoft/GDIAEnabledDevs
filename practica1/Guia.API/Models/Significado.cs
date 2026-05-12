using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guia.API.Models
{
    public class Significado
    {
        public int Id { get; set; }
        public int ValorNumero { get; set; } 
        
        // Campos detallados para tu contenido de transformación
        public string Apodo { get; set; } = string.Empty;     // Ej: "El Líder y Pionero"
        public string Mision { get; set; } = string.Empty;    // Ej: "Ser un faro de inspiración..."
        public string Reto { get; set; } = string.Empty;      // Ej: "Superar el miedo al fracaso..."
        public string Mantra { get; set; } = string.Empty;    // Ej: "Confío en mi poder interior..."
        public string Amuleto { get; set; } = string.Empty;   // Ej: "Un cristal de cuarzo rojo..."
        public string MensajeMagico { get; set; } = string.Empty; // Ej: "Eres el inicio del cambio."
        
        // Relación con Temas
        public int TemaId { get; set; }
        
        [ForeignKey("TemaId")]
        public virtual Tema? Tema { get; set; }
    }
}