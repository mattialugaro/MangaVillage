﻿namespace MangaVillage
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
        [Display(Name = "Indirizzo di consegna")]
        public string IndirizzoConsegna { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Data dell' ordine")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.DateTime)]
        public DateTime DataOrdine { get; set; }

        public string Note { get; set; }

        public bool Pagato { get; set; }

        [Display(Name = "Username utente")]
        public int IDUtenteFk { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DettaglioOrdine> DettaglioOrdine { get; set; }

        public virtual Utente Utente { get; set; }

        [NotMapped]
        public decimal TotaleCarrello { get; set; }
    }
}
