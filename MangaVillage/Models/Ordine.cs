namespace MangaVillage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ordine")]
    public partial class Ordine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ordine()
        {
            DettaglioOrdine = new HashSet<DettaglioOrdine>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        public string IndirizzoConsegna { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataOrdine { get; set; }

        public string Note { get; set; }

        public bool Pagato { get; set; }

        public int IDUtenteFk { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DettaglioOrdine> DettaglioOrdine { get; set; }

        [NotMapped]
        public decimal TotaleCarrello { get; set; }
    }
}
