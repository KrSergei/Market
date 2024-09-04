using Microsoft.EntityFrameworkCore;

namespace Market.Models
{
    public class ProductContext : DbContext
    {
        private string _connectionString;

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categoryes { get; set; }
        public virtual DbSet<Storage> Storages { get; set; }


        public ProductContext(string connectingString)
        {
            _connectionString = connectingString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine).UseLazyLoadingProxies().UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(x => x.Id).HasName("ProductId");
                entity.HasIndex(x => x.Name).IsUnique();

                entity.Property(e => e.Name)
                .HasColumnName("ProductName")
                .HasMaxLength(255)
                .IsRequired();

                entity.Property(e => e.Description)
                .HasColumnName("Description")
                .HasMaxLength(255)
                .IsRequired();

                entity.Property(e => e.Price)
                .HasColumnName("Price")
                .IsRequired();

                entity.HasOne(x => x.Category)
                .WithMany(z => z.Products)
                .HasForeignKey(c =>c.Id)
                .HasConstraintName("CategoryToProduct");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("ProductsCategory");

                entity.HasKey(x => x.Id).HasName("CategoryId");
                entity.HasIndex(x => x.Name).IsUnique();

                entity.Property(e => e.Name)
                .HasColumnName("ProductName")
                .HasMaxLength(255)
                .IsRequired();
            });

            modelBuilder.Entity<Storage>(entity =>
            {
                entity.ToTable("Storage");
                entity.HasKey(x => x.Id).HasName("StorageId");

                entity.Property(x => x.Name)
                .HasColumnName("StorageName");

                entity.Property(x => x.Count)
                .HasColumnName("ProductCount");      

                entity.HasMany(x => x.Products)
                .WithMany(s => s.Storages)
                .UsingEntity(z => z.ToTable("ProductStorage"));
            });
        }
    }
}
