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

        public int NumeroVolume { get; set; }

        public int Quantita { get; set; }

        public virtual Manga Manga { get; set; }

        public virtual Ordine Ordine { get; set; }
    }
}
