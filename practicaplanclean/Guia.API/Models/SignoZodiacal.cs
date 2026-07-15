using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guia.API.Models
{// Tabla para Signos (Aries, Tauro, etc.)
    public class SignoZodiacal {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty; // Ej: "Capricornio"
        public string Icono { get; set; } = string.Empty; // Ej: "♑"
        public string Elemento { get; set; } = string.Empty; // Tierra, Fuego...
        public string DescripcionLarga { get; set; } = string.Empty; // La del PDF
        public string PalabrasClave { get; set; } = string.Empty;
    }
}