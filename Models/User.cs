using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace COCOApp.Models
{
    public partial class User
    {
        public User()
        {
            Customers = new HashSet<Customer>();
            Orders = new HashSet<Order>();
            Products = new HashSet<Product>();
            Reports = new HashSet<Report>();
            ReportsOrdersMappings = new HashSet<ReportsOrdersMapping>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        [StringLength(50, ErrorMessage = "Độ dài tên đăng nhập không được vượt quá 50 ký tự")]
        [MinLength(3, ErrorMessage = "Tên đăng nhập phải có ít nhất 3 ký tự")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(100, ErrorMessage = "Độ dài mật khẩu không được vượt quá 100 ký tự")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { get; set; } = null!;

        public int Role { get; set; }

        public bool Status { get; set; }

        public string? RememberToken { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual UserRole? RoleNavigation { get; set; } = null!;
        public virtual SellerDetail? SellerDetail { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<ReportsOrdersMapping> ReportsOrdersMappings { get; set; }
    }
}
