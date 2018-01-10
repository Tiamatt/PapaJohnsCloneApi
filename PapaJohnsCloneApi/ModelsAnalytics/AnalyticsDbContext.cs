using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PapaJohnsCloneApi.ModelsAnalytics
{
    public partial class AnalyticsDbContext : DbContext
    {
        public virtual DbSet<AdminLog> AdminLog { get; set; }
        public virtual DbSet<AdminLogAction> AdminLogAction { get; set; }
        public virtual DbSet<Brand> Brand { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Color> Color { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<ErrorLog> ErrorLog { get; set; }
        public virtual DbSet<ErrorLogType> ErrorLogType { get; set; }
        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<ItemAction> ItemAction { get; set; }
        public virtual DbSet<ItemDetail> ItemDetail { get; set; }
        public virtual DbSet<ItemImage> ItemImage { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<PriceRange> PriceRange { get; set; }
        public virtual DbSet<Size> Size { get; set; }

        public AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options)
        : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminLog>(entity =>
            {
                entity.Property(e => e.FieldName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.FieldValue)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Notes).HasMaxLength(2000);

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Timestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.AdminLogAction)
                    .WithMany(p => p.AdminLog)
                    .HasForeignKey(d => d.AdminLogActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AdminLog_AdminLogActionId_FK");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.AdminLog)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AdminLog_EmployeeId_FK");
            });

            modelBuilder.Entity<AdminLogAction>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("AdminLogAction_Name_Unique")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("Brand_Name_Unique")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("Category_Name_Unique")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("Color_Name_Unique")
                    .IsUnique();

                entity.Property(e => e.HexCode).HasMaxLength(7);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .HasName("Customer_Email_Unique")
                    .IsUnique();

                entity.Property(e => e.CustomerId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("Department_Name_Unique")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => new { e.FirstName, e.MiddleName, e.LastName, e.Birthday })
                    .HasName("Employee_FourFields_Unique")
                    .IsUnique();

                entity.Property(e => e.EmployeeId).ValueGeneratedNever();

                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.HireDate).HasColumnType("datetime");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.MiddleName).HasMaxLength(100);

                entity.Property(e => e.TerminationDate).HasColumnType("datetime");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Employee_DepartmentId_FK");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.PositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Employee_PositionId_FK");
            });

            modelBuilder.Entity<ErrorLog>(entity =>
            {
                entity.Property(e => e.Class)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.DetailedComment).HasMaxLength(2000);

                entity.Property(e => e.Exception).HasColumnType("xml");

                entity.Property(e => e.Method)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.MethodParams).HasMaxLength(2000);

                entity.Property(e => e.Namespace)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ShortComment)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Timestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.ErrorLog)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ErrorLog_EmployeeId_FK");

                entity.HasOne(d => d.ErrorLogType)
                    .WithMany(p => p.ErrorLog)
                    .HasForeignKey(d => d.ErrorLogTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ErrorLog_ErrorLogTypeId_FK");
            });

            modelBuilder.Entity<ErrorLogType>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("ErrorLogType_Name_Unique")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("Gender_Name_Unique")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("Item_Name_Unique")
                    .IsUnique();

                entity.Property(e => e.ItemId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Item_BrandId_FK");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Item_CategoryId_FK");

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.GenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Item_GenderId_FK");
            });

            modelBuilder.Entity<ItemAction>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("ItemAction_Name_Unique")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ItemDetail>(entity =>
            {
                entity.Property(e => e.ItemDetailId).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.ItemDetail)
                    .HasForeignKey(d => d.ColorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ItemDetail_ColorId_FK");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.ItemDetail)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("ItemDetail_CustomerId_FK");

                entity.HasOne(d => d.ItemAction)
                    .WithMany(p => p.ItemDetail)
                    .HasForeignKey(d => d.ItemActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Item_ItemActionId_FK");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemDetail)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ItemDetail_ItemId_FK");

                entity.HasOne(d => d.Size)
                    .WithMany(p => p.ItemDetail)
                    .HasForeignKey(d => d.SizeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ItemDetail_SizeId_FK");
            });

            modelBuilder.Entity<ItemImage>(entity =>
            {
                entity.Property(e => e.ItemImageId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.ImageType).HasMaxLength(200);

                entity.Property(e => e.Src).IsRequired();

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemImage)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ItemImage_ItemId_FK");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("Position_Name_Unique")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<PriceRange>(entity =>
            {
                entity.Property(e => e.Max).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Min).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<Size>(entity =>
            {
                entity.Property(e => e.AustraliaSizing).HasMaxLength(10);

                entity.Property(e => e.EuropeSizing).HasMaxLength(10);

                entity.Property(e => e.JapanSizing).HasMaxLength(10);

                entity.Property(e => e.Sizing).HasMaxLength(10);

                entity.Property(e => e.UkSizing).HasMaxLength(10);

                entity.Property(e => e.UsaSizing).HasMaxLength(10);
            });
        }
    }
}
