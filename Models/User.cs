using System;
using System.Collections.Generic;

namespace COCOApp.Models
{
    public partial class User
    {
        public User()
        {
            Customers = new HashSet<Customer>();
            ExportOrderItems = new HashSet<ExportOrderItem>();
            ExportOrders = new HashSet<ExportOrder>();
            ImportOrders = new HashSet<ImportOrder>();
            Products = new HashSet<Product>();
            Reports = new HashSet<Report>();
            ReportsExportOrdersMappings = new HashSet<ReportsExportOrdersMapping>();
            Suppliers = new HashSet<Supplier>();
        }

        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int? Role { get; set; }
        public bool Status { get; set; }
        public string? RememberToken { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual UserRole? RoleNavigation { get; set; }
        public virtual BuyerDetail? BuyerDetail { get; set; }
        public virtual SellerDetail? SellerDetail { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<ExportOrderItem> ExportOrderItems { get; set; }
        public virtual ICollection<ExportOrder> ExportOrders { get; set; }
        public virtual ICollection<ImportOrder> ImportOrders { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<ReportsExportOrdersMapping> ReportsExportOrdersMappings { get; set; }
        public virtual ICollection<Supplier> Suppliers { get; set; }
    }
}
