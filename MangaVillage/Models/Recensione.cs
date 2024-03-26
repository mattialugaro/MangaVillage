namespace MangaVillage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Recensione")]
    public partial class Recensione
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Recensione()
        {
            Foto_Recensione = new HashSet<Foto_Recensione>();
        }
        [ScaffoldColumn(false)]
        [Key]
        public int ID { get; set; }

        [Required]
        public string Descrizione { get; set; }

        public decimal Voto { get; set; }
       
        public int IDMangaFk { get; set; }

        public int IDUtenteFk { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Foto_Recensione> Foto_Recensione { get; set; }

        public virtual Manga Manga { get; set; }

        public virtual Utente Utente { get; set; }
    }
}
