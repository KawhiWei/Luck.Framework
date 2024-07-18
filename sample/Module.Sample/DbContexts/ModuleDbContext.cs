using Luck.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using Module.Sample.Domain;

namespace Module.Sample.DbContexts
{
    public class ModuleDbContext : LuckDbContextBase
    {
        public ModuleDbContext(DbContextOptions options, IServiceProvider serviceProvider) : base(options)
        {
        }
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
