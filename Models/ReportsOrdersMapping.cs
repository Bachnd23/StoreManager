using System;
using System.Collections.Generic;

namespace COCOApp.Models
{
    public partial class ReportsOrdersMapping
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public int OrderId { get; set; }
        public int SellerId { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Report Report { get; set; } = null!;
        public virtual User Seller { get; set; } = null!;
    }
}
