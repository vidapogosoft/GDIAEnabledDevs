using Microsoft.EntityFrameworkCore;
using Guia.API.Models;

namespace Guia.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Aquí le decimos qué clases se vuelven tablas en SQL
        // Tablas principales del sistema
        public DbSet<Tema> Temas { get; set; }
        public DbSet<Significado> Significados { get; set; }

        // Nuevas tablas de usuario
        public DbSet<Persona> Personas { get; set; }
        public DbSet<PersonaDetalle> PersonasDetalles { get; set; }
        public DbSet<PersonaNumerologia> PersonasNumerologia { get; set; }
        public DbSet<FraseGratitud> FrasesGratitud { get; set; }
        public DbSet<RetoSemanal> RetosSemanales { get; set; }
        public DbSet<Bitacora> Bitacoras { get; set; }
        public DbSet<SignoZodiacal> SignosZodiacales { get; set; }
        public DbSet<FaseLunar> FasesLunares { get; set; }
        public DbSet<ElementoAstro> ElementosAstro { get; set; }
        public DbSet<Arcano> Arcanos { get; set; }
        public DbSet<ArbolVida> ArbolesVida { get; set; }
        public DbSet<Arquetipo> Arquetipos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuramos que PersonaId sea único para mantener la relación 1 a 1
            modelBuilder.Entity<PersonaDetalle>()
                .HasIndex(d => d.PersonaId)
                .IsUnique();

            modelBuilder.Entity<PersonaNumerologia>()
                .HasIndex(n => n.PersonaId)
                .IsUnique();
        }
    }
}