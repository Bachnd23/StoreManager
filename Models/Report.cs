using System;
using System.Collections.Generic;

namespace COCOApp.Models
{
    public partial class Report
    {
        public Report()
        {
            ReportsOrdersMappings = new HashSet<ReportsOrdersMapping>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<ReportsOrdersMapping> ReportsOrdersMappings { get; set; }
    }
}
