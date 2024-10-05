namespace RDLCReport
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ExportOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ExportOrder()
        {
            ExportOrderItems = new HashSet<ExportOrderItem>();
            ReportsExportOrdersMappings = new HashSet<ReportsExportOrdersMapping>();
        }

        public int id { get; set; }

        public int? customer_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime orderDate { get; set; }

        public bool complete { get; set; }

        public decimal orderTotal { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }

        public int? seller_id { get; set; }

        public virtual Customer Customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExportOrderItem> ExportOrderItems { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReportsExportOrdersMapping> ReportsExportOrdersMappings { get; set; }
    }
}
