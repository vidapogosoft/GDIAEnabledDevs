// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;

// namespace Guia.API.Models
// {
//     public class ArbolVida
//     {
//         [Key]
//         public int Id { get; set; }

//         // Relación con la Persona
//         [Required]
//         public int PersonaId { get; set; }

//         [ForeignKey("PersonaId")]
//         public virtual Persona? Persona { get; set; }

//         // --- LAS 10 SEFIROT (VALOR Y NOMBRE) ---

//         [Display(Name = "1. Corona")]
//         [Column("Kether")]
//         public int Kether_Valor { get; set; }

//         [NotMapped]
//         public string Kether_Nombre { get; set; } = "Misión de Vida";

//         [Display(Name = "2. Sabiduría")]
//         [Column("Chokmah")]
//         public int Chokmah_Valor { get; set; }

//         [NotMapped]
//         public string Chokmah_Nombre { get; set; } = "Dones de Vidas Pasadas";

//         [Display(Name = "3. Entendimiento")]
//         [Column("Binah")]
//         public int Binah_Valor { get; set; }

//         [NotMapped]
//         public string Binah_Nombre { get; set; } = "Karma / Tikún";

//         [Display(Name = "4. Misericordia")]
//         [Column("Chesed")]
//         public int Chesed_Valor { get; set; }

//         [NotMapped]
//         public string Chesed_Nombre { get; set; } = "Abundancia y Expansión";

//         [Display(Name = "5. Severidad")]
//         [Column("Gevurah")]
//         public int Gevurah_Valor { get; set; }

//         [NotMapped]
//         public string Gevurah_Nombre { get; set; } = "Desafíos y Pruebas";

//         [Display(Name = "6. Belleza")]
//         [Column("Tiferet")]
//         public int Tiferet_Valor { get; set; }

//         [NotMapped]
//         public string Tiferet_Nombre { get; set; } = "Esencia / El Ser";

//         [Display(Name = "7. Victoria")]
//         [Column("Netzach")]
//         public int Netzach_Valor { get; set; }

//         [NotMapped]
//         public string Netzach_Nombre { get; set; } = "Fortaleza Interior";

//         [Display(Name = "8. Gloria")]
//         [Column("Hod")]
//         public int Hod_Valor { get; set; }

//         [NotMapped]
//         public string Hod_Nombre { get; set; } = "Mente y Comunicación";

//         [Display(Name = "9. Fundamento")]
//         [Column("Yesod")]
//         public int Yesod_Valor { get; set; }

//         [NotMapped]
//         public string Yesod_Nombre { get; set; } = "Subconsciente y Raíz";

//         [Display(Name = "10. Reino")]
//         [Column("Malchut")]
//         public int Malchut_Valor { get; set; }

//         [NotMapped]
//         public string Malchut_Nombre { get; set; } = "Realización Física / Destino";

//         // --- LOS 22 SENDEROS ---
//         // Guardaremos el número del Arcano Mayor (0-21) que corresponde a cada sendero.
//         // Esto permite que al cargar el árbol sepamos exactamente qué carta dibujar.
        
//         public int Sendero_1_2 { get; set; } // De Kether a Chokmah
//         public int Sendero_1_3 { get; set; } // De Kether a Binah
//         public int Sendero_1_6 { get; set; } // De Kether a Tiferet
//         public int Sendero_2_3 { get; set; } // De Chokmah a Binah
//         public int Sendero_2_4 { get; set; } // De Chokmah a Chesed
//         public int Sendero_2_6 { get; set; } // De Chokmah a Tiferet
//         public int Sendero_3_5 { get; set; } // De Binah a Gevurah
//         public int Sendero_3_6 { get; set; } // De Binah a Tiferet
//         public int Sendero_4_5 { get; set; } // De Chesed a Gevurah
//         public int Sendero_4_6 { get; set; } // De Chesed a Tiferet
//         public int Sendero_4_7 { get; set; } // De Chesed a Netzach
//         public int Sendero_5_6 { get; set; } // De Gevurah a Tiferet
//         public int Sendero_5_8 { get; set; } // De Gevurah a Hod
//         public int Sendero_6_7 { get; set; } // De Tiferet a Netzach
//         public int Sendero_6_8 { get; set; } // De Tiferet a Hod
//         public int Sendero_6_9 { get; set; } // De Tiferet a Yesod
//         public int Sendero_7_8 { get; set; } // De Netzach a Hod
//         public int Sendero_7_9 { get; set; } // De Netzach a Yesod
//         public int Sendero_7_10 { get; set; } // De Netzach a Malchut
//         public int Sendero_8_9 { get; set; } // De Hod a Yesod
//         public int Sendero_8_10 { get; set; } // De Hod a Malchut
//         public int Sendero_9_10 { get; set; } // De Yesod a Malchut
        
//         public DateTime FechaCalculo { get; set; } = DateTime.Now;
//     }
// }
