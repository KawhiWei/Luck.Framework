using Luck.EntityFrameworkCore.DbContexts;
using Luck.Walnut.Domain.AggregateRoots.Applications;
using Luck.Walnut.Domain.AggregateRoots.Environments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Luck.Walnut.Infrastructure
{
    public class WalnutDbContext : LuckDbContextBase
    {

        public WalnutDbContext(DbContextOptions options, IServiceProvider serviceProvider) : base(options, serviceProvider)
        {
        }

        public DbSet<AppConfiguration> AppConfigurations => Set<AppConfiguration>();


        public DbSet<AppEnvironment> AppEnvironments => Set<AppEnvironment>();

        public DbSet<Application> MyApplications => Set<Application>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
