namespace MangaVillage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Genere")]
    public partial class Genere
    {
        [ScaffoldColumn(false)]
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string Nome { get; set; }
    }
}
