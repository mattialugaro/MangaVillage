namespace MangaVillage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Categoria")]
    public partial class Categoria
    {
        [ScaffoldColumn(false)]
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string Nome { get; set; }
    }
}
