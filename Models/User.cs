using System;
using System.Collections.Generic;

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
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int Role { get; set; }
        public bool Status { get; set; }
        public string? RememberToken { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual UserRole RoleNavigation { get; set; } = null!;
        public virtual SellerDetail? SellerDetail { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<ReportsOrdersMapping> ReportsOrdersMappings { get; set; }
    }
}
