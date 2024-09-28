using System;
using System.Collections.Generic;

namespace COCOApp.Models
{
    public partial class Report
    {
        public Report()
        {
            ReportsExportOrdersMappings = new HashSet<ReportsExportOrdersMapping>();
        }

        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? SellerId { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual User? Seller { get; set; }
        public virtual ReportDetail? ReportDetail { get; set; }
        public virtual ICollection<ReportsExportOrdersMapping> ReportsExportOrdersMappings { get; set; }
    }
}
