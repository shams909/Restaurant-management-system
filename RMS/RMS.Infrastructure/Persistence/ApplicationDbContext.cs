using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Persistence
{
    // Inheriting from DbContext is what gives this class its magic database powers
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // ==========================================
        // PASTE IT RIGHT HERE!
        // This puts all your tables in the "rms" schema
        // ==========================================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("rms");
            base.OnModelCreating(modelBuilder);
        }
        // This is what the professor asked for. It ensures the ORM always has a database connection!
       


        // Core
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }

        // Menu Engine
        public DbSet<MenuCategory> MenuCategories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<ItemVariant> ItemVariants { get; set; }
        public DbSet<ItemAddon> ItemAddons { get; set; }

        // Operations
        public DbSet<Table> Tables { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Recipe> Recipes { get; set; }

        // Orders
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderItemAddon> OrderItemAddons { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
