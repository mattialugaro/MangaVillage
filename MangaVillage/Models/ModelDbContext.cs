using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace MangaVillage
{
    public partial class ModelDbContext : DbContext
    {
        public ModelDbContext()
            : base("name=ModelDbContext")
        {
        }

        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<Foto> Foto { get; set; }
        public virtual DbSet<Foto_Recensione> Foto_Recensione { get; set; }
        public virtual DbSet<Genere> Genere { get; set; }
        public virtual DbSet<Manga> Manga { get; set; }
        public virtual DbSet<Recensione> Recensione { get; set; }
        public virtual DbSet<Utente> Utente { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manga>()
                .HasMany(e => e.Foto)
                .WithRequired(e => e.Manga)
                .HasForeignKey(e => e.IDMangaFk)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Manga>()
                .HasMany(e => e.Recensione)
                .WithRequired(e => e.Manga)
                .HasForeignKey(e => e.IDMangaFk)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Recensione>()
                .Property(e => e.Voto)
                .HasPrecision(18, 1);

            modelBuilder.Entity<Recensione>()
                .HasMany(e => e.Foto_Recensione)
                .WithRequired(e => e.Recensione)
                .HasForeignKey(e => e.IDRecensioneFk)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Utente>()
                .HasMany(e => e.Recensione)
                .WithRequired(e => e.Utente)
                .HasForeignKey(e => e.IDUtenteFk)
                .WillCascadeOnDelete(false);
        }
    }
}
