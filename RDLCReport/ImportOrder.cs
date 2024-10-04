namespace RDLCReport
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ImportOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ImportOrder()
        {
            ImportOrderItems = new HashSet<ImportOrderItem>();
        }

        public int id { get; set; }

        public int? supplier_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime orderDate { get; set; }

        public bool complete { get; set; }

        public decimal orderTotal { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }

        public int? seller_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImportOrderItem> ImportOrderItems { get; set; }

        public virtual User User { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
