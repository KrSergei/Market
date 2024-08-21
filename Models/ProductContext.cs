using Microsoft.EntityFrameworkCore;

namespace Market.Models
{
    public class ProductContext : DbContext
    {
        private string _connectionString = "Host=localhost;Username=postgres;Password=1;Database=Market";
        public virtual DbSet<ProductStorage> ProductStorages { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categoryes { get; set; }
        public virtual DbSet<Storage> Storages { get; set; }

        public ProductContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine).UseLazyLoadingProxies().UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //1.37.18
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(x => x.Id).HasName("product_pkey");
                entity.ToTable("Products");
                entity.Property(x => x.ID_user).HasColumnName("user_id");
                entity.Property(x => x.Name)
                      .HasMaxLength(255)
                      .HasColumnName("Name");
            });
        }
    }
}
