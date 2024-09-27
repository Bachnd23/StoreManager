using System;
using System.Collections.Generic;

namespace COCOApp.Models
{
    public partial class ExportOrder
    {
        public ExportOrder()
        {
            ExportOrderItems = new HashSet<ExportOrderItem>();
            ReportsExportOrdersMappings = new HashSet<ReportsExportOrdersMapping>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public bool Complete { get; set; }
        public decimal OrderTotal { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? SellerId { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual User? Seller { get; set; }
        public virtual ICollection<ExportOrderItem> ExportOrderItems { get; set; }
        public virtual ICollection<ReportsExportOrdersMapping> ReportsExportOrdersMappings { get; set; }
    }
}
