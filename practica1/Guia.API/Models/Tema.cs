using System.Collections.Generic;
using System;

namespace Guia.API.Models
{
    public class Tema
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty; // Ej: "Lección de Vida"
        public string DescripcionGeneral { get; set; } = string.Empty;
        
        public bool EstaActivo { get; set; } = true;
        public bool EsGratis { get; set; } = false; 
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Relación: Un tema tiene muchos significados
        public virtual ICollection<Significado> Significados { get; set; } = new List<Significado>();
    }
}