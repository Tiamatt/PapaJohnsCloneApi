using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PapaJohnsCloneApi.Models
{
    public partial class PapaJohnsCloneDbContext : DbContext
    {
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<ItemAllowedQuestions> ItemAllowedQuestions { get; set; }
        public virtual DbSet<ItemCategory> ItemCategory { get; set; }
        public virtual DbSet<ItemPrice> ItemPrice { get; set; }
        public virtual DbSet<ItemSelectedQuestion> ItemSelectedQuestion { get; set; }
        public virtual DbSet<ItemSelectedToppings> ItemSelectedToppings { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<QuestionCategory> QuestionCategory { get; set; }
        public virtual DbSet<Topping> Topping { get; set; }
        public virtual DbSet<ToppingCategory> ToppingCategory { get; set; }

        public PapaJohnsCloneDbContext(DbContextOptions<PapaJohnsCloneDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ_Item_Name")
                    .IsUnique();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.ItemCategory)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.ItemCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Item_ItemCategoryId");
            });

            modelBuilder.Entity<ItemAllowedQuestions>(entity =>
            {
                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemAllowedQuestions)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemAllowedQuestions_ItemId");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.ItemAllowedQuestions)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemAllowedQuestions_QuestionId");
            });

            modelBuilder.Entity<ItemCategory>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ_ItemCategory_Name")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ItemPrice>(entity =>
            {
                entity.Property(e => e.BasicPrice).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PricePerTopping).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.ItemCategory)
                    .WithMany(p => p.ItemPrice)
                    .HasForeignKey(d => d.ItemCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemPrice_ItemCategoryId");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemPrice)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_ItemPrice_ItemId");

                entity.HasOne(d => d.QuestionIdSizeNavigation)
                    .WithMany(p => p.ItemPrice)
                    .HasForeignKey(d => d.QuestionIdSize)
                    .HasConstraintName("FK_ItemPrice_QuestionIdSize");
            });

            modelBuilder.Entity<ItemSelectedQuestion>(entity =>
            {
                entity.HasIndex(e => new { e.ItemId, e.QuestionCategoryId })
                    .HasName("UQ_ItemSelectedQuestion_ItemId_QuestionCategoryId")
                    .IsUnique();

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemSelectedQuestion)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemSelectedQuestion_ItemId");

                entity.HasOne(d => d.QuestionCategory)
                    .WithMany(p => p.ItemSelectedQuestion)
                    .HasForeignKey(d => d.QuestionCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemSelectedQuestion_QuestionCategoryId");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.ItemSelectedQuestion)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemSelectedQuestion_QuestionId");
            });

            modelBuilder.Entity<ItemSelectedToppings>(entity =>
            {
                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemSelectedToppings)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemSelectedToppings_ItemId");

                entity.HasOne(d => d.Topping)
                    .WithMany(p => p.ItemSelectedToppings)
                    .HasForeignKey(d => d.ToppingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemSelectedToppings_ToppingId");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.QuestionCategory)
                    .WithMany(p => p.Question)
                    .HasForeignKey(d => d.QuestionCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Question_QuestionCategoryId");
            });

            modelBuilder.Entity<QuestionCategory>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ_FilterCategory_Name")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Topping>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.ToppingCategory)
                    .WithMany(p => p.Topping)
                    .HasForeignKey(d => d.ToppingCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Topping_ToppingCategoryId");
            });

            modelBuilder.Entity<ToppingCategory>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ_ToppingCategory_Name")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
