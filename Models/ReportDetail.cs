using System;
using System.Collections.Generic;

namespace COCOApp.Models
{
    public partial class ReportDetail
    {
        public int ReportId { get; set; }
        public string? Details { get; set; }

        public virtual Report Report { get; set; } = null!;
    }
}
