using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guia.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialSqlServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Arcanos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LetraHebrea = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SignificadoEsoterico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mensaje = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ElementoOPlaneta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagenUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arcanos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Arquetipos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LetraHebrea = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SignificadoEsoterico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mensaje = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ElementoOPlaneta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagenUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arquetipos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ElementosAstro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Esencia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementosAstro", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FasesLunares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SignificadoEspiritual = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Recomendacion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FasesLunares", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FrasesGratitud",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrasesGratitud", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Personas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombres = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SignosZodiacales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Elemento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescripcionLarga = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PalabrasClave = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignosZodiacales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Temas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescripcionGeneral = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstaActivo = table.Column<bool>(type: "bit", nullable: false),
                    EsGratis = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Temas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArbolesVida",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonaId = table.Column<int>(type: "int", nullable: false),
                    Kether_Valor = table.Column<int>(type: "int", nullable: false),
                    Kether_Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Chokmah_Valor = table.Column<int>(type: "int", nullable: false),
                    Chokmah_Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Binah_Valor = table.Column<int>(type: "int", nullable: false),
                    Binah_Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Chesed_Valor = table.Column<int>(type: "int", nullable: false),
                    Chesed_Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gevurah_Valor = table.Column<int>(type: "int", nullable: false),
                    Gevurah_Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tiferet_Valor = table.Column<int>(type: "int", nullable: false),
                    Tiferet_Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Netzach_Valor = table.Column<int>(type: "int", nullable: false),
                    Netzach_Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hod_Valor = table.Column<int>(type: "int", nullable: false),
                    Hod_Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Yesod_Valor = table.Column<int>(type: "int", nullable: false),
                    Yesod_Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Malchut_Valor = table.Column<int>(type: "int", nullable: false),
                    Malchut_Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sendero_1_2 = table.Column<int>(type: "int", nullable: false),
                    Sendero_1_3 = table.Column<int>(type: "int", nullable: false),
                    Sendero_1_6 = table.Column<int>(type: "int", nullable: false),
                    Sendero_2_3 = table.Column<int>(type: "int", nullable: false),
                    Sendero_2_4 = table.Column<int>(type: "int", nullable: false),
                    Sendero_2_6 = table.Column<int>(type: "int", nullable: false),
                    Sendero_3_5 = table.Column<int>(type: "int", nullable: false),
                    Sendero_3_6 = table.Column<int>(type: "int", nullable: false),
                    Sendero_4_5 = table.Column<int>(type: "int", nullable: false),
                    Sendero_4_6 = table.Column<int>(type: "int", nullable: false),
                    Sendero_4_7 = table.Column<int>(type: "int", nullable: false),
                    Sendero_5_6 = table.Column<int>(type: "int", nullable: false),
                    Sendero_5_8 = table.Column<int>(type: "int", nullable: false),
                    Sendero_6_7 = table.Column<int>(type: "int", nullable: false),
                    Sendero_6_8 = table.Column<int>(type: "int", nullable: false),
                    Sendero_6_9 = table.Column<int>(type: "int", nullable: false),
                    Sendero_7_8 = table.Column<int>(type: "int", nullable: false),
                    Sendero_7_9 = table.Column<int>(type: "int", nullable: false),
                    Sendero_7_10 = table.Column<int>(type: "int", nullable: false),
                    Sendero_8_9 = table.Column<int>(type: "int", nullable: false),
                    Sendero_8_10 = table.Column<int>(type: "int", nullable: false),
                    Sendero_9_10 = table.Column<int>(type: "int", nullable: false),
                    FechaCalculo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArbolesVida", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArbolesVida_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonasDetalles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonaId = table.Column<int>(type: "int", nullable: false),
                    SignoZodiaco = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FaseLunar = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Elemento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VibracionDia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MensajeGratitud = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotasAstrologicas = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonasDetalles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonasDetalles_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonasNumerologia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonaId = table.Column<int>(type: "int", nullable: false),
                    MisionVida = table.Column<int>(type: "int", nullable: false),
                    NumeroAlma = table.Column<int>(type: "int", nullable: true),
                    NumeroPersonalidad = table.Column<int>(type: "int", nullable: false),
                    NumeroDestino = table.Column<int>(type: "int", nullable: false),
                    LeccionVida = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonasNumerologia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonasNumerologia_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RetosSemanales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instrucciones = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroAsociado = table.Column<int>(type: "int", nullable: true),
                    TemaId = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    EsGlobal = table.Column<bool>(type: "bit", nullable: false),
                    Dificultad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RetosSemanales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RetosSemanales_Temas_TemaId",
                        column: x => x.TemaId,
                        principalTable: "Temas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Significados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ValorNumero = table.Column<int>(type: "int", nullable: false),
                    Apodo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mision = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mantra = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amuleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MensajeMagico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Significados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Significados_Temas_TemaId",
                        column: x => x.TemaId,
                        principalTable: "Temas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bitacoras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonaId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contenido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValorSincronico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstadoReto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RetoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bitacoras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bitacoras_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bitacoras_RetosSemanales_RetoId",
                        column: x => x.RetoId,
                        principalTable: "RetosSemanales",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArbolesVida_PersonaId",
                table: "ArbolesVida",
                column: "PersonaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bitacoras_PersonaId",
                table: "Bitacoras",
                column: "PersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_Bitacoras_RetoId",
                table: "Bitacoras",
                column: "RetoId");

            migrationBuilder.CreateIndex(
                name: "IX_Personas_Email",
                table: "Personas",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Personas_Username",
                table: "Personas",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonasDetalles_PersonaId",
                table: "PersonasDetalles",
                column: "PersonaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonasNumerologia_PersonaId",
                table: "PersonasNumerologia",
                column: "PersonaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RetosSemanales_TemaId",
                table: "RetosSemanales",
                column: "TemaId");

            migrationBuilder.CreateIndex(
                name: "IX_Significados_TemaId",
                table: "Significados",
                column: "TemaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArbolesVida");

            migrationBuilder.DropTable(
                name: "Arcanos");

            migrationBuilder.DropTable(
                name: "Arquetipos");

            migrationBuilder.DropTable(
                name: "Bitacoras");

            migrationBuilder.DropTable(
                name: "ElementosAstro");

            migrationBuilder.DropTable(
                name: "FasesLunares");

            migrationBuilder.DropTable(
                name: "FrasesGratitud");

            migrationBuilder.DropTable(
                name: "PersonasDetalles");

            migrationBuilder.DropTable(
                name: "PersonasNumerologia");

            migrationBuilder.DropTable(
                name: "Significados");

            migrationBuilder.DropTable(
                name: "SignosZodiacales");

            migrationBuilder.DropTable(
                name: "RetosSemanales");

            migrationBuilder.DropTable(
                name: "Personas");

            migrationBuilder.DropTable(
                name: "Temas");
        }
    }
}
