using MangaVillage.Models;
using MangaVillage;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace MangaVillage.Models
{
    public partial class ModelDbContext : DbContext
    {
        public ModelDbContext()
            : base("name=ModelDbContext")
        {
        }

        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<Foto> Foto { get; set; }
        public virtual DbSet<Genere> Genere { get; set; }
        public virtual DbSet<Manga> Manga { get; set; }
        public virtual DbSet<Recensione> Recensione { get; set; }
        public virtual DbSet<Utente> Utente { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>()
                .HasMany(e => e.Manga)
                .WithMany(e => e.Categoria)
                .Map(m => m.ToTable("Manga_Categoria").MapLeftKey("IDCategoriaFk").MapRightKey("IDMangaFk"));

            modelBuilder.Entity<Genere>()
                .HasMany(e => e.Manga)
                .WithMany(e => e.Genere)
                .Map(m => m.ToTable("Manga_Genere").MapLeftKey("IDGenereFk").MapRightKey("IDMangaFk"));

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

            modelBuilder.Entity<Utente>()
                .HasMany(e => e.Recensione)
                .WithRequired(e => e.Utente)
                .HasForeignKey(e => e.IDUtenteFk)
                .WillCascadeOnDelete(false);
        }
    }
}
