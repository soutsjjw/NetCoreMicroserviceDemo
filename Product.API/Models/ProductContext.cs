﻿using Microsoft.EntityFrameworkCore;

namespace Product.API.Models
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options)
            : base(options)
        { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(new Product
            {
                ID = 1,
                Name = "產品1",
                Stock = 100
            },
            new Product
            {
                ID = 2,
                Name = "產品2",
                Stock = 100
            });
        }
    }
}
