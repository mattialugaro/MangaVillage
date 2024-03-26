namespace MangaVillage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Foto")]
    public partial class Foto
    {
        [ScaffoldColumn(false)]
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; }

        public int IDMangaFk { get; set; }

        public virtual Manga Manga { get; set; }
    }
}
