using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ItemProduct> ItemProducts { get; set; }
    public DbSet<Finance> Finances { get; set; }
    public DbSet<ItemFinance> ItemFinances { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Orders)
            .WithOne(o => o.User)
            .HasForeignKey(o => o.UserId);
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.Products)
            .WithOne(u => u.User)
            .HasForeignKey(u => u.UserId);
        
        modelBuilder.Entity<Product>()
            .HasOne(u => u.Category)
            .WithMany(x => x.Products)
            .HasForeignKey(u => u.CategoryId);
        modelBuilder.Entity<Product>()
            .HasMany(x => x.Orders)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId);
        
        modelBuilder.Entity<Product>()
            .HasMany(x => x.ItemProducts)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId);
        
        modelBuilder.Entity<User>()
            .HasOne(x => x.Finance)
            .WithOne(x => x.User)
            .HasForeignKey<Finance>(x => x.UserId);
        
        modelBuilder.Entity<Finance>()
            .HasIndex(x => x.UserId)
            .IsUnique();
        
        modelBuilder.Entity<Finance>()
            .HasMany(x => x.Items)
            .WithOne(x => x.Finance)
            .HasForeignKey(x => x.FinanceId);
    }
}
