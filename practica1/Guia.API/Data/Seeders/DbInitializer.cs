using Guia.API.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Guia.API.Data.Seeders
{
    public static class DbInitializer
    {
       public static void Initialize(ApplicationDbContext context)
        {
            // Crea la base de datos si no existe
            context.Database.EnsureCreated();

            

          // --- 1. CREACIÓN DE TODOS LOS TEMAS ---
            var configuracionTemas = new List<(string Titulo, string Descripcion)>
            {
                ("Lección de Vida", "Representa el camino que has venido a recorrer y las herramientas con las que cuentas."),
                ("Número del Alma", "Indica tus deseos más profundos, tus motivaciones internas y tu esencia espiritual."),
                ("Destino", "El camino que tu alma eligió recorrer y la meta de tu vida."),
                ("Personalidad", "La imagen que proyectas al mundo y cómo te perciben los demás."),
                ("Año Personal", "La energía específica que te acompaña durante este ciclo anual."),
                ("Año Universal", "La vibración colectiva que afecta a todo el planeta durante este año."),
                ("Vibración Diaria", "La vibración que te guía en tu día a día.")
            };

            foreach (var tInfo in configuracionTemas) {
                if (!context.Temas.Any(t => t.Titulo == tInfo.Titulo)) {
                    context.Temas.Add(new Tema {
                        Titulo = tInfo.Titulo,
                        DescripcionGeneral = tInfo.Descripcion,
                        EstaActivo = true,
                        EsGratis = true,
                        FechaCreacion = DateTime.Now
                    });
                }
            }
            context.SaveChanges();

            // --- 2. CARGA DE SIGNIFICADOS COMPLETOS ---

            // A. LECCIÓN DE VIDA (Tu información exacta)
            var temaLeccion = context.Temas.First(t => t.Titulo == "Lección de Vida");
            if (!context.Significados.Any(s => s.TemaId == temaLeccion.Id)) {
                var listaLeccion = new List<Significado> {
                    new Significado { ValorNumero = 1, Apodo = "El Líder y Pionero", Mision = "Ser un faro de inspiración, liderando con valentía y autenticidad. Aprender a confiar en ti mismo y ser independiente.", Reto = "Superar el miedo al fracaso y confiar en tu intuición para tomar decisiones.", Mantra = "“Confío en mi poder interior para crear mi camino único.”", Amuleto = "Un cristal de cuarzo rojo o un dije en forma de estrella.", MensajeMagico = "Eres el inicio del cambio. El universo te ha elegido para abrir nuevas puertas.", TemaId = temaLeccion.Id },
                    new Significado { ValorNumero = 2, Apodo = "El Mediador y Pacificador", Mision = "Crear armonía en las relaciones y ser un puente entre personas y emociones. Tu empatía es tu mayor fortaleza.", Reto = "Aprender a priorizarte y evitar que otros abusen de tu bondad.", Mantra = "“Encuentro la paz en mí y la comparto con el mundo.”", Amuleto = "Una piedra de luna o una pluma blanca.", MensajeMagico = "Eres la conexión entre lo visible y lo invisible, tejiendo hilos de amor y comprensión.", TemaId = temaLeccion.Id },
                    new Significado { ValorNumero = 3, Apodo = "El Comunicador y Artista", Mision = "Inspirar alegría y creatividad en los demás a través de tu arte o palabras.", Reto = "Mantener el enfoque y superar la procrastinación.", Mantra = "“Mi creatividad fluye y transforma mi realidad.”", Amuleto = "Un colibrí o un cristal citrino.", MensajeMagico = "Tus palabras y acciones tienen el poder de sanar corazones y encender almas.", TemaId = temaLeccion.Id },
                    new Significado { ValorNumero = 4, Apodo = "El Constructor y Organizador", Mision = "Establecer estructuras sólidas y trabajar con disciplina para construir un legado duradero.", Reto = "Aceptar los cambios inesperados con flexibilidad.", Mantra = "“Construyo con propósito, creando un futuro sólido y lleno de luz.”", Amuleto = "Una pirámide pequeña o un ónix negro.", MensajeMagico = "Eres el arquitecto del orden divino, creando equilibrio en un mundo caótico.", TemaId = temaLeccion.Id },
                    new Significado { ValorNumero = 5, Apodo = "El Explorador y Aventurero", Mision = "Explorar lo desconocido, buscar la libertad y enseñar a otros a adaptarse al cambio.", Reto = "Encontrar estabilidad sin sacrificar tu deseo de aventura.", Mantra = "“Abrazo el cambio como el motor de mi evolución.”", Amuleto = "Una brújula antigua o un ágata azul.", MensajeMagico = "El universo se mueve contigo. Eres la chispa que ilumina nuevos horizontes.", TemaId = temaLeccion.Id },
                    new Significado { ValorNumero = 6, Apodo = "El Cuidador y Sanador", Mision = "Servir con amor, creando un espacio seguro para los demás.", Reto = "Aprender a no sacrificarte en exceso por otros.", Mantra = "“Mi amor nutre y equilibra el mundo que me rodea.”", Amuleto = "Un corazón de cuarzo rosa o un dije en forma de flor.", MensajeMagico = "Tu energía amorosa sana las heridas del alma. Eres un canal de paz y armonía.", TemaId = temaLeccion.Id },
                    new Significado { ValorNumero = 7, Apodo = "El Buscador y Filósofo", Mision = "Descubrir los misterios del universo y conectar con la sabiduría interior.", Reto = "Mantener el equilibrio entre el aislamiento y la conexión con los demás.", Mantra = "“Exploro el universo dentro de mí para entender el mundo que me rodea.”", Amuleto = "Un ojo turco o una amatista.", MensajeMagico = "Tu mente es un portal hacia lo divino. Eres el buscador de verdades eternas.", TemaId = temaLeccion.Id },
                    new Significado { ValorNumero = 8, Apodo = "El Poderoso y Visionario", Mision = "Alcanzar el éxito material con integridad y usarlo para un bien mayor.", Reto = "Evitar la ambición desmedida o el apego excesivo al poder.", Mantra = "“El éxito fluye hacia mí porque lo uso para iluminar al mundo.”", Amuleto = "Un lingote pequeño de oro simbólico o un cristal de pirita.", MensajeMagico = "El universo confía en ti para manejar el poder con sabiduría y amor.", TemaId = temaLeccion.Id },
                    new Significado { ValorNumero = 9, Apodo = "El Humanitario y Sabio", Mision = "Servir al mundo con compasión y enseñar a otros a través de tu sabiduría.", Reto = "Soltar el pasado y aceptar los ciclos naturales de la vida.", Mantra = "“Sirvo al mundo con amor, compartiendo mi sabiduría eterna.”", Amuleto = "Un símbolo de infinito o un jaspe rojo.", MensajeMagico = "Eres la luz que guía a otros hacia su propósito más elevado.", TemaId = temaLeccion.Id },
                    new Significado { ValorNumero = 11, Apodo = "El Visionario Espiritual", Mision = "Elevar la consciencia colectiva a través de la intuición y la inspiración espiritual.", Reto = "Equilibrar la gran sensibilidad con el mundo material.", Mantra = "“Soy un canal divino de inspiración y luz.”", Amuleto = "Una estrella de seis puntas o un cristal angelita.", MensajeMagico = "Tu visión trasciende lo físico, eres un puente hacia lo divino.", TemaId = temaLeccion.Id },
                    new Significado { ValorNumero = 22, Apodo = "El Maestro Constructor", Mision = "Convertir visiones espirituales en proyectos materiales que beneficien a la humanidad.", Reto = "Manejar la presión de grandes responsabilidades.", Mantra = "“Manifiesto grandes sueños en realidades tangibles.”", Amuleto = "Una figura de mandala o un cuarzo transparente.", MensajeMagico = "Tus manos tienen el poder de construir el nuevo mundo.", TemaId = temaLeccion.Id },
                    new Significado { ValorNumero = 33, Apodo = "El Maestro Sanador", Mision = "Sanar a través del amor puro y la protección de los más vulnerables.", Reto = "Evitar el descuido propio por cuidar a los demás.", Mantra = "“Mi amor incondicional transforma el mundo.”", Amuleto = "Una vela blanca o un cuarzo verde.", MensajeMagico = "Eres el refugio del amor crístico en la tierra.", TemaId = temaLeccion.Id },
                    new Significado { ValorNumero = 44, Apodo = "El Maestro del Poder Material y Espiritual", Mision = "Organizar el mundo material con una ética espiritual inquebrantable.", Reto = "Mantener la humildad ante el gran poder alcanzado.", Mantra = "“Equilibro lo material y espiritual para crear un legado eterno.”", Amuleto = "Un anillo con forma de espiral o un cristal obsidiana.", MensajeMagico = "Eres la autoridad sagrada que manifiesta la abundancia divina.", TemaId = temaLeccion.Id }
                };
                context.Significados.AddRange(listaLeccion);
                context.SaveChanges();
            }

            // B. NÚMERO DEL ALMA (TEMA 2)
            var temaAlma = context.Temas.First(t => t.Titulo == "Número del Alma");
            if (!context.Significados.Any(s => s.TemaId == temaAlma.Id)) {
                context.Significados.AddRange(new List<Significado> {
                    new Significado { ValorNumero = 1, Apodo = "Chispa Independiente", Mision = "Tu alma anhela libertad y ser el autor de su historia.", Reto = "Controlar el ego.", Mantra = "“Mi voluntad es divina.”", Amuleto = "Pirita.", MensajeMagico = "Tu fuego interior es imparable.", TemaId = temaAlma.Id },
                    new Significado { ValorNumero = 2, Apodo = "Alma Compañera", Mision = "Buscas paz profunda, unión y vínculos reales.", Reto = "Decir no sin culpa.", Mantra = "“Amo con equilibrio.”", Amuleto = "Selenita.", MensajeMagico = "Eres el bálsamo del mundo.", TemaId = temaAlma.Id },
                    new Significado { ValorNumero = 3, Apodo = "Alma Expresiva", Mision = "Tu esencia se nutre de la risa y el arte.", Reto = "Evitar la dispersión.", Mantra = "“Expreso mi verdad.”", Amuleto = "Citrino.", MensajeMagico = "Tu risa es medicina pura.", TemaId = temaAlma.Id },
                    new Significado { ValorNumero = 4, Apodo = "El Alma Estable", Mision = "Anhelas seguridad, raíces fuertes y ver resultados tangibles de tus esfuerzos constantes.", Reto = "Soltar la rigidez mental y el miedo a la escasez o al cambio.", Mantra = "“Soy tierra firme y mi hogar interior es seguro.”", Amuleto = "Turmalina Negra o un cubo de madera.", MensajeMagico = "Eres la base sólida sobre la que se construyen los grandes sueños.", TemaId = temaAlma.Id },
                    new Significado { ValorNumero = 5, Apodo = "El Alma Libre", Mision = "Tu espíritu pide aventura, viajes y cambios constantes. Tu alma odia las ataduras.", Reto = "Aprender a comprometerte sin sentir que pierdes tus alas.", Mantra = "“Fluyo con los cambios y crezco en libertad.”", Amuleto = "Piedra Aguamarina o una pequeña llave.", MensajeMagico = "El viento siempre sopla a tu favor para llevarte a nuevas fronteras.", TemaId = temaAlma.Id },
                    new Significado { ValorNumero = 6, Apodo = "El Alma Protectora", Mision = "Tu plenitud viene de cuidar, embellecer y dar amor a tu familia o comunidad.", Reto = "Dejar de cargar problemas ajenos que no te corresponden solucionar.", Mantra = "“Cuido de mí tanto como cuido de los que amo.”", Amuleto = "Esmeralda o un dije de Árbol de la Vida.", MensajeMagico = "Eres el refugio seguro donde el amor siempre florece.", TemaId = temaAlma.Id },
                    new Significado { ValorNumero = 7, Apodo = "Alma Mística", Mision = "Deseas conocimiento oculto y silencio sagrado.", Reto = "No aislarte de la realidad.", Mantra = "“La verdad reside en mí.”", Amuleto = "Labradorita.", MensajeMagico = "Eres un canal con lo divino.", TemaId = temaAlma.Id },
                    new Significado { ValorNumero = 8, Apodo = "El Alma Abundante", Mision = "Buscas la maestría sobre el mundo material y el equilibrio perfecto del dar y recibir.", Reto = "Entender que la verdadera riqueza es un estado interno del ser.", Mantra = "“Merezco prosperidad y la comparto con sabiduría.”", Amuleto = "Ojo de Tigre o una moneda antigua.", MensajeMagico = "Tienes el poder de manifestar el cielo en la tierra.", TemaId = temaAlma.Id },
                    new Significado { ValorNumero = 9, Apodo = "Alma Universal", Mision = "Amor incondicional y cierre de ciclos.", Reto = "Soltar apegos pasados.", Mantra = "“Amo sin condiciones.”", Amuleto = "Lapislázuli.", MensajeMagico = "Tu alma es luz antigua.", TemaId = temaAlma.Id },
                    new Significado { ValorNumero = 11,Apodo = "El Alma Iluminada", Mision = "Tu esencia anhela despertar conciencias y vivir desde una intuición radical.", Reto = "Superar la hipersensibilidad y el nerviosismo extremo.", Mantra = "“Soy un puente entre el cielo y la tierra.”", Amuleto = "Un cristal de Angelita o Selenita.",MensajeMagico = "Tu alma vibra en una frecuencia superior; confía en tus visiones.",TemaId = temaAlma.Id},
                    new Significado { ValorNumero = 22, Apodo = "El Alma Constructora", Mision = "Deseas dejar un legado tangible que transforme la realidad de muchos.", Reto = "No dejarte aplastar por la magnitud de tus propios sueños.", Mantra = "“Construyo el cielo en la tierra con mis manos.”", Amuleto = "Un cuarzo ahumado o una piedra tectita.", MensajeMagico = "Tu ambición es sagrada porque busca el bienestar colectivo.", TemaId = temaAlma.Id},
                    new Significado { ValorNumero = 33, Apodo = "El Alma Crística", Mision = "Tu motor es el amor incondicional y la protección de la vida en todas sus formas.", Reto = "Evitar el martirio o el descuido total de tus propias necesidades.", Mantra = "“Mi amor es un escudo de luz para el mundo.”", Amuleto = "Una esmeralda o un dije de paloma.", MensajeMagico = "Has venido a enseñar el poder del perdón y la compasión infinita.", TemaId = temaAlma.Id},
                    new Significado { ValorNumero = 44, Apodo = "El Alma de Jerarquía", Mision = "Anhelas el dominio total de las leyes materiales bajo una ética espiritual inquebrantable.", Reto = "No volverte frío o demasiado enfocado en el control.", Mantra = "“Manejo el poder con integridad y sabiduría divina.”", Amuleto = "Un anillo de plata o una turmalina negra.", MensajeMagico = "Eres la autoridad espiritual manifestada en el mundo de la materia.", TemaId = temaAlma.Id}
                });
            }
            context.SaveChanges();

            // C. DESTINO (TEMA 3)
            var temaDestino = context.Temas.First(t => t.Titulo == "Destino");
            if (!context.Significados.Any(s => s.TemaId == temaDestino.Id)) {
                context.Significados.AddRange(new List<Significado> {
                    new Significado {
                        ValorNumero = 1, 
                        Apodo = "Liderazgo Soberano", 
                        Mision = "Tu destino es convertirte en un pionero independiente, abriendo caminos donde otros no se atreven a entrar. Debes aprender a confiar plenamente en tu visión.", 
                        Reto = "Superar la necesidad de aprobación externa y aceptar la soledad que a veces acompaña al líder.", 
                        Mantra = "“Lidero mi destino con honor y valentía.”", 
                        Amuleto = "Un dije de Sol o una moneda de oro.", 
                        MensajeMagico = "Naciste para abrir caminos. El universo espera que des el primer paso.", 
                        TemaId = temaDestino.Id 
                    },
                    new Significado { 
                        ValorNumero = 2, 
                        Apodo = "El Maestro de la Unión", 
                        Mision = "Tu destino es dominar el arte de la diplomacia y la cooperación. Estás aquí para unir voluntades y crear paz en entornos divididos.", 
                        Reto = "No perderte en las necesidades de los demás y mantener tu centro emocional.", 
                        Mantra = "“Soy el puente que une y la paz que armoniza.”", 
                        Amuleto = "Un cuarzo blanco o dos hilos entrelazados.", 
                        MensajeMagico = "Tu fuerza reside en la suavidad. Eres el pegamento que mantiene unido lo sagrado.", 
                        TemaId = temaDestino.Id 
                    },
                    new Significado { 
                        ValorNumero = 3, 
                        Apodo = "La Expresión Expandida", 
                        Mision = "Tu destino es comunicar verdades que inspiren alegría. Debes manifestar tu creatividad en el mundo material para sanar a otros.", 
                        Reto = "Enfocar tu talento disperso en una sola gran obra que deje huella.", 
                        Mantra = "“Mi voz es un canal de luz y creatividad.”", 
                        Amuleto = "Un cristal citrino o una pluma de ave.", 
                        MensajeMagico = "Viniste a recordarle al mundo que la vida es una celebración divina.", 
                        TemaId = temaDestino.Id 
                    },
                    new Significado { 
                        ValorNumero = 4, 
                        Apodo = "El Arquitecto del Legado", 
                        Mision = "Tu destino es construir algo duradero. Debes manifestar orden, disciplina y bases sólidas para las futuras generaciones.", 
                        Reto = "Aprender que la estructura no debe convertirse en una prisión; mantén la mente abierta.", 
                        Mantra = "“Construyo realidades sólidas con paciencia y fe.”", 
                        Amuleto = "Un ónix negro o una piedra cuadrada.", 
                        MensajeMagico = "Eres la mano que da forma a la materia. Tu trabajo trasciende el tiempo.", 
                        TemaId = temaDestino.Id 
                    },
                    new Significado { 
                        ValorNumero = 5, 
                        Apodo = "Libertad Evolutiva", 
                        Mision = "Tu destino es experimentar la vida en todas sus formas. Debes ser un agente de cambio y enseñar a otros a no temer a la incertidumbre.", 
                        Reto = "Aprender que la verdadera libertad nace de la disciplina interna, no del libertinaje.", 
                        Mantra = "“Soy libre en mi evolución y fluyo con el cambio.”", 
                        Amuleto = "Una brújula antigua o un ágata azul.", 
                        MensajeMagico = "Tu destino es el horizonte. Nunca dejes de explorar lo desconocido.", 
                        TemaId = temaDestino.Id 
                    },
                    new Significado { 
                        ValorNumero = 6, 
                        Apodo = "La Responsabilidad del Amor", 
                        Mision = "Tu destino es alcanzar el ideal del servicio amoroso. Estás aquí para sanar hogares y comunidades a través de la responsabilidad.", 
                        Reto = "No intentar arreglar la vida de todos; permite que cada uno aprenda su propia lección.", 
                        Mantra = "“Sirvo con amor y equilibrio mi entrega.”", 
                        Amuleto = "Un cuarzo rosa o un dije de flor.", 
                        MensajeMagico = "Eres el corazón del mundo. Donde pones tu intención, florece la vida.", 
                        TemaId = temaDestino.Id 
                    },
                    new Significado { 
                        ValorNumero = 7, 
                        Apodo = "La Sabiduría del Silencio", 
                        Mision = "Tu destino es convertirte en un especialista del espíritu. Debes buscar la verdad detrás de las apariencias y compartir tu conocimiento.", 
                        Reto = "Superar el escepticismo y confiar en las señales místicas del universo.", 
                        Mantra = "“Busco la verdad y la encuentro en mi interior.”", 
                        Amuleto = "Una amatista o un dije de búho.", 
                        MensajeMagico = "Tu mente es un laboratorio sagrado. Estás aquí para descifrar lo invisible.", 
                        TemaId = temaDestino.Id 
                    },
                    new Significado { 
                        ValorNumero = 8, 
                        Apodo = "Maestría Material", 
                        Mision = "Tu destino es manifestar abundancia y equilibrio de poder. Debes dirigir grandes proyectos con integridad y justicia.", 
                        Reto = "Ser justo con el éxito y recordar que el dinero es energía espiritual en movimiento.", 
                        Mantra = "“Soy abundancia consciente y poder equilibrado.”", 
                        Amuleto = "Una pirita o una moneda antigua.", 
                        MensajeMagico = "Eres un co-creador de realidades. El poder te ha sido dado para bendecir.", 
                        TemaId = temaDestino.Id 
                    },
                    new Significado { 
                        ValorNumero = 9, 
                        Apodo = "La Culminación Universal", 
                        Mision = "Tu destino es alcanzar la conciencia universal. Estás aquí para cerrar ciclos kármicos y enseñar el amor desinteresado.", 
                        Reto = "Soltar los apegos personales por una causa mucho más grande que tú mismo.", 
                        Mantra = "“Suelto el pasado y me entrego al servicio universal.”", 
                        Amuleto = "Un jaspe rojo o un símbolo de infinito.", 
                        MensajeMagico = "Tu destino es la luz total. Eres el final de un camino y el inicio de la eternidad.", 
                        TemaId = temaDestino.Id 
                    },
                    // NÚMEROS MAESTROS EN DESTINO
                    new Significado { 
                        ValorNumero = 11, 
                        Apodo = "El Faro de Conciencia", 
                        Mision = "Tu destino es ser un canal de iluminación. Debes actuar como un puente entre dimensiones para despertar a la humanidad.", 
                        Reto = "Confiar en tus visiones aunque el mundo no las comprenda aún.", 
                        Mantra = "“Soy luz en movimiento, guiando el despertar.”", 
                        Amuleto = "Una estrella o un cristal angelita.", 
                        MensajeMagico = "Tu voz tiene ecos de eternidad. Habla, que el mundo escucha.", 
                        TemaId = temaDestino.Id 
                    },
                    new Significado { 
                        ValorNumero = 22, 
                        Apodo = "El Visionario Universal", 
                        Mision = "Tu destino es materializar utopías. Estás aquí para crear sistemas que cambien la forma en que vive la humanidad.", 
                        Reto = "No rendirte ante la magnitud de tus propios sueños.", 
                        Mantra = "“Construyo el cielo en la tierra con mis manos.”", 
                        Amuleto = "Un cuarzo transparente o un mandala.", 
                        MensajeMagico = "El arquitecto divino trabaja a través de ti. Manifiesta lo imposible.", 
                        TemaId = temaDestino.Id 
                    },
                    new Significado { 
                        ValorNumero = 33, 
                        Apodo = "El Avatar del Amor", 
                        Mision = "Tu destino es la entrega total y el servicio a la humanidad a través del amor.", 
                        Reto = "No descuidar tu propia vida por los demás.", 
                        Mantra = "“Soy amor en acción.”", 
                        Amuleto = "Cuarzo Verde.", 
                        MensajeMagico = "Eres un canal de sanación universal.", 
                        TemaId = temaDestino.Id 
                    },
                    new Significado { 
                        ValorNumero = 44, 
                        Apodo = "El Estratega Divino", 
                        Mision = "Manifestar grandes estructuras que unan lo material con lo espiritual.", 
                        Reto = "La carga de la responsabilidad masiva.", 
                        Mantra = "“Materializo la luz.”", 
                        Amuleto = "Obsidiana dorada.", 
                        MensajeMagico = "Tu legado será eterno y sólido.", 
                        TemaId = temaDestino.Id 
                    }
                });
            }
            context.SaveChanges();

            // D. PERSONALIDAD (TEMA 4)
            var temaPers = context.Temas.First(t => t.Titulo == "Personalidad");
            if (!context.Significados.Any(s => s.TemaId == temaPers.Id)) {
                context.Significados.AddRange(new List<Significado> {
                    new Significado { 
                        ValorNumero = 1, 
                        Apodo = "Imagen Magnética", 
                        Mision = "El mundo te percibe como una persona segura, audaz y con una gran fuerza de voluntad. Proyectas la imagen de alguien que sabe exactamente a dónde va.", 
                        Reto = "Cuidar de no parecer alguien arrogante, frío o demasiado dominante ante los demás.", 
                        Mantra = "“Mi presencia es poderosa y mi luz abre caminos.”", 
                        Amuleto = "Un Rubí o una piedra de Granate.", 
                        MensajeMagico = "Tu imagen abre puertas que otros creen cerradas. Confía en tu impacto.", 
                        TemaId = temaPers.Id 
                    },
                    new Significado { 
                        ValorNumero = 2, 
                        Apodo = "Imagen Diplomática", 
                        Mision = "Te ven como una persona pacífica, amable y una excelente colaboradora. Proyectas una energía de apoyo y discreción que invita a la confianza.", 
                        Reto = "Superar la timidez excesiva y no permitir que tu presencia pase desapercibida por miedo a molestar.", 
                        Mantra = "“Mi suavidad es mi mayor fortaleza y escudo.”", 
                        Amuleto = "Una Perla o una piedra de Cuarzo Blanco.", 
                        MensajeMagico = "La gente busca tu compañía para encontrar calma en medio de la tormenta.", 
                        TemaId = temaPers.Id 
                    },
                    new Significado { 
                        ValorNumero = 3, 
                        Apodo = "Imagen Brillante", 
                        Mision = "Proyectas entusiasmo, alegría y una gran creatividad. El mundo te ve como alguien sociable, ingenioso y lleno de vida.", 
                        Reto = "No caer en la superficialidad o en el uso de la máscara del payaso para ocultar tus tristezas.", 
                        Mantra = "“Irradio alegría y mi creatividad es contagiosa.”", 
                        Amuleto = "Un Topacio o un dije de sol.", 
                        MensajeMagico = "Eres el centro de luz en cualquier lugar. Tu brillo natural es un regalo para otros.", 
                        TemaId = temaPers.Id 
                    },
                    new Significado { 
                        ValorNumero = 4, 
                        Apodo = "Imagen Respetable", 
                        Mision = "Te perciben como una persona seria, trabajadora y sumamente confiable. Proyectas orden, disciplina y respeto por las normas.", 
                        Reto = "Evitar parecer alguien demasiado rígido, aburrido o severo con los errores de los demás.", 
                        Mantra = "“Mi integridad es mi carta de presentación.”", 
                        Amuleto = "Un zafiro azul o una piedra de turmalina.", 
                        MensajeMagico = "Eres la roca en la que otros se apoyan. Tu estabilidad es tu mayor respeto.", 
                        TemaId = temaPers.Id 
                    },
                    new Significado { 
                        ValorNumero = 5, 
                        Apodo = "Imagen Versátil", 
                        Mision = "El mundo te ve como alguien magnético, adaptable y amante de la libertad. Proyectas una imagen juvenil, curiosa y aventurera.", 
                        Reto = "Controlar la inquietud excesiva y evitar proyectar una imagen de irresponsabilidad o falta de compromiso.", 
                        Mantra = "“Abrazo el cambio y mi energía fluye libremente.”", 
                        Amuleto = "Un cristal de Ojo de Tigre o una Turquesa.", 
                        MensajeMagico = "Eres un camaleón espiritual. Tu capacidad de adaptación es tu superpoder social.", 
                        TemaId = temaPers.Id 
                    },
                    new Significado { 
                        ValorNumero = 6, 
                        Apodo = "Imagen Armoniosa", 
                        Mision = "Te perciben como alguien protector, estético y profundamente hogareño. Proyectas una vibración maternal/paternal y de sanación.", 
                        Reto = "No cargar con las culpas ajenas ni volverte esclavo de las necesidades de tu entorno por el qué dirán.", 
                        Mantra = "“Irradio paz, amor y equilibrio en mi entorno.”", 
                        Amuleto = "Una Esmeralda o un dije de corazón.", 
                        MensajeMagico = "Eres el refugio de muchos. Tu sola presencia armoniza los espacios que habitas.", 
                        TemaId = temaPers.Id 
                    },
                    new Significado { 
                        ValorNumero = 7, 
                        Apodo = "Imagen Distinguida", 
                        Mision = "Proyectas un aire de misterio, intelecto y refinamiento. Te ven como alguien sabio, observador y quizás un poco solitario.", 
                        Reto = "Aprender a ser más accesible y no parecer alguien distante, crítico o frío con los demás.", 
                        Mantra = "“Mi sabiduría interna brilla con elegancia.”", 
                        Amuleto = "Una piedra de Amatista o una Labradorita.", 
                        MensajeMagico = "Tu aura es profunda y enigmática. Eres el buscador que otros admiran en silencio.", 
                        TemaId = temaPers.Id 
                    },
                    new Significado { 
                        ValorNumero = 8, 
                        Apodo = "Imagen Exitosa", 
                        Mision = "Te ven como una persona con gran autoridad, ambiciosa y con capacidad de mando. Proyectas éxito económico y eficiencia.", 
                        Reto = "No utilizar tu imagen para intimidar a otros ni enfocarte únicamente en las apariencias materiales.", 
                        Mantra = "“El poder y la abundancia se manifiestan en mí.”", 
                        Amuleto = "Un diamante o un cristal de Pirita.", 
                        MensajeMagico = "Naciste para manejar grandes energías. Tu impacto visual es el de un triunfador.", 
                        TemaId = temaPers.Id 
                    },
                    new Significado { 
                        ValorNumero = 9, 
                        Apodo = "Imagen Compasiva", 
                        Mision = "Proyectas una imagen de sabiduría, tolerancia y amor universal. Te ven como alguien idealista, generoso y desinteresado.", 
                        Reto = "No parecer alguien demasiado ingenuo o que vive fuera de la realidad por estar en las nubes.", 
                        Mantra = "“Mi amor universal envuelve a todo ser vivo.”", 
                        Amuleto = "Un Lapislázuli o un Ópalo.", 
                        MensajeMagico = "Eres un faro de esperanza. Tu mirada ve la belleza que otros ignoran.", 
                        TemaId = temaPers.Id 
                    },
                    // NÚMEROS MAESTROS EN PERSONALIDAD
                    new Significado { 
                        ValorNumero = 11, 
                        Apodo = "Imagen Inspiradora", 
                        Mision = "Proyectas una energía eléctrica y visionaria. La gente te ve como alguien que está un paso adelante, casi como un profeta moderno.", 
                        Reto = "Manejar la ansiedad que te genera ser observado y no sentirte 'raro' por tu alta sensibilidad.", 
                        Mantra = "“Soy un canal de inspiración divina aquí y ahora.”", 
                        Amuleto = "Una piedra de Selenita o un cristal Angelita.", 
                        MensajeMagico = "Tu imagen es un recordatorio de lo divino para quienes te cruzan en el camino.", 
                        TemaId = temaPers.Id 
                    },
                    new Significado { 
                        ValorNumero = 22, 
                        Apodo = "El Arquitecto del Mundo", 
                        Mision = "Proyectas una capacidad inmensa para materializar sueños a gran escala.", 
                        Reto = "No caer en el materialismo puro u olvidar el descanso.", 
                        Mantra = "“Construyo con sabiduría para el bien común.”", 
                        Amuleto = "Un cubo de pirita o algo de cuarzo rutilado.", 
                        MensajeMagico = "Tu sola presencia transmite que lo imposible es realizable.", 
                        TemaId = temaPers.Id 
                    },
                    new Significado { 
                        ValorNumero = 33, 
                        Apodo = "La Imagen de la Entrega", 
                        Mision = "Proyectas una calidez y un sacrificio que inspira devoción y confianza ciega.", 
                        Reto = "No dejar que otros se vuelvan dependientes de tu luz.", 
                        Mantra = "“Mi servicio es mi mayor brillo.”", 
                        Amuleto = "Un dije de coral o una turquesa.", 
                        MensajeMagico = "Eres la representación del amor incondicional en la tierra.", 
                        TemaId = temaPers.Id 
                    },
                    new Significado { 
                        ValorNumero = 44, 
                        Apodo = "La Imagen de la Estructura Sagrada", 
                        Mision = "Proyectas una autoridad implacable unida a una ética espiritual de acero.", 
                        Reto = "Parecer una figura demasiado rígida o difícil de alcanzar.", 
                        Mantra = "“Ordeno el mundo con la justicia del espíritu.”", 
                        Amuleto = "Un anillo de hierro o una obsidiana arcoíris.", 
                        MensajeMagico = "Transmites la fuerza de los antiguos constructores de templos.", 
                        TemaId = temaPers.Id
                    }
                });
            }
            context.SaveChanges();

            // E. AÑO PERSONAL (TEMA 5)
            var temaAñoP = context.Temas.First(t => t.Titulo == "Año Personal");
            if (!context.Significados.Any(s => s.TemaId == temaAñoP.Id)) {
                context.Significados.AddRange(new List<Significado> {
                    new Significado { 
                        ValorNumero = 1, 
                        Apodo = "Año de Inicios", 
                        Mision = "Es el momento de sembrar las semillas de lo que quieres cosechar en los próximos 9 años. Debes tomar la iniciativa y confiar en tus nuevos proyectos.", 
                        Reto = "Vencer la duda, la procrastinación y el miedo a emprender algo totalmente nuevo.", 
                        Mantra = "“Hoy comienza mi nueva vida y el universo respalda cada uno de mis pasos.”", 
                        Amuleto = "Ruda o un cristal de Citrino.", 
                        MensajeMagico = "Es tu año de renacer. Tienes el lienzo en blanco para escribir una nueva historia.", 
                        TemaId = temaAñoP.Id 
                    },
                    new Significado { 
                        ValorNumero = 2, 
                        Apodo = "Año de Gestación", 
                        Mision = "Aprender a esperar con paciencia. Es un tiempo para colaborar, fortalecer alianzas y permitir que lo sembrado en el año 1 eche raíces.", 
                        Reto = "Controlar la ansiedad por ver resultados rápidos y trabajar en la diplomacia.", 
                        Mantra = "“Fluyo con el tiempo perfecto de la naturaleza y cultivo mi paz.”", 
                        Amuleto = "Piedra de Luna o una cinta blanca.", 
                        MensajeMagico = "La magia sucede en el silencio y la cooperación. Escucha tu intuición.", 
                        TemaId = temaAñoP.Id 
                    },
                    new Significado { 
                        ValorNumero = 3, 
                        Apodo = "Año de Expansión", 
                        Mision = "Expresar tu creatividad y disfrutar de la vida social. Es un año para brillar, comunicar tus ideas y encontrar alegría en lo cotidiano.", 
                        Reto = "No dispersar tu energía en demasiadas cosas y evitar el gasto innecesario.", 
                        Mantra = "“Mi alegría se expande y atrae bendiciones a mi vida.”", 
                        Amuleto = "Un dije de colibrí o un cristal de Pirita.", 
                        MensajeMagico = "El universo celebra contigo. Tu entusiasmo es la llave de tu abundancia.", 
                        TemaId = temaAñoP.Id 
                    },
                    new Significado { 
                        ValorNumero = 4, 
                        Apodo = "Año de Construcción", 
                        Mision = "Trabajar con disciplina y poner orden en tu vida material y salud. Es tiempo de establecer bases sólidas y realistas.", 
                        Reto = "Superar la sensación de pesadez o restricción y evitar la terquedad.", 
                        Mantra = "“Con cada acción disciplinada, construyo mi libertad futura.”", 
                        Amuleto = "Una piedra de Ónix o un objeto de madera.", 
                        MensajeMagico = "Eres el arquitecto de tu realidad. El esfuerzo de hoy es la paz de mañana.", 
                        TemaId = temaAñoP.Id 
                    },
                    new Significado { 
                        ValorNumero = 5, 
                        Apodo = "Año de Cambios", 
                        Mision = "Adaptarte a lo inesperado y buscar la libertad. Es un año de viajes, movimiento y nuevas experiencias que desafían tu rutina.", 
                        Reto = "No caer en excesos o impulsividad que pongan en riesgo tu estabilidad.", 
                        Mantra = "“Abrazo la aventura del cambio y confío en mi adaptabilidad.”", 
                        Amuleto = "Una brújula o un cristal de Ágata azul.", 
                        MensajeMagico = "El viento del cambio sopla a tu favor. Atrévete a ser diferente.", 
                        TemaId = temaAñoP.Id 
                    },
                    new Significado { 
                        ValorNumero = 6, 
                        Apodo = "Año del Hogar", 
                        Mision = "Enfocarte en la familia, las responsabilidades afectivas y la armonía del entorno. Es un tiempo para sanar vínculos.", 
                        Reto = "No asumir cargas familiares que no te corresponden por un sentido de sacrificio.", 
                        Mantra = "“Mi hogar es un santuario de amor y yo soy su guardián.”", 
                        Amuleto = "Cuarzo rosa o una vela de color rosa.", 
                        MensajeMagico = "El amor es la fuerza que lo ordena todo. Nutre tu corazón.", 
                        TemaId = temaAñoP.Id 
                    },
                    new Significado { 
                        ValorNumero = 7, 
                        Apodo = "Año de Reflexión", 
                        Mision = "Hacer una pausa para el autoconocimiento, el estudio y la espiritualidad. Es un año de introspección profunda.", 
                        Reto = "Evitar el aislamiento melancólico o la tendencia a sobreanalizar todo.", 
                        Mantra = "“Busco la sabiduría en mi interior y encuentro todas las respuestas.”", 
                        Amuleto = "Amatista o un dije de búho.", 
                        MensajeMagico = "Estás en un retiro espiritual personal. Tu mente se está preparando para la cima.", 
                        TemaId = temaAñoP.Id 
                    },
                    new Significado { 
                        ValorNumero = 8, 
                        Apodo = "Año de Poder", 
                        Mision = "Manifestar tus ambiciones y manejar el éxito material. Es el año de la justicia kármica: recibes lo que has trabajado.", 
                        Reto = "Mantener el equilibrio entre la ambición y la ética espiritual.", 
                        Mantra = "“Soy un imán para la prosperidad y el éxito equilibrado.”", 
                        Amuleto = "Una moneda de la suerte o una piedra de Ojo de Tigre.", 
                        MensajeMagico = "Naciste para mandar y manifestar. El poder es una herramienta sagrada en tus manos.", 
                        TemaId = temaAñoP.Id 
                    },
                    new Significado { 
                        ValorNumero = 9, 
                        Apodo = "Año de Cierre", 
                        Mision = "Limpiar, soltar, perdonar y agradecer. Es el final de un ciclo de 9 años; debes dejar ir lo que ya no vibra contigo.", 
                        Reto = "Vencer la resistencia a soltar personas, objetos o situaciones del pasado.", 
                        Mantra = "“Suelto el pasado con amor, confío en el presente y agradezco lo vivido.”", 
                        Amuleto = "Sal de mar o una pluma blanca.", 
                        MensajeMagico = "Cierras un gran ciclo hoy. Vacía tus manos para que el universo las llene de nuevo mañana.", 
                        TemaId = temaAñoP.Id 
                    }
                });
            }
            context.SaveChanges();

            // F. AÑO UNIVERSAL (TEMA 6)
            var temaAñoU = context.Temas.First(t => t.Titulo == "Año Universal");
            if (!context.Significados.Any(s => s.TemaId == temaAñoU.Id)) {
                context.Significados.AddRange(new List<Significado> {
                    new Significado { 
                            ValorNumero = 1, 
                            Apodo = "Año Global de Despertar", 
                            Mision = "Es el inicio de un ciclo de 9 años para la humanidad. Representa un despertar masivo, nuevas tecnologías y el nacimiento de ideologías que cambiarán el mundo.", 
                            Reto = "Adaptarse a la rapidez de los cambios y soltar las estructuras obsoletas del pasado.", 
                            Mantra = "“Somos uno en el inicio de una nueva era de luz.”", 
                            Amuleto = "Cuarzo Cristal o una punta de cuarzo transparente.", 
                            MensajeMagico = "El mundo renace contigo. Tu chispa individual es parte del gran incendio del despertar.", 
                            TemaId = temaAñoU.Id 
                        },
                        new Significado { 
                            ValorNumero = 2, 
                            Apodo = "Año Global de Alianzas", 
                            Mision = "Un tiempo para la diplomacia mundial, la búsqueda de la paz y el fortalecimiento de las relaciones internacionales y colectivas.", 
                            Reto = "Superar la polarización y aprender a trabajar en equipo por el bien común.", 
                            Mantra = "“En la unión encontramos la fuerza para sanar el planeta.”", 
                            Amuleto = "Piedra de Luna o una cinta de color azul cielo.", 
                            MensajeMagico = "La cooperación es la clave del milagro. Juntos somos el puente hacia lo nuevo.", 
                            TemaId = temaAñoU.Id 
                        },
                        new Significado { 
                            ValorNumero = 3, 
                            Apodo = "Año Global de Expresión", 
                            Mision = "Un periodo de gran creatividad colectiva, avances en la comunicación y una expansión de las artes y la alegría social.", 
                            Reto = "Evitar la superficialidad y el uso de la comunicación para desinformar.", 
                            Mantra = "“La voz del mundo canta una melodía de esperanza.”", 
                            Amuleto = "Citrino o un dije de sol radiante.", 
                            MensajeMagico = "La alegría es un acto de rebeldía espiritual. Brilla con el mundo.", 
                            TemaId = temaAñoU.Id 
                        },
                        new Significado { 
                            ValorNumero = 4, 
                            Apodo = "Año Global de Estabilidad", 
                            Mision = "Tiempo de poner orden en la economía mundial, construir nuevas bases legales y enfocarse en la ecología y el trabajo duro.", 
                            Reto = "No caer en la rigidez extrema o en sistemas totalitarios por miedo al caos.", 
                            Mantra = "“Construimos sobre roca firme el futuro de la humanidad.”", 
                            Amuleto = "Ónix negro o un puñado de tierra sagrada.", 
                            MensajeMagico = "El orden divino se manifiesta en la tierra. Eres parte del cimiento.", 
                            TemaId = temaAñoU.Id 
                        },
                        new Significado { 
                            ValorNumero = 5, 
                            Apodo = "Año Global de Transformación", 
                            Mision = "Un año de revoluciones, descubrimientos científicos asombrosos y una gran necesidad de libertad colectiva.", 
                            Reto = "Manejar la inestabilidad social y los cambios repentinos con sabiduría.", 
                            Mantra = "“La libertad es el motor que mueve nuestra evolución universal.”", 
                            Amuleto = "Ágata azul o una pequeña brújula de bolsillo.", 
                            MensajeMagico = "El universo acelera su marcha. Siente el viento del cambio en tu rostro.", 
                            TemaId = temaAñoU.Id 
                        },
                        new Significado { 
                            ValorNumero = 6, 
                            Apodo = "Año Global de Responsabilidad", 
                            Mision = "Fomento del cuidado comunitario, la educación, la salud pública y el bienestar de los sectores más vulnerables.", 
                            Reto = "Equilibrar el servicio social sin caer en el control excesivo de la privacidad.", 
                            Mantra = "“El amor y la justicia son la ley que rige nuestra unión.”", 
                            Amuleto = "Cuarzo rosa o un símbolo de flor de la vida.", 
                            MensajeMagico = "La humanidad es una sola familia. Tu servicio es el latido de este corazón.", 
                            TemaId = temaAñoU.Id 
                        },
                        new Significado { 
                            ValorNumero = 7, 
                            Apodo = "Año Global de Espiritualidad", 
                            Mision = "Un tiempo de introspección para el planeta, descubrimientos en el espacio y un interés masivo por lo místico y científico.", 
                            Reto = "Evitar el fanatismo y buscar la verdad basada en el conocimiento y la fe equilibrada.", 
                            Mantra = "“La sabiduría del universo se revela a quienes saben escuchar.”", 
                            Amuleto = "Amatista o un cristal de Labradorita.", 
                            MensajeMagico = "El cielo toca la tierra este año. Escucha los susurros de lo divino.", 
                            TemaId = temaAñoU.Id 
                        },
                        new Significado { 
                            ValorNumero = 8, 
                            Apodo = "Año Global de Cosecha", 
                            Mision = "Justicia kármica a nivel mundial. Reequilibrio de la riqueza, el poder y las instituciones financieras.", 
                            Reto = "Superar la ambición desmedida y usar el poder para el beneficio de todos.", 
                            Mantra = "“La abundancia del universo es infinita y equitativa.”", 
                            Amuleto = "Pirita o una moneda de plata.", 
                            MensajeMagico = "El poder se entrega a quienes saben servir. Es el año de la gran manifestación.", 
                            TemaId = temaAñoU.Id 
                        },
                        new Significado { 
                            ValorNumero = 9, 
                            Apodo = "Año Global de Cierre", 
                            Mision = "Final de un ciclo de 9 años. Tiempo de perdonar deudas históricas, soltar viejos paradigmas y prepararse para el renacimiento.", 
                            Reto = "No resistirse a los finales necesarios que la humanidad debe atravesar.", 
                            Mantra = "“Soltamos lo viejo con gratitud y nos preparamos para la luz.”", 
                            Amuleto = "Sal de mar o un incienso de copal.", 
                            MensajeMagico = "La noche es más oscura justo antes del amanecer. Confía en el final del ciclo.", 
                            TemaId = temaAñoU.Id
                        }
                });
            }

            context.SaveChanges();

            // --- CARGA DE SIGNIFICADOS: VIBRACIÓN DIARIA (TEMA 7) ---
            var temaVibracion = context.Temas.First(t => t.Titulo == "Vibración Diaria");
            if (!context.Significados.Any(s => s.TemaId == temaVibracion.Id)) {
                context.Significados.AddRange(new List<Significado> {
                    new Significado { 
                        ValorNumero = 1, 
                        Apodo = "Día de Inicios", 
                        Mision = "Hoy la energía te impulsa a tomar la iniciativa. Es un excelente día para comenzar un proyecto, hacer una llamada importante o decidir un cambio personal. La fuerza del '1' te da valentía.", 
                        Reto = "Vencer la indecisión y no esperar a que otros den el primer paso por ti.", 
                        Mantra = "“Hoy soy el arquitecto de mi realidad y avanzo con determinación.”", 
                        Amuleto = "Un objeto de color rojo o una piedra Jaspe.", 
                        MensajeMagico = "El universo te da luz verde hoy. Confía en tu primer impulso.", 
                        TemaId = temaVibracion.Id 
                    },
                    new Significado { 
                        ValorNumero = 2, 
                        Apodo = "Día de Cooperación", 
                        Mision = "Hoy es un día para escuchar, colaborar y trabajar en equipo. La energía favorece las alianzas, las reconciliaciones y los detalles sutiles.", 
                        Reto = "No tomarte las críticas de forma personal y mantener tu equilibrio emocional.", 
                        Mantra = "“Fluyo en armonía con los demás y escucho mi voz interior.”", 
                        Amuleto = "Cuarzo blanco o algo de color plata.", 
                        MensajeMagico = "La magia hoy está en los detalles y en la unión. Sé paciente.", 
                        TemaId = temaVibracion.Id 
                    },
                    new Significado { 
                        ValorNumero = 3, 
                        Apodo = "Día de Expresión", 
                        Mision = "La energía de hoy es expansiva y alegre. Es el momento ideal para comunicar tus ideas, escribir, crear arte o simplemente disfrutar de una buena charla.", 
                        Reto = "Evitar la dispersión y no gastar energía en chismes o quejas innecesarias.", 
                        Mantra = "“Mi palabra crea belleza y mi alegría es contagiosa.”", 
                        Amuleto = "Citrino o algo de color amarillo/oro.", 
                        MensajeMagico = "Tu sonrisa es tu mejor talismán hoy. Comparte tu brillo.", 
                        TemaId = temaVibracion.Id 
                    },
                    new Significado { 
                        ValorNumero = 4, 
                        Apodo = "Día de Orden", 
                        Mision = "Hoy toca poner los pies en la tierra. Organiza tu agenda, limpia tu espacio o termina esas tareas pendientes que requieren disciplina.", 
                        Reto = "No dejarte ganar por el cansancio o la sensación de rutina pesada.", 
                        Mantra = "“Organizo mi mundo y construyo bases sólidas para mis sueños.”", 
                        Amuleto = "Ónix o algo de color café/verde oscuro.", 
                        MensajeMagico = "La disciplina de hoy es la libertad de mañana. Construye con fe.", 
                        TemaId = temaVibracion.Id 
                    },
                    new Significado { 
                        ValorNumero = 5, 
                        Apodo = "Día de Aventura", 
                        Mision = "Hoy el universo te pide movimiento. Rompe la rutina, prueba algo nuevo o haz un viaje corto. La energía es rápida y llena de sorpresas.", 
                        Reto = "Controlar la impulsividad y no tomar decisiones permanentes basadas en emociones temporales.", 
                        Mantra = "“Abrazo lo inesperado y aprendo de cada nueva experiencia.”", 
                        Amuleto = "Aguamarina o algo de color azul turquesa.", 
                        MensajeMagico = "El cambio es la única constante. ¡Diviértete explorando hoy!", 
                        TemaId = temaVibracion.Id 
                    },
                    new Significado { 
                        ValorNumero = 6, 
                        Apodo = "Día del Corazón", 
                        Mision = "Hoy la prioridad es el hogar y los seres queridos. Dedica tiempo a sanar vínculos, embellecer tu casa o dar un consejo amoroso.", 
                        Reto = "Evitar querer controlar la vida de los demás bajo la excusa de cuidarlos.", 
                        Mantra = "“Doy y recibo amor en equilibrio perfecto.”", 
                        Amuleto = "Cuarzo rosa o algo de color rosa/verde.", 
                        MensajeMagico = "Donde pones tu amor hoy, nace un milagro. Cuida tu santuario.", 
                        TemaId = temaVibracion.Id 
                    },
                    new Significado { 
                        ValorNumero = 7, 
                        Apodo = "Día de Introspección", 
                        Mision = "Un día para el silencio y el estudio. Busca un momento para meditar, leer o investigar un tema profundo. Tu intuición está al máximo.", 
                        Reto = "No caer en pensamientos melancólicos o aislarte por miedo al mundo.", 
                        Mantra = "“En el silencio encuentro las respuestas que buscaba.”", 
                        Amuleto = "Amatista o algo de color violeta.", 
                        MensajeMagico = "Hay un mensaje oculto para ti hoy. Escucha con el alma.", 
                        TemaId = temaVibracion.Id 
                    },
                    new Significado { 
                        ValorNumero = 8, 
                        Apodo = "Día de Manifestación", 
                        Mision = "La energía favorece los negocios, el dinero y el poder personal. Es un buen día para negociar o concretar temas materiales importantes.", 
                        Reto = "Actuar con total integridad; recuerda que lo que das hoy, vuelve multiplicado.", 
                        Mantra = "“Soy un canal de abundancia y manejo mi poder con sabiduría.”", 
                        Amuleto = "Pirita o algo de color cobre/bronce.", 
                        MensajeMagico = "Eres un imán para la prosperidad. Proyecta tu éxito.", 
                        TemaId = temaVibracion.Id 
                    },
                    new Significado { 
                        ValorNumero = 9, 
                        Apodo = "Día de Cierre", 
                        Mision = "Hoy es para soltar. Termina lo que empezaste, perdona, limpia tus cajones y prepárate para un nuevo ciclo. Momento de altruismo.", 
                        Reto = "No aferrarte a lo que ya cumplió su función en tu vida.", 
                        Mantra = "“Suelto con amor y agradezco todo lo aprendido hoy.”", 
                        Amuleto = "Lapislázuli o algo de color blanco puro.", 
                        MensajeMagico = "Estás liberando espacio para lo grande que viene. Confía.", 
                        TemaId = temaVibracion.Id 
                    }
                });
            }
            context.SaveChanges();
            // --- FRASES DE GRATITUD ---
            if (!context.FrasesGratitud.Any())
            {
                var frases = new List<FraseGratitud>
                {
                    new FraseGratitud { Texto = "La salud de mi familia y el pan en mi mesa.", Categoria = "SALUD" },
                    new FraseGratitud { Texto = "La oportunidad de aprender algo nuevo hoy.", Categoria = "FORTALEZA" },
                    new FraseGratitud { Texto = "La paz que siento al terminar este proyecto.", Categoria = "FORTALEZA" },
                    new FraseGratitud { Texto = "La claridad mental que guía mis decisiones.", Categoria = "PAZ INTERIOR" },
                    new FraseGratitud { Texto = "Los desafíos que pulen mi carácter.", Categoria = "AMOR PROPIO" },
                    new FraseGratitud { Texto = "La sincronicidad que me muestra el camino.", Categoria = "PAZ INTERIOR" }
                };
                context.FrasesGratitud.AddRange(frases);
                context.SaveChanges();
            }
            // --- RETOS SEMANALES ---
            if (!context.RetosSemanales.Any())
            {
                var temaMision = context.Temas.FirstOrDefault(t => t.Titulo == "Lección de Vida");
                int temaId = temaMision?.Id ?? 1;

                var retos = new List<RetoSemanal>
                {
                    new RetoSemanal 
                    { 
                        Titulo = "El Camino de la Paz", 
                        Descripcion = "Hoy respira profundo 3 veces cuando sientas estrés.",
                        Instrucciones = "Cierra los ojos, inhala en 4 tiempos, mantén 4 y exhala en 4. Hazlo tres veces seguidas.",
                        TemaId = temaId,
                        Activo = true,
                        EsGlobal = true,
                        FechaInicio = DateTime.Now
                    },
                    new RetoSemanal 
                    { 
                        Titulo = "Frecuencia de Alegría", 
                        Descripcion = "Escribe 3 cosas que te hicieron sonreír hoy.",
                        Instrucciones = "Al final del día, busca tres momentos pequeños (un café, una charla, un paisaje) y regístralos en tu bitácora.",
                        TemaId = temaId,
                        Activo = true,
                        EsGlobal = true,
                        FechaInicio = DateTime.Now
                    },
                    // --- RETOS PARA ESTADOS DE ANSIEDAD O ESTRÉS ---
                    new RetoSemanal 
                    { 
                        Titulo = "Anclaje de Tierra", 
                        Descripcion = "Identifica 5 cosas que puedes ver ahora mismo.",
                        Instrucciones = "Usa la técnica 5-4-3-2-1: Nombra 5 cosas que ves, 4 que tocas, 3 que oyes, 2 que hueles y 1 que saboreas para salir de tu mente y volver al presente.",
                        TemaId = temaId,
                        Activo = true,
                        EsGlobal = true,
                        FechaInicio = DateTime.Now
                    },

                    // --- RETOS PARA ESTADOS DE TRISTEZA O DESÁNIMO ---
                    new RetoSemanal 
                    { 
                        Titulo = "Luz de Gratitud", 
                        Descripcion = "Agradece algo que hoy das por sentado.",
                        Instrucciones = "Piensa en algo básico: el agua caliente, tus zapatos, o tu conexión a internet. Escribe por qué ese pequeño detalle hace tu vida más fácil hoy.",
                        TemaId = temaId,
                        Activo = true,
                        EsGlobal = true,
                        FechaInicio = DateTime.Now
                    },

                    // --- RETOS PARA EL ESTANCAMIENTO O BLOQUEO ---
                    new RetoSemanal 
                    { 
                        Titulo = "Movimiento Consciente", 
                        Descripcion = "Camina 10 minutos sin mirar el celular.",
                        Instrucciones = "Sal a caminar. El objetivo no es llegar a un sitio, sino sentir cómo tus pies tocan el suelo y cómo el aire entra en tus pulmones. Observa tu entorno en silencio.",
                        TemaId = temaId,
                        Activo = true,
                        EsGlobal = true,
                        FechaInicio = DateTime.Now
                    },

                    // --- RETOS PARA LA CONEXIÓN CON OTROS (ALMA) ---
                    new RetoSemanal 
                    { 
                        Titulo = "Vínculo de Oro", 
                        Descripcion = "Envía un mensaje de aprecio a alguien que no esperas.",
                        Instrucciones = "Elige a un amigo o familiar y dile: 'Me acordé de ti y quería decirte que valoro tu presencia'. No esperes nada a cambio, solo entrega esa frecuencia.",
                        TemaId = temaId,
                        Activo = true,
                        EsGlobal = true,
                        FechaInicio = DateTime.Now
                    },

                    // --- RETOS PARA EL AUTOCUIDADO (VIBRACIÓN ALTA) ---
                    new RetoSemanal 
                    { 
                        Titulo = "Silencio Sagrado", 
                        Descripcion = "Pasa 5 minutos en silencio total al despertar.",
                        Instrucciones = "Antes de tocar el celular o prender la TV, quédate sentado en tu cama. Solo observa tus pensamientos sin juzgarlos. Eres el observador, no el pensamiento.",
                        TemaId = temaId,
                        Activo = true,
                        EsGlobal = true,
                        FechaInicio = DateTime.Now
                    },
                    // --- PARA CUANDO SE SIENTE DESORIENTADO (FALTA DE NORTE) ---
                    new RetoSemanal 
                    { 
                        Titulo = "El Observador en el Faro", 
                        Descripcion = "Hoy no tomes decisiones importantes, solo observa.",
                        Instrucciones = "Cuando te sientas perdido, imagina que eres un faro. Las olas (problemas) vienen y van, pero el faro no se mueve. Dedica 5 minutos a ver tus dudas como si no fueran tuyas, solo como nubes pasando.",
                        TemaId = temaId,
                        Activo = true,
                        EsGlobal = true,
                        FechaInicio = DateTime.Now
                    },

                    // --- PARA CUANDO SIENTE QUE "ALGO FALTA" (VACÍO DEL ALMA) ---
                    new RetoSemanal 
                    { 
                        Titulo = "La Carta al Futuro", 
                        Descripcion = "Escribe una frase sobre quién quieres ser en un año.",
                        Instrucciones = "Esa sensación de que 'algo falta' es tu 'yo' del futuro llamándote. Escribe en tu bitácora una sola frase que empiece con: 'Estoy listo para recibir...', y deja que el universo trabaje el resto.",
                        TemaId = temaId,
                        Activo = true,
                        EsGlobal = true,
                        FechaInicio = DateTime.Now
                    },

                    // --- PARA CUANDO SIENTE QUE "HAY ALGO MÁS" (DESPERTAR MÍSTICO) ---
                    new RetoSemanal 
                    { 
                        Titulo = "Sincronicidad Visual", 
                        Descripcion = "Busca patrones en los números de hoy.",
                        Instrucciones = "Durante el día, presta atención a las matrículas, relojes o precios. Si ves números repetidos o tu número de vida, anótalo. Es la confirmación de que hay un orden mayor guiándote.",
                        TemaId = temaId,
                        Activo = true,
                        EsGlobal = true,
                        FechaInicio = DateTime.Now
                    },

                    // --- PARA EL ENOJO O FRUSTRACIÓN (TENSIÓN EN EL ISP/TRABAJO) ---
                    new RetoSemanal 
                    { 
                        Titulo = "El Escudo de Agua", 
                        Descripcion = "Visualiza el enojo resbalando por tu cuerpo.",
                        Instrucciones = "Si algo sale mal con los servidores o clientes, antes de reaccionar, bebe un vaso de agua lentamente. Imagina que el agua apaga el fuego interno y que las palabras de los demás no pueden mojarte.",
                        TemaId = temaId,
                        Activo = true,
                        EsGlobal = true,
                        FechaInicio = DateTime.Now
                    }
                };
                context.RetosSemanales.AddRange(retos);
                context.SaveChanges();
            }

            if (!context.SignosZodiacales.Any())
            {
                context.SignosZodiacales.AddRange(new List<SignoZodiacal>
                {
                    new SignoZodiacal { Nombre = "Aries", Icono = "♈", Elemento = "Fuego", DescripcionLarga = "Es el despertar de la vida. Representa la energía impulsiva, el liderazgo natural y el coraje para iniciar nuevos ciclos. Su fuerza es la acción pura.", PalabrasClave = "Iniciativa, Valor, Energía" },
                    new SignoZodiacal { Nombre = "Tauro", Icono = "♉", Elemento = "Tierra", DescripcionLarga = "La estabilidad hecha materia. Representa la perseverancia, el disfrute de los sentidos y la capacidad de dar forma y permanencia a los proyectos.", PalabrasClave = "Resistencia, Bienestar, Firmeza" },
                    new SignoZodiacal { Nombre = "Géminis", Icono = "♊", Elemento = "Aire", DescripcionLarga = "El puente de la comunicación. Representa la dualidad, el intelecto curioso y la capacidad de adaptar el pensamiento a múltiples realidades.", PalabrasClave = "Dualidad, Mente, Adaptabilidad" },
                    new SignoZodiacal { Nombre = "Cáncer", Icono = "♋", Elemento = "Agua", DescripcionLarga = "El refugio del alma. Rige las emociones profundas, la nutrición espiritual y los lazos con el origen y la familia. Su fuerza es la intuición.", PalabrasClave = "Emoción, Protección, Raíces" },
                    new SignoZodiacal { Nombre = "Leo", Icono = "♌", Elemento = "Fuego", DescripcionLarga = "El centro radiante. Representa la creatividad, la autoexpresión digna y el poder del corazón. Es el brillo soberano que inspira a otros.", PalabrasClave = "Brillo, Generosidad, Creatividad" },
                    new SignoZodiacal { Nombre = "Virgo", Icono = "♍", Elemento = "Tierra", DescripcionLarga = "La alquimia del detalle. Busca la perfección a través del servicio, el análisis y la purificación de la materia y el espíritu.", PalabrasClave = "Análisis, Orden, Servicio" },
                    new SignoZodiacal { Nombre = "Libra", Icono = "♎", Elemento = "Aire", DescripcionLarga = "La armonía del encuentro. Representa la justicia, la belleza en las relaciones y la búsqueda constante del equilibrio entre los opuestos.", PalabrasClave = "Equilibrio, Justicia, Unión" },
                    new SignoZodiacal { Nombre = "Escorpio", Icono = "♏", Elemento = "Agua", DescripcionLarga = "La transformación profunda. Rige los misterios, la muerte y el renacimiento. Su energía es magnética, intensa y regeneradora.", PalabrasClave = "Transmutación, Poder, Intensidad" },
                    new SignoZodiacal { Nombre = "Sagitario", Icono = "♐", Elemento = "Fuego", DescripcionLarga = "La flecha de la verdad. Representa la expansión de la conciencia, los viajes largos y la búsqueda filosófica del sentido de la vida.", PalabrasClave = "Expansión, Optimismo, Libertad" },
                    new SignoZodiacal { Nombre = "Capricornio", Icono = "♑", Elemento = "Tierra", DescripcionLarga = "La cima del logro. Representa la disciplina, el tiempo y la construcción de un legado sólido a través del esfuerzo y la maestría.", PalabrasClave = "Estructura, Ambición, Maestría" },
                    new SignoZodiacal { Nombre = "Acuario", Icono = "♒", Elemento = "Aire", DescripcionLarga = "El visionario del futuro. Representa la libertad individual, la innovación social y la conexión con la mente universal y colectiva.", PalabrasClave = "Innovación, Humanismo, Desapego" },
                    new SignoZodiacal { Nombre = "Piscis", Icono = "♓", Elemento = "Agua", DescripcionLarga = "El océano de la unidad. Representa la compasión universal, los sueños y la disolución del ego para conectar con el todo espiritual.", PalabrasClave = "Misticismo, Compasión, Sueños" }
                });
                //context.SignosZodiacales.AddRange(retos);
                context.SaveChanges();
            }

            if (!context.FasesLunares.Any())
            {
                context.FasesLunares.AddRange(new List<FaseLunar>
                {
                    new FaseLunar { Nombre = "Luna Nueva", Icono = "🌑", SignificadoEspiritual = "Fase de siembra. La energía está concentrada en el interior, ideal para plantar intenciones.", Recomendacion = "Escribe tus metas de este mes y medita en silencio." },
                    new FaseLunar { Nombre = "Luna Creciente", Icono = "🌒", SignificadoEspiritual = "Fase de impulso. Las semillas comienzan a brotar. Es tiempo de actuar y nutrir tus proyectos.", Recomendacion = "Toma las primeras acciones concretas para tus planes." },
                    new FaseLunar { Nombre = "Cuarto Creciente", Icono = "🌓", SignificadoEspiritual = "Fase de desafío. Momento de tomar decisiones y superar los primeros obstáculos.", Recomendacion = "No te rindas ante la presión; fortalece tu voluntad." },
                    new FaseLunar { Nombre = "Luna Gibosa", Icono = "🌔", SignificadoEspiritual = "Fase de perfeccionamiento. Momento de ajustar detalles antes de la gran culminación.", Recomendacion = "Analiza tus avances y corrige el rumbo si es necesario." },
                    new FaseLunar { Nombre = "Luna Llena", Icono = "🌕", SignificadoEspiritual = "Fase de plenitud. La luz máxima revela los resultados. Energía de expansión y celebración.", Recomendacion = "Agradece lo logrado y observa lo que sale a la luz." },
                    new FaseLunar { Nombre = "Luna Diseminante", Icono = "🌖", SignificadoEspiritual = "Fase de compartir. Tiempo de enseñar, comunicar y distribuir los frutos obtenidos.", Recomendacion = "Habla de tus experiencias y ayuda a otros con tu saber." },
                    new FaseLunar { Nombre = "Cuarto Menguante", Icono = "🌗", SignificadoEspiritual = "Fase de liberación. Momento de soltar lo que ya no sirve y realizar una limpieza interna.", Recomendacion = "Perdona, cierra ciclos y limpia tu espacio físico." },
                    new FaseLunar { Nombre = "Luna Balsámica", Icono = "🌘", SignificadoEspiritual = "Fase de descanso. El ciclo termina. Tiempo de introspección profunda y recuperación.", Recomendacion = "Duerme más, medita y prepárate para el nuevo inicio." }
                });
                context.SaveChanges();
            }
            if (!context.ElementosAstro.Any())
            {
                context.ElementosAstro.AddRange(new List<ElementoAstro>
                {
                    new ElementoAstro { Nombre = "Fuego", Icono = "🔥", Esencia = "La chispa de la vida", Descripcion = "Representa el entusiasmo, la voluntad, la pasión y el impulso creativo que inicia todo movimiento." },
                    new ElementoAstro { Nombre = "Tierra", Icono = "🌍", Esencia = "La solidez de la forma", Descripcion = "Representa la manifestación práctica, la paciencia, el mundo material y la capacidad de dar frutos constantes." },
                    new ElementoAstro { Nombre = "Aire", Icono = "💨", Esencia = "El aliento del pensamiento", Descripcion = "Representa la comunicación, el intelecto, las ideas sociales y la capacidad de observar la realidad objetivamente." },
                    new ElementoAstro { Nombre = "Agua", Icono = "💧", Esencia = "El flujo de la emoción", Descripcion = "Representa la sensibilidad profunda, la intuición, los sueños y la conexión psíquica con los demás." }
                });
                context.SaveChanges();
            }
            if (!context.Arcanos.Any()){
                var listaArcanos = new List<Arcano>
                {
                    new Arcano { Numero = 0, Nombre = "El Loco", LetraHebrea = "א (Aleph)", ElementoOPlaneta = "Aire", Mensaje = "El salto de fe hacia lo desconocido.", SignificadoEsoterico = "Potencial puro e inocencia espiritual.", ImagenUrl = "/img/arcanos/0_loco.png" },
                    new Arcano { Numero = 1, Nombre = "El Mago", LetraHebrea = "ב (Beth)", ElementoOPlaneta = "Mercurio", Mensaje = "Como es arriba, es abajo. Tienes el poder.", SignificadoEsoterico = "Manifestación y voluntad consciente.", ImagenUrl = "/img/arcanos/1_mago.png"  },
                    new Arcano { Numero = 2, Nombre = "La Sacerdotisa", LetraHebrea = "ג (Gimel)", ElementoOPlaneta = "Luna", Mensaje = "Escucha el silencio de tu intuición.", SignificadoEsoterico = "Sabiduría oculta y el subconsciente.", ImagenUrl = "/img/arcanos/2_sacerdotisa.png" },
                    new Arcano { Numero = 3, Nombre = "La Emperatriz", LetraHebrea = "ד (Daleth)", ElementoOPlaneta = "Venus", Mensaje = "La abundancia florece desde el amor.", SignificadoEsoterico = "Creatividad, fertilidad y naturaleza.", ImagenUrl = "/img/arcanos/3_emperatriz.png" },
                    new Arcano { Numero = 4, Nombre = "El Emperador", LetraHebrea = "ה (He)", ElementoOPlaneta = "Aries", Mensaje = "Estructura y autoridad con justicia.", SignificadoEsoterico = "Orden, estabilidad y dominio material.", ImagenUrl = "/img/arcanos/4_emperador.png" },
                    new Arcano { Numero = 5, Nombre = "El Hierofante", LetraHebrea = "ו (Vau)", ElementoOPlaneta = "Tauro", Mensaje = "El puente entre lo humano y lo divino.", SignificadoEsoterico = "Tradición, enseñanza y valores espirituales.", ImagenUrl = "/img/arcanos/5_hierofante.png" },
                    new Arcano { Numero = 6, Nombre = "Los Enamorados", LetraHebrea = "ז (Zain)", ElementoOPlaneta = "Géminis", Mensaje = "La unión de los opuestos en armonía.", SignificadoEsoterico = "Elecciones del corazón y dualidad.", ImagenUrl = "/img/arcanos/6_enamorados.png" },
                    new Arcano { Numero = 7, Nombre = "El Carro", LetraHebrea = "ח (Cheth)", ElementoOPlaneta = "Cáncer", Mensaje = "Victoria a través de la determinación.", SignificadoEsoterico = "Control, avance y triunfo sobre obstáculos.", ImagenUrl = "/img/arcanos/7_carro.png" },
                    new Arcano { Numero = 8, Nombre = "La Justicia", LetraHebrea = "ל (Lamed)", ElementoOPlaneta = "Libra", Mensaje = "Cosechas exactamente lo que has sembrado.", SignificadoEsoterico = "Equilibrio kármico y verdad objetiva.", ImagenUrl = "/img/arcanos/8_justicia.png"  },
                    new Arcano { Numero = 9, Nombre = "El Ermitaño", LetraHebrea = "י (Yod)", ElementoOPlaneta = "Virgo", Mensaje = "La luz que buscas está en tu interior.", SignificadoEsoterico = "Introspección, guía y soledad sagrada.", ImagenUrl = "/img/arcanos/9_ermitano.png" },
                    new Arcano { Numero = 10, Nombre = "La Rueda de la Fortuna", LetraHebrea = "כ (Kaph)", ElementoOPlaneta = "Júpiter", Mensaje = "Todo cambia, el ciclo nunca se detiene.", SignificadoEsoterico = "Destino, ciclos y giros inesperados.", ImagenUrl = "/img/arcanos/10_rueda_fortuna.png" },
                    new Arcano { Numero = 11, Nombre = "La Fuerza", LetraHebrea = "ט (Teth)", ElementoOPlaneta = "Leo", Mensaje = "El dominio de la fiera interna con amor.", SignificadoEsoterico = "Coraje espiritual y compasión.", ImagenUrl = "/img/arcanos/11_fuerza.png" },
                    new Arcano { Numero = 12, Nombre = "El Colgado", LetraHebrea = "מ (Mem)", ElementoOPlaneta = "Agua", Mensaje = "Ver el mundo desde una nueva perspectiva.", SignificadoEsoterico = "Sacrificio, pausa e iluminación.", ImagenUrl = "/img/arcanos/12_colgado.png" },
                    new Arcano { Numero = 13, Nombre = "La Muerte", LetraHebrea = "נ (Nun)", ElementoOPlaneta = "Escorpio", Mensaje = "Lo viejo muere para que nazca lo nuevo.", SignificadoEsoterico = "Transformación profunda y renacimiento.", ImagenUrl = "/img/arcanos/13_muerte.png" },
                    new Arcano { Numero = 14, Nombre = "La Templanza", LetraHebrea = "ס (Samekh)", ElementoOPlaneta = "Sagitario", Mensaje = "Alquimia emocional y equilibrio.", SignificadoEsoterico = "Moderación, paciencia y propósito.", ImagenUrl = "/img/arcanos/14_templanza.png" },
                    new Arcano { Numero = 15, Nombre = "El Diablo", LetraHebrea = "ע (Ayin)", ElementoOPlaneta = "Capricornio", Mensaje = "Libérate de las cadenas que tú mismo creaste.", SignificadoEsoterico = "Sombra, apegos y tentación material." , ImagenUrl = "/img/arcanos/15_diablo.png" },
                    new Arcano { Numero = 16, Nombre = "La Torre", LetraHebrea = "Pe", ElementoOPlaneta = "Marte", Mensaje = "El derrumbe de lo falso para construir verdad.", SignificadoEsoterico = "Revelación súbita y liberación forzada.", ImagenUrl = "/img/arcanos/16_torre.png" },
                    new Arcano { Numero = 17, Nombre = "La Estrella", LetraHebrea = "Tzaddi", ElementoOPlaneta = "Acuario", Mensaje = "La esperanza es la guía en la oscuridad.", SignificadoEsoterico = "Inspiración, fe y sanación.", ImagenUrl = "/img/arcanos/17_estrella.png" },
                    new Arcano { Numero = 18, Nombre = "La Luna", LetraHebrea = "Qoph", ElementoOPlaneta = "Piscis", Mensaje = "Navega tus miedos y confía en tus sueños.", SignificadoEsoterico = "Ilusión, misterio y el inconsciente profundo.", ImagenUrl = "/img/arcanos/18_luna.png" },
                    new Arcano { Numero = 19, Nombre = "El Sol", LetraHebrea = "ר (Resh)", ElementoOPlaneta = "Sol", Mensaje = "Éxito, alegría y claridad absoluta.", SignificadoEsoterico = "Vitalidad, éxito y realización.", ImagenUrl = "/img/arcanos/19_sol.png" },
                    new Arcano { Numero = 20, Nombre = "El Juicio", LetraHebrea = "ש (Shin)", ElementoOPlaneta = "Fuego", Mensaje = "El despertar a una nueva vida superior.", SignificadoEsoterico = "Llamado interno, perdón y absolución.", ImagenUrl = "/img/arcanos/20_juicio.png" },
                    new Arcano { Numero = 21, Nombre = "El Mundo", LetraHebrea = "ת (Tau)", ElementoOPlaneta = "Saturno", Mensaje = "La culminación del viaje, el todo en uno.", SignificadoEsoterico = "Finalización, integración y unidad.", ImagenUrl = "/img/arcanos/21_mundo.png" },
                    new Arcano { Numero = 22, Nombre = "El Loco", LetraHebrea = "א (Aleph)", ElementoOPlaneta = "Aire", Mensaje = "El salto de fe hacia lo desconocido.", SignificadoEsoterico = "Potencial puro e inocencia espiritual.", ImagenUrl = "/img/arcanos/0_loco.png" }
                };

                context.Arcanos.AddRange(listaArcanos);
                context.SaveChanges();
            }

            // --- INICIALIZADORES PARA LA TABLA ARQUETIPOS ---
            if (!context.Arquetipos.Any()){
                        var arquetipos = new List<Arquetipo>
                        {
                            new Arquetipo { 
                                Numero = 1, Nombre = "Independencia", LetraHebrea = "א (Alef)", 
                                ElementoOPlaneta = "Fuego / Sol", 
                                Mensaje = "Potencialidad pura y liderazgo.", 
                                SignificadoEsoterico = "Representa el inicio, la autosuficiencia, el ser anfitrión y la unidad. Es el potencial absoluto antes de manifestarse.", 
                                ImagenUrl = "/img/arquetipos/arquetipo1.jpg" 
                            },
                            new Arquetipo { 
                                Numero = 2, Nombre = "Dualidad", LetraHebrea = "ב (Bet)", 
                                ElementoOPlaneta = "Luna", 
                                Mensaje = "El espejo, la pareja y la recepción.", 
                                SignificadoEsoterico = "Simboliza la intuición, el secreto y el reconocimiento en el otro. Es el útero que recibe y la dependencia necesaria para el equilibrio.", 
                                ImagenUrl = "/img/arquetipos/arquetipo2.jpg" 
                            },
                            new Arquetipo { 
                                Numero = 3, Nombre = "Intelecto", LetraHebrea = "ג (Guímel)", 
                                ElementoOPlaneta = "Mercurio", 
                                Mensaje = "Creación mental y comunicación.", 
                                SignificadoEsoterico = "Representa al Artífice, el padre ideológico y el conocimiento racional. Es la mente, la cabeza y la capacidad de explicar el mundo.", 
                                ImagenUrl = "/img/arquetipos/arquetipo3.jpg" 
                            },
                            new Arquetipo { 
                                Numero = 4, Nombre = "Estructura", LetraHebrea = "ד (Dálet)", 
                                ElementoOPlaneta = "Tierra", 
                                Mensaje = "Solidez, cimientos y raíces.", 
                                SignificadoEsoterico = "Simboliza el mundo físico, la casa y el cuerpo. Es la precisión, la organización y el útero que da forma a la realidad tangible.", 
                                ImagenUrl = "/img/arquetipos/arquetipo4.jpg" 
                            },
                            new Arquetipo { 
                                Numero = 5, Nombre = "Coherencia", LetraHebrea = "ה (He)", 
                                ElementoOPlaneta = "Hierro / Marte", 
                                Mensaje = "Liderazgo ético y marco de vida.", 
                                SignificadoEsoterico = "Representa al director de equipo que une pensar, hablar y hacer. Es la moral, el orden y el elemento humano o quinto elemento.", 
                                ImagenUrl = "/img/arquetipos/arquetipo5.jpg" 
                            },
                            new Arquetipo { 
                                Numero = 6, Nombre = "Conexión", LetraHebrea = "ו (Vav)", 
                                ElementoOPlaneta = "Venus", 
                                Mensaje = "Inocencia, juego y creación.", 
                                SignificadoEsoterico = "Simboliza la niñez, el puente de comunicación y el remate de las ideas. Representa la sensualidad y la alegría de ser humano.", 
                                ImagenUrl = "/img/arquetipos/arquetipo6.jpg" 
                            },
                            new Arquetipo { 
                                Numero = 7, Nombre = "Acción", LetraHebrea = "ז (Zayin)", 
                                ElementoOPlaneta = "Marte", 
                                Mensaje = "Control, conquista y determinación.", 
                                SignificadoEsoterico = "Representa el triunfo, la toma de decisiones y la capacidad de colonizar. Es la energía que fecunda y avanza hacia objetivos claros.", 
                                ImagenUrl = "/img/arquetipos/arquetipo7.jpg" 
                            },
                            new Arquetipo { 
                                Numero = 8, Nombre = "Sabiduría", LetraHebrea = "ח (Jet)", 
                                ElementoOPlaneta = "Saturno", 
                                Mensaje = "Poder, justicia y planificación.", 
                                SignificadoEsoterico = "Simboliza la ley, el territorio y la estrategia. Es la sabiduría aplicada a la familia, el dinero y el equilibrio de la sangre.", 
                                ImagenUrl = "/img/arquetipos/arquetipo8.jpg" 
                            },
                            new Arquetipo { 
                                Numero = 9, Nombre = "Buscador", LetraHebrea = "ט (Tet)", 
                                ElementoOPlaneta = "Neptuno", 
                                Mensaje = "Experimentación e iluminación interna.", 
                                SignificadoEsoterico = "Representa al guía y orientador. Es la sabiduría del solitario, la conexión con los números y la búsqueda de soluciones profundas.", 
                                ImagenUrl = "/img/arquetipos/arquetipo9.jpg" 
                            },
                            new Arquetipo { 
                                Numero = 10, Nombre = "Oportunidad", LetraHebrea = "י (Yod)", 
                                ElementoOPlaneta = "Júpiter", 
                                Mensaje = "Cambio, suerte y abundancia.", 
                                SignificadoEsoterico = "Simboliza el movimiento y el sentido de unidad con lo superior a través del Árbol de la Vida. Representa la riqueza y estabilidad divina.", 
                                ImagenUrl = "/img/arquetipos/arquetipo10.jpg" 
                            },
                            new Arquetipo { 
                                Numero = 11, Nombre = "Fuerza", LetraHebrea = "כ (Kaf)", 
                                ElementoOPlaneta = "Sol", 
                                Mensaje = "Coraje, marca y autoconfianza.", 
                                SignificadoEsoterico = "Representa el principio de la secuencia creadora material. Es la energía de la marca y la fuerza necesaria para sostener la realidad.", 
                                ImagenUrl = "/img/arquetipos/arquetipo11.jpg" 
                            },
                            new Arquetipo { 
                                Numero = 12, Nombre = "El Colgado", LetraHebrea = "Lamed", 
                                ElementoOPlaneta = "Neptuno / Agua",
                                Mensaje = "Es tiempo de detenerse y mirar desde otra perspectiva. El sacrificio de hoy es la sabiduría de mañana.",
                                SignificadoEsoterico = "Representa la pausa necesaria antes de un gran cambio espiritual. No es una derrota, es una elección de ver la verdad.",
                                ImagenUrl = "/images/arquetipos/12.jpg"
                            },
                            new Arquetipo { 
                                Numero = 13, Nombre = "Transformación", LetraHebrea = "מ (Mem)", 
                                ElementoOPlaneta = "Agua", 
                                Mensaje = "Amor, unidad y cambio de estado.", 
                                SignificadoEsoterico = "Simboliza morir antes de morir para renacer. Es la escalera hacia la espiritualidad y el cambio profundo de la conciencia.", 
                                ImagenUrl = "/img/arquetipos/arquetipo13.jpg" 
                            },
                            new Arquetipo { 
                                Numero = 14, Nombre = "Suficiencia", LetraHebrea = "נ (Nun)", 
                                ElementoOPlaneta = "Aire", 
                                Mensaje = "Liberación del alma y abundancia.", 
                                SignificadoEsoterico = "Representa la oración, el soltar y el cerrar acuerdos. Es la capacidad de usar la tecnología y comunicación para la libertad espiritual.", 
                                ImagenUrl = "/img/arquetipos/arquetipo14.jpg" 
                            },
                            new Arquetipo { 
                                Numero = 15, Nombre = "El Imán", LetraHebrea = "Sámej (ס)", 
                                ElementoOPlaneta = "Fuego", 
                                Mensaje = "Magnetismo, pasión y sombra.", 
                                SignificadoEsoterico = "Representa el poder de atracción, el carisma y los deseos profundos. Es la fuerza que nos vincula a la materia, la sexualidad y la capacidad de seducción para materializar.", 
                                ImagenUrl = "/img/arquetipos/arquetipo15.jpg" 
                            },
                            new Arquetipo { 
                                Numero = 16, Nombre = "La Ruptura", LetraHebrea = "Ayin (ע)", 
                                ElementoOPlaneta = "Marte / Urano", 
                                Mensaje = "Liberación y caída de estructuras.", 
                                SignificadoEsoterico = "Simboliza la eliminación de lo que ya no sirve. Es la torre que se rompe para liberar la verdad, permitiendo que la energía estancada fluya de nuevo hacia la construcción de algo real.", 
                                ImagenUrl = "/img/arquetipos/arquetipo16.jpg" 
                            },
                            new Arquetipo { 
                                Numero = 17, Nombre = "La Esperanza", LetraHebrea = "Pe (פ)", 
                                ElementoOPlaneta = "Aire / Acuario", 
                                Mensaje = "Inspiración, fe y guía estelar.", 
                                SignificadoEsoterico = "Representa la luz en el camino y la conexión con el propósito divino. Es el arquetipo de la transparencia, la sanación y la confianza en que el universo provee lo necesario.", 
                                ImagenUrl = "/img/arquetipos/arquetipo17.jpg" 
                            },
                            new Arquetipo { 
                                Numero = 18, Nombre = "Tradición", LetraHebrea = "צ (Tsadi)", 
                                ElementoOPlaneta = "Luna", 
                                Mensaje = "Pasado, imaginación y raíces familiares.", 
                                SignificadoEsoterico = "Representa el origen, los sueños y los registros akáshicos. Es la investigación de los secretos del inconsciente familiar.", 
                                ImagenUrl = "/img/arquetipos/arquetipo18.jpg" 
                            },
                            new Arquetipo { 
                                Numero = 19, Nombre = "Claridad", LetraHebrea = "ק (Kof)", 
                                ElementoOPlaneta = "Sol", 
                                Mensaje = "Éxito, armonía y felicidad.", 
                                SignificadoEsoterico = "Representa la autoridad moral, la paz y la confianza absoluta. Es el Sol que brinda abundancia y luz al propósito de vida.", 
                                ImagenUrl = "/img/arquetipos/arquetipo19.jpg" 
                            },
                            new Arquetipo { 
                                Numero = 20, Nombre = "El Despertar", LetraHebrea = "Resh (ר)", 
                                ElementoOPlaneta = "Fuego / Saturno", 
                                Mensaje = "Renovación y llamado de la conciencia.", 
                                SignificadoEsoterico = "Representa la resurrección de ideas, el juicio constructivo y la liberación de patrones antiguos. Es el despertar a una nueva realidad espiritual.", 
                                ImagenUrl = "/img/arquetipos/arquetipo20.jpg" 
                            },
                            new Arquetipo { 
                                Numero = 21, Nombre = "Visión Global", LetraHebrea = "Shin (ש)", 
                                ElementoOPlaneta = "Totalidad", 
                                Mensaje = "Logro, integración y armonía global.", 
                                SignificadoEsoterico = "Simboliza la totalidad, el éxito y la belleza. Es la capacidad de ver el mundo como un todo integrado, logrando la plenitud y la valentía creativa.", 
                                ImagenUrl = "/img/arquetipos/arquetipo21.jpg" 
                            },
                            new Arquetipo { 
                                Numero = 22, Nombre = "Trascendencia", LetraHebrea = "Tav (ת)", 
                                ElementoOPlaneta = "Plutón", 
                                Mensaje = "Genio, originalidad y grandes retos.", 
                                SignificadoEsoterico = "Representa la capacidad de traspasar límites y la autoconfianza absoluta. Es el arquetipo del genio que manifiesta grandes ideas y proyectos fuera de lo común.", 
                                ImagenUrl = "/img/arquetipos/arquetipo22.jpg" 
                            },
                            new Arquetipo { 
                                Numero = 26, Nombre = "Gestión de Crisis", LetraHebrea = "Yod-Vav (יה)", 
                                ElementoOPlaneta = "Aspectos del 8", 
                                Mensaje = "Intervención especial y milagros.", 
                                SignificadoEsoterico = "Este es un arquetipo de puestos especiales. Simboliza la capacidad de 'bajar al fango' en situaciones difíciles y realizar una función mesiánica o transformadora.", 
                                ImagenUrl = "/img/arquetipos/arquetipo26.jpg" 
                            },
                            new Arquetipo { 
                                Numero = 33, Nombre = "Amor Universal", LetraHebrea = "Lamed-Alef (לא)", 
                                ElementoOPlaneta = "Aspectos del 6", 
                                Mensaje = "Entrega incondicional y servicio.", 
                                SignificadoEsoterico = "Representa el grado más alto de conexión humana. Es la guía desde el amor puro, la sanación a través de la risa y el remate perfecto de la creación humana.", 
                                ImagenUrl = "/img/arquetipos/arquetipo33.jpg" 
                            }
                        };
                        context.Arquetipos.AddRange(arquetipos);
                        context.SaveChanges();
            }
            // Guardar todos los cambios finales
            // context.SaveChanges();
        }
    }
}