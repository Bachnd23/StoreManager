using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace RDLCReport
{
    public partial class Model : DbContext
    {
        public Model()
            : base("name=ConStr")
        {
        }

        public virtual DbSet<BuyerDetail> BuyerDetails { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<ExportOrderItem> ExportOrderItems { get; set; }
        public virtual DbSet<ExportOrder> ExportOrders { get; set; }
        public virtual DbSet<ImportOrderItem> ImportOrderItems { get; set; }
        public virtual DbSet<ImportOrder> ImportOrders { get; set; }
        public virtual DbSet<ProductDetail> ProductDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ReportDetail> ReportDetails { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<ReportsExportOrdersMapping> ReportsExportOrdersMappings { get; set; }
        public virtual DbSet<SellerDetail> SellerDetails { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Products)
                .WithOptional(e => e.Category)
                .HasForeignKey(e => e.category_id);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.ExportOrders)
                .WithOptional(e => e.Customer)
                .HasForeignKey(e => e.customer_id);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Reports)
                .WithOptional(e => e.Customer)
                .HasForeignKey(e => e.customer_id);

            modelBuilder.Entity<ExportOrderItem>()
                .Property(e => e.product_price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ExportOrderItem>()
                .Property(e => e.total)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ExportOrder>()
                .Property(e => e.orderTotal)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ExportOrder>()
                .HasMany(e => e.ExportOrderItems)
                .WithRequired(e => e.ExportOrder)
                .HasForeignKey(e => e.order_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ExportOrder>()
                .HasMany(e => e.ReportsExportOrdersMappings)
                .WithRequired(e => e.ExportOrder)
                .HasForeignKey(e => e.order_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ImportOrderItem>()
                .Property(e => e.product_cost)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ImportOrder>()
                .Property(e => e.orderTotal)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ImportOrder>()
                .HasMany(e => e.ImportOrderItems)
                .WithRequired(e => e.ImportOrder)
                .HasForeignKey(e => e.order_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.cost)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ExportOrderItems)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.product_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ImportOrderItems)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.product_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasOptional(e => e.ProductDetail)
                .WithRequired(e => e.Product);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ReportDetails)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.product_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Report>()
                .HasMany(e => e.ReportDetails)
                .WithRequired(e => e.Report)
                .HasForeignKey(e => e.report_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Report>()
                .HasMany(e => e.ReportsExportOrdersMappings)
                .WithRequired(e => e.Report)
                .HasForeignKey(e => e.report_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.ImportOrders)
                .WithOptional(e => e.Supplier)
                .HasForeignKey(e => e.supplier_id);

            modelBuilder.Entity<UserRole>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.UserRole)
                .HasForeignKey(e => e.role);

            modelBuilder.Entity<User>()
                .HasOptional(e => e.BuyerDetail)
                .WithRequired(e => e.User);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Customers)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.seller_id);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ExportOrderItems)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.seller_id);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ExportOrders)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.seller_id);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ImportOrders)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.seller_id);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Products)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.seller_id);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Reports)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.seller_id);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ReportsExportOrdersMappings)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.seller_id);

            modelBuilder.Entity<User>()
                .HasOptional(e => e.SellerDetail)
                .WithRequired(e => e.User);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Suppliers)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.seller_id);
        }
    }
}
