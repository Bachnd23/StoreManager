using System;
using System.Collections.Generic;

namespace COCOApp.Models
{
    public partial class ImportOrderItem
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Volume { get; set; }
        public decimal ProductCost { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ImportOrder Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
