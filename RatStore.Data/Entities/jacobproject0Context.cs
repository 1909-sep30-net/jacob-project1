using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RatStore.Data.Entities
{
    public partial class jacobproject0Context : DbContext
    {
        public jacobproject0Context()
        {
        }

        public jacobproject0Context(DbContextOptions<jacobproject0Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Component> Component { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductComponent> ProductComponent { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=RatStoreDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Component>(entity =>
            {
                entity.Property(e => e.ComponentId).HasColumnName("componentId");

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasColumnType("money");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.Username)
                    .HasName("UQ__Customer__F3DBC572C4DC95EA")
                    .IsUnique();

                entity.Property(e => e.CustomerId).HasColumnName("customerId");

                entity.Property(e => e.FirstName)
                    .HasColumnName("firstName")
                    .HasMaxLength(255);

                entity.Property(e => e.LastName)
                    .HasColumnName("lastName")
                    .HasMaxLength(255);

                entity.Property(e => e.MiddleName)
                    .HasColumnName("middleName")
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(255);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phoneNumber")
                    .HasMaxLength(255);

                entity.Property(e => e.PreferredStoreId)
                    .HasColumnName("preferredStoreId")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(255);

                entity.HasOne(d => d.PreferredStore)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.PreferredStoreId)
                    .HasConstraintName("FK__Customer__prefer__66B53B20");
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(e => new { e.LocationId, e.ComponentId })
                    .HasName("PK__Inventor__4603C7420A5A355D");

                entity.Property(e => e.LocationId).HasColumnName("locationId");

                entity.Property(e => e.ComponentId).HasColumnName("componentId");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Component)
                    .WithMany(p => p.Inventory)
                    .HasForeignKey(d => d.ComponentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventory__compo__5772F790");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Inventory)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventory__locat__567ED357");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.LocationId).HasColumnName("locationId");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("orderId");

                entity.Property(e => e.CustomerId).HasColumnName("customerId");

                entity.Property(e => e.LocationId).HasColumnName("locationId");

                entity.Property(e => e.OrderDate)
                    .HasColumnName("orderDate")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('GETDATE()')");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Order__customerI__6D6238AF");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK__Order__locationI__6C6E1476");
            });

            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId })
                    .HasName("PK__OrderDet__BAD83E4BBCD7F5C6");

                entity.Property(e => e.OrderId).HasColumnName("orderId");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderDeta__order__703EA55A");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderDeta__produ__7132C993");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<ProductComponent>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.ComponentId })
                    .HasName("PK__ProductC__5B777D4620B911D6");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.ComponentId).HasColumnName("componentId");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Component)
                    .WithMany(p => p.ProductComponent)
                    .HasForeignKey(d => d.ComponentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProductCo__compo__4DE98D56");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductComponent)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProductCo__produ__4CF5691D");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
