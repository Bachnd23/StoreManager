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

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<ExportOrder> ExportOrders { get; set; } = null!;
        public virtual DbSet<ExportOrderItem> ExportOrderItems { get; set; } = null!;
        public virtual DbSet<ImportOrder> ImportOrders { get; set; } = null!;
        public virtual DbSet<ImportOrderItem> ImportOrderItems { get; set; } = null!;
        public virtual DbSet<InventoryManagement> InventoryManagements { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductDetail> ProductDetails { get; set; } = null!;
        public virtual DbSet<Report> Reports { get; set; } = null!;
        public virtual DbSet<ReportDetail> ReportDetails { get; set; } = null!;
        public virtual DbSet<SellerDetail> SellerDetails { get; set; } = null!;
        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserDetail> UserDetails { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(config.GetConnectionString("MyConStr"));
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
                    .HasConstraintName("FK__Customers__selle__3D5E1FD2");
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
                    .HasConstraintName("FK__ExportOrd__custo__4316F928");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.ExportOrders)
                    .HasForeignKey(d => d.SellerId)
                    .HasConstraintName("FK__ExportOrd__selle__4222D4EF");
            });

            modelBuilder.Entity<ExportOrderItem>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId })
                    .HasName("PK__ExportOr__022945F6F00DFF80");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.ProductPrice)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("product_price");

                entity.Property(e => e.RealVolume).HasColumnName("real_volume");

                entity.Property(e => e.SellerId).HasColumnName("seller_id");

                entity.Property(e => e.Status).HasColumnName("status");

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
                    .HasConstraintName("FK__ExportOrd__order__49C3F6B7");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ExportOrderItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ExportOrd__produ__4AB81AF0");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.ExportOrderItems)
                    .HasForeignKey(d => d.SellerId)
                    .HasConstraintName("FK__ExportOrd__selle__4BAC3F29");
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
                    .HasConstraintName("FK__ImportOrd__selle__619B8048");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.ImportOrders)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK__ImportOrd__suppl__628FA481");
            });

            modelBuilder.Entity<ImportOrderItem>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId })
                    .HasName("PK__ImportOr__022945F60309E7EC");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.ProductCost)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("product_cost");

                entity.Property(e => e.RealVolume).HasColumnName("real_volume");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.Volume).HasColumnName("volume");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ImportOrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ImportOrd__order__693CA210");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ImportOrderItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ImportOrd__produ__6A30C649");
            });

            modelBuilder.Entity<InventoryManagement>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("PK__Inventor__47027DF59B92ECA2");

                entity.ToTable("InventoryManagement");

                entity.Property(e => e.ProductId)
                    .ValueGeneratedNever()
                    .HasColumnName("product_id");

                entity.Property(e => e.AllocatedVolume).HasColumnName("allocated_volume");

                entity.Property(e => e.RemainingVolume).HasColumnName("remaining_volume");

                entity.Property(e => e.ShippedVolume).HasColumnName("shipped_volume");

                entity.HasOne(d => d.Product)
                    .WithOne(p => p.InventoryManagement)
                    .HasForeignKey<InventoryManagement>(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventory__produ__6FE99F9F");
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
                    .HasConstraintName("FK__Products__catego__35BCFE0A");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SellerId)
                    .HasConstraintName("FK__Products__seller__34C8D9D1");
            });

            modelBuilder.Entity<ProductDetail>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("PK__ProductD__47027DF510D018B5");

                entity.Property(e => e.ProductId)
                    .ValueGeneratedNever()
                    .HasColumnName("product_id");

                entity.Property(e => e.AdditionalInfo).HasColumnName("additional_info");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.HasOne(d => d.Product)
                    .WithOne(p => p.ProductDetail)
                    .HasForeignKey<ProductDetail>(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProductDe__produ__38996AB5");
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
                    .HasConstraintName("FK__Reports__custome__5165187F");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.SellerId)
                    .HasConstraintName("FK__Reports__seller___5070F446");
            });

            modelBuilder.Entity<ReportDetail>(entity =>
            {
                entity.HasKey(e => new { e.ReportId, e.ProductId })
                    .HasName("PK__ReportDe__33EB5B875FB6B529");

                entity.Property(e => e.ReportId).HasColumnName("report_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("date")
                    .HasColumnName("orderDate");

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ReportDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReportDet__produ__5535A963");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.ReportDetails)
                    .HasForeignKey(d => d.ReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReportDet__repor__5441852A");
            });

            modelBuilder.Entity<SellerDetail>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__SellerDe__B9BE370FA59D62B2");

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
                    .HasConstraintName("FK__SellerDet__user___5812160E");
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
                    .HasConstraintName("FK__Suppliers__selle__5CD6CB2B");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Users__AB6E61649625D6B6")
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
                    .HasConstraintName("FK__Users__role__2B3F6F97");
            });

            modelBuilder.Entity<UserDetail>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__UserDeta__B9BE370F2258664A");

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
                    .WithOne(p => p.UserDetail)
                    .HasForeignKey<UserDetail>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserDetai__user___2E1BDC42");
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
