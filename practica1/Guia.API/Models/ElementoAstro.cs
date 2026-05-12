using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guia.API.Models
{   
    // Tabla para Elementos
    public class ElementoAstro {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty; // Tierra, Fuego...
        public string Icono { get; set; } = string.Empty; // 🌍, 🔥
        public string Esencia { get; set; } = string.Empty; // "La estabilidad y manifestación"
        public string Descripcion { get; set; } = string.Empty;
    }
        
}