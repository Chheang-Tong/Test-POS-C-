using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace SimplePosDesktop.Data
{
  // Product table
  public class Product
  {
    public int Id { get; set; }
    public string Barcode { get; set; } = "";
    public string Name { get; set; } = "";
    public double Price { get; set; }
    public int StockQty { get; set; }
    public override string ToString()
    {
      return $"{Name} - ${Price:F2}";
    }

  }

  // Sale / invoice
  public class Sale
  {
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public double TotalAmount { get; set; }

    public List<SaleItem> Items { get; set; } = new();
  }

  // Items inside each sale
  public class SaleItem
  {
    public int Id { get; set; }

    public int SaleId { get; set; }
    public Sale? Sale { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public int Quantity { get; set; }
    public double UnitPrice { get; set; }
    public double LineTotal { get; set; }
  }

  public class PosDbContext : DbContext
  {
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<SaleItem> SaleItems => Set<SaleItem>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlite("Data Source=pos.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Product>().HasData(
          new Product { Id = 1, Barcode = "1001", Name = "Water Bottle", Price = 0.50, StockQty = 100 },
          new Product { Id = 2, Barcode = "1002", Name = "Sandwich", Price = 2.50, StockQty = 50 },
          new Product { Id = 3, Barcode = "1003", Name = "Snack Pack", Price = 1.75, StockQty = 80 },
          new Product { Id = 4, Barcode = "1004", Name = "Notebook", Price = 3.20, StockQty = 30 }
      );
    }
  }
}
