using RMS.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Persistence
{



    public class ApplicationDbContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;

        // [MODIFIED]: We inject the CurrentUserService here!
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService)
            : base(options)
        {
            _currentUserService = currentUserService;
        }

        // We create dynamic properties so EF Core can re-evaluate them on EVERY request!
        public Guid CurrentTenantId => string.IsNullOrEmpty(_currentUserService.TenantId) ? Guid.Empty : Guid.Parse(_currentUserService.TenantId);
        public int CurrentBranchId => _currentUserService.BranchId;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Restore the Professor's required default schema!
            builder.HasDefaultSchema("rms");
            
            base.OnModelCreating(builder);

            // [NEW]: Global Query Filters (The Invisible Walls)
            // By referencing CurrentTenantId, EF Core will run this check dynamically every single time!
            
            // Tell Entity Framework to NEVER return data that doesn't belong to this Tenant!
            builder.Entity<MenuItem>().HasQueryFilter(e => e.TenantId == CurrentTenantId);
            builder.Entity<MenuCategory>().HasQueryFilter(e => e.TenantId == CurrentTenantId);
            builder.Entity<Customer>().HasQueryFilter(e => e.TenantId == CurrentTenantId);

            // Isolate Branch-specific data!
            builder.Entity<Order>().HasQueryFilter(e => e.BranchId == CurrentBranchId || CurrentBranchId == 0);
            builder.Entity<InventoryItem>().HasQueryFilter(e => e.BranchId == CurrentBranchId || CurrentBranchId == 0);
            builder.Entity<Table>().HasQueryFilter(e => e.BranchId == CurrentBranchId || CurrentBranchId == 0);
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

        // Inventory Transactions
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }

    }
}
