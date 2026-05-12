using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guia.API.Models
{
    public class FraseGratitud
    {
        public int Id { get; set; }
        public string Texto { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty; // Ej: "Amor", "Éxito", etc.
    }
}