using Microsoft.EntityFrameworkCore;
using ProductCRUD.Models;

namespace ProductCRUD.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Products");

            // Specifying the primary key for stored procedure
            modelBuilder.Entity<Product>().HasKey(p => p.Id);

            // Maping stored procedure

            modelBuilder.Entity<Product>().ToFunction("CreateProduct");
            modelBuilder.Entity<Product>().ToFunction("GetProductById");
            modelBuilder.Entity<Product>().ToFunction("UpdateProduct");
            modelBuilder.Entity<Product>().ToFunction("DeleteProduct");
            modelBuilder.Entity<Product>().ToFunction("GetProducts");
        }

    }


}
