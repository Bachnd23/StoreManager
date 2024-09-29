using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace COCOApp.Models
{
    public partial class StoreManagerContext : DbContext
    {
        public StoreManagerContext()
        {
        }

        public StoreManagerContext(DbContextOptions<StoreManagerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BuyerDetail> BuyerDetails { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<ExportOrder> ExportOrders { get; set; } = null!;
        public virtual DbSet<ExportOrderItem> ExportOrderItems { get; set; } = null!;
        public virtual DbSet<ImportOrder> ImportOrders { get; set; } = null!;
        public virtual DbSet<ImportOrderItem> ImportOrderItems { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductDetail> ProductDetails { get; set; } = null!;
        public virtual DbSet<Report> Reports { get; set; } = null!;
        public virtual DbSet<ReportDetail> ReportDetails { get; set; } = null!;
        public virtual DbSet<ReportsExportOrdersMapping> ReportsExportOrdersMappings { get; set; } = null!;
        public virtual DbSet<SellerDetail> SellerDetails { get; set; } = null!;
        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=StoreManager;User ID=sa;Password=123456;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BuyerDetail>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__BuyerDet__B9BE370F0C640C7F");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("user_id");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .HasColumnName("address");

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("dob");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(255)
                    .HasColumnName("fullname");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.Phone)
                    .HasMaxLength(255)
                    .HasColumnName("phone");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.BuyerDetail)
                    .HasForeignKey<BuyerDetail>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BuyerDeta__user___403A8C7D");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryName).HasMaxLength(255);

                entity.Property(e => e.Description).HasMaxLength(255);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Note).HasColumnName("note");

                entity.Property(e => e.Phone).HasColumnName("phone");

                entity.Property(e => e.SellerId).HasColumnName("seller_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.SellerId)
                    .HasConstraintName("FK__Customers__selle__4F7CD00D");
            });

            modelBuilder.Entity<ExportOrder>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Complete).HasColumnName("complete");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("date")
                    .HasColumnName("orderDate");

                entity.Property(e => e.OrderTotal)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("orderTotal");

                entity.Property(e => e.SellerId).HasColumnName("seller_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.ExportOrders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__ExportOrd__custo__5535A963");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.ExportOrders)
                    .HasForeignKey(d => d.SellerId)
                    .HasConstraintName("FK__ExportOrd__selle__5441852A");
            });

            modelBuilder.Entity<ExportOrderItem>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId })
                    .HasName("PK__ExportOr__022945F68383BCDF");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.ProductPrice)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("product_price");

                entity.Property(e => e.SellerId).HasColumnName("seller_id");

                entity.Property(e => e.Total)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("total");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.Volume).HasColumnName("volume");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ExportOrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ExportOrd__order__59FA5E80");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ExportOrderItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ExportOrd__produ__5AEE82B9");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.ExportOrderItems)
                    .HasForeignKey(d => d.SellerId)
                    .HasConstraintName("FK__ExportOrd__selle__5BE2A6F2");
            });

            modelBuilder.Entity<ImportOrder>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Complete).HasColumnName("complete");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("date")
                    .HasColumnName("orderDate");

                entity.Property(e => e.OrderTotal)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("orderTotal");

                entity.Property(e => e.SellerId).HasColumnName("seller_id");

                entity.Property(e => e.SupplierId).HasColumnName("supplier_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.ImportOrders)
                    .HasForeignKey(d => d.SellerId)
                    .HasConstraintName("FK__ImportOrd__selle__0B91BA14");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.ImportOrders)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK__ImportOrd__suppl__0C85DE4D");
            });

            modelBuilder.Entity<ImportOrderItem>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId })
                    .HasName("PK__ImportOr__022945F6EABD2A0D");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.ProductCost)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("product_cost");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.Volume).HasColumnName("volume");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ImportOrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ImportOrd__order__114A936A");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ImportOrderItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ImportOrd__produ__123EB7A3");
            });

            modelBuilder.Entity<ImportOrder>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Complete).HasColumnName("complete");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("date")
                    .HasColumnName("orderDate");

                entity.Property(e => e.OrderTotal)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("orderTotal");

                entity.Property(e => e.SellerId).HasColumnName("seller_id");

                entity.Property(e => e.SupplierId).HasColumnName("supplier_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.ImportOrders)
                    .HasForeignKey(d => d.SellerId)
                    .HasConstraintName("FK__ImportOrd__selle__76969D2E");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.ImportOrders)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK__ImportOrd__suppl__778AC167");
            });

            modelBuilder.Entity<ImportOrderItem>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId })
                    .HasName("PK__ImportOr__022945F64E082F75");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.ProductCost)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("product_cost");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.Volume).HasColumnName("volume");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ImportOrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ImportOrd__order__7C4F7684");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ImportOrderItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ImportOrd__produ__7D439ABD");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Cost)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("cost");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.MeasureUnit).HasMaxLength(255);

                entity.Property(e => e.ProductName).HasMaxLength(255);

                entity.Property(e => e.SellerId).HasColumnName("seller_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Products__catego__47DBAE45");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SellerId)
                    .HasConstraintName("FK__Products__seller__46E78A0C");
            });

            modelBuilder.Entity<ProductDetail>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("PK__ProductD__47027DF58C403DED");

                entity.Property(e => e.ProductId)
                    .ValueGeneratedNever()
                    .HasColumnName("product_id");

                entity.Property(e => e.AdditionalInfo).HasColumnName("additional_info");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.HasOne(d => d.Product)
                    .WithOne(p => p.ProductDetail)
                    .HasForeignKey<ProductDetail>(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProductDe__produ__4AB81AF0");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.SellerId).HasColumnName("seller_id");

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Reports__custome__619B8048");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.SellerId)
                    .HasConstraintName("FK__Reports__seller___60A75C0F");
            });

            modelBuilder.Entity<ReportDetail>(entity =>
            {
                entity.HasKey(e => e.ReportId)
                    .HasName("PK__ReportDe__779B7C58D5E09062");

                entity.Property(e => e.ReportId)
                    .ValueGeneratedNever()
                    .HasColumnName("report_id");

                entity.Property(e => e.Details).HasColumnName("details");

                entity.HasOne(d => d.Report)
                    .WithOne(p => p.ReportDetail)
                    .HasForeignKey<ReportDetail>(d => d.ReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReportDet__repor__6477ECF3");
            });

            modelBuilder.Entity<ReportsExportOrdersMapping>(entity =>
            {
                entity.HasKey(e => new { e.ReportId, e.OrderId })
                    .HasName("PK__ReportsE__F3FEEA7AB7B067B9");

                entity.ToTable("ReportsExportOrdersMapping");

                entity.Property(e => e.ReportId).HasColumnName("report_id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.SellerId).HasColumnName("seller_id");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ReportsExportOrdersMappings)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReportsEx__order__693CA210");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.ReportsExportOrdersMappings)
                    .HasForeignKey(d => d.ReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReportsEx__repor__68487DD7");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.ReportsExportOrdersMappings)
                    .HasForeignKey(d => d.SellerId)
                    .HasConstraintName("FK__ReportsEx__selle__6754599E");
            });

            modelBuilder.Entity<SellerDetail>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__SellerDe__B9BE370F3128401B");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("user_id");

                entity.Property(e => e.BusinessAddress)
                    .HasMaxLength(255)
                    .HasColumnName("business_address");

                entity.Property(e => e.BusinessName)
                    .HasMaxLength(255)
                    .HasColumnName("business_name");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.SellerDetail)
                    .HasForeignKey<SellerDetail>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SellerDet__user___6C190EBB");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Note).HasColumnName("note");

                entity.Property(e => e.Phone).HasColumnName("phone");

                entity.Property(e => e.SellerId).HasColumnName("seller_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.Suppliers)
                    .HasForeignKey(d => d.SellerId)
                    .HasConstraintName("FK__Suppliers__selle__06CD04F7");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Note).HasColumnName("note");

                entity.Property(e => e.Phone).HasColumnName("phone");

                entity.Property(e => e.SellerId).HasColumnName("seller_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.Suppliers)
                    .HasForeignKey(d => d.SellerId)
                    .HasConstraintName("FK__Suppliers__selle__71D1E811");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Users__AB6E61645F15E132")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.RememberToken)
                    .HasMaxLength(100)
                    .HasColumnName("remember_token");

                entity.Property(e => e.ResetPasswordToken)
                    .HasMaxLength(100)
                    .HasColumnName("reset_password_token");

                entity.Property(e => e.Role).HasColumnName("role");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .HasColumnName("username");

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Role)
                    .HasConstraintName("FK__Users__role__3D5E1FD2");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
