using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guia.API.Models
{
    public class Arcano
    {
            [Key]
            public int Id { get; set; }
            
            [Required]
            public int Numero { get; set; } // Del 0 al 21
            
            [Required]
            [StringLength(100)]
            public string? Nombre { get; set; }
            
            [StringLength(50)]
            public string? LetraHebrea { get; set; }
            
            public string? SignificadoEsoterico { get; set; }
            
            public string? Mensaje { get; set; }
            
            public string? ElementoOPlaneta { get; set; }
            
            // Esta URL servirá para pintar la cartita en el árbol
            public string? ImagenUrl { get; set; }
    }

   
}