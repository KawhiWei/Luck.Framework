﻿using Luck.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using Module.Sample.Domain;

namespace Module.Sample.DbContexts
{
    public class ModuleDbContext : LuckDbContext
    {
        public ModuleDbContext(DbContextOptions options, IServiceProvider serviceProvider) : base(options, serviceProvider)
        {
        }
        public DbSet<Order> Orders => Set<Order>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
