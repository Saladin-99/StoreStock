using Microsoft.EntityFrameworkCore;
using StoreStock.Models;

namespace StoreStock.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<StockItem> StockItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure composite primary key for StockItem entity
            modelBuilder.Entity<StockItem>()
                .HasKey(e => new { e.StoreId, e.ProductId });
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(10,2)");
        }
    }
}
