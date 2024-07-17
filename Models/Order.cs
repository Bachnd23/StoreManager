using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Volume { get; set; }

        [Required(ErrorMessage = "Ngày đặt hàng là bắt buộc")]
        public DateTime OrderDate { get; set; }

        public bool Complete { get; set; }

        [Required(ErrorMessage = "Chi phí sản phẩm là bắt buộc")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Chi phí sản phẩm phải lớn hơn 0")]
        public decimal OrderProductCost { get; set; }

        [Required(ErrorMessage = "Tổng đơn hàng là bắt buộc")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Tổng đơn hàng phải lớn hơn 0")]
        public decimal OrderTotal { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int SellerId { get; set; }

        public virtual Customer? Customer { get; set; } = null!;

        public virtual Product? Product { get; set; } = null!;

        public virtual User? Seller { get; set; } = null!;

        public virtual ICollection<ReportsOrdersMapping> ReportsOrdersMappings { get; set; }
    }
}
