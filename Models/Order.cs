using System;
using System.Collections.Generic;

namespace COCOApp.Models
{
    public partial class Order
    {
        public Order()
        {
            ReportsOrdersMappings = new HashSet<ReportsOrdersMapping>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Volume { get; set; }
        public DateTime Date { get; set; }
        public bool Complete { get; set; }
        public decimal OrderProductCost { get; set; }
        public decimal OrderTotal { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public virtual ICollection<ReportsOrdersMapping> ReportsOrdersMappings { get; set; }
    }
}
