using System;
using System.Collections.Generic;

namespace COCOApp.Models
{
    public partial class Product
    {
        public Product()
        {
            ExportOrderItems = new HashSet<ExportOrderItem>();
        }

        public int Id { get; set; }
        public string ProductName { get; set; } = null!;
        public string MeasureUnit { get; set; } = null!;
        public decimal Cost { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int SellerId { get; set; }

        public virtual User Seller { get; set; } = null!;
        public virtual ProductDetail? ProductDetail { get; set; }
        public virtual ICollection<ExportOrderItem> ExportOrderItems { get; set; }
    }
}
