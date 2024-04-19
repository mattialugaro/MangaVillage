namespace MangaVillage
{
    using MangaVillage;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DettaglioOrdine")]
    public partial class DettaglioOrdine
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDOrdineFk { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDMangaFk { get; set; }

        [Display(Name = "Volume")]
        public int NumeroVolume { get; set; }

        [Display(Name = "Quantità")]
        public int Quantita { get; set; }

        public virtual Manga Manga { get; set; }

        public virtual Ordine Ordine { get; set; }

        public bool Equals(DettaglioOrdine dettaglioOrdine)
        {
            return dettaglioOrdine.IDMangaFk == this.IDMangaFk && dettaglioOrdine.NumeroVolume == this.NumeroVolume;
        }
        
    }
}
