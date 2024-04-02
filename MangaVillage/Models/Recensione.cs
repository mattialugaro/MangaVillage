namespace MangaVillage.Models
{
    using MangaVillage;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Recensione")]
    public partial class Recensione
    {
        public int ID { get; set; }

        [Required]
        public decimal Voto { get; set; }

        [Required]
        public string Descrizione { get; set; }

        [Required]
        [Display(Name = "Titolo Manga")]
        public int IDMangaFk { get; set; }

        [Required]
        [Display(Name = "Autore Recensione")]
        public int IDUtenteFk { get; set; }

        public virtual Manga Manga { get; set; }

        public virtual Utente Utente { get; set; }
    }
}
