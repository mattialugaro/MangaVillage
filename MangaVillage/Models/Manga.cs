namespace MangaVillage
{
    using MangaVillage.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Manga")]
    public partial class Manga
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Manga()
        {
            Foto = new HashSet<Foto>();
            Recensione = new HashSet<Recensione>();
            //Generi = new HashSet<Genere>();
            //Categorie = new HashSet<Categoria>();
        }

        [ScaffoldColumn(false)]
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Titolo { get; set; }

        [Required]
        [StringLength(100)]
        public string Autore { get; set; }

        [Display(Name = "Anno di pubblicazione")]
        [StringLength(10)]
        public string AnnoUscita { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nazionalità")]
        public string Nazionalita { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Stato di pubblicazione")]
        public string StatoPubblicazione { get; set; }

        [Required]
        [StringLength(50)]
        public string Copertina { get; set; }

        [Required]
        public string Trama { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Foto> Foto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recensione> Recensione { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Genere> Genere { get; set; } = new List<Genere>();

        [NotMapped]
        public virtual ICollection<Genere> GenereTendina { get; set; }

        [NotMapped]
        [Display(Name = "Genere")]
        public string GenereString {  get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Categoria> Categoria { get; set; } = new List<Categoria>();

        [NotMapped]
        public virtual ICollection<Categoria> CategoriaTendina { get; set; }

        [NotMapped]
        [Display(Name = "Categoria")]
        public string CategoriaString { get; set; }


    }
}
