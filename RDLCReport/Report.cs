namespace RDLCReport
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Report
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Report()
        {
            ReportDetails = new HashSet<ReportDetail>();
            ReportsExportOrdersMappings = new HashSet<ReportsExportOrdersMapping>();
        }

        public int id { get; set; }

        public int? customer_id { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }

        public int? seller_id { get; set; }

        public virtual Customer Customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReportDetail> ReportDetails { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReportsExportOrdersMapping> ReportsExportOrdersMappings { get; set; }
    }
}
