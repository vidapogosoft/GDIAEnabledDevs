using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guia.API.Models
{
    public class PersonaNumerologia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonaId { get; set; }

        // Aquí guardamos los números finales calculados
        public int MisionVida { get; set; }
        public int? NumeroAlma { get; set; }      // Vocales del nombre
        public int NumeroPersonalidad { get; set; } // Consonantes del nombre
        public int NumeroDestino { get; set; }    // Suma de nombre completo
        public int LeccionVida { get; set; }      // Otro cálculo de fecha

        [ForeignKey("PersonaId")]
        public virtual Persona? Persona { get; set; }
    }
}