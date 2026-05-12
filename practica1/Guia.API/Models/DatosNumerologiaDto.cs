namespace Guia.API.Models
{
    public class DatosNumerologiaDto
    {
            // Insumos desde la fecha
        public int DiaNacimiento { get; set; }
        public int MesNacimiento { get; set; }
        public int AnioNacimiento { get; set; }
        
        // Insumos desde los nombres (Ya calculados previamente)
        public int NumeroAlma { get; set; }        // Vocales
        public int NumeroPersonalidad { get; set; } // Consonantes
        public int NumeroDestino { get; set; }      // Nombre completo
        public int MisionVida { get; set; }         // Fecha total
        public int RegaloDivino { get; set; }       // Dos últimos dígitos año o cálculo especial
    }
}