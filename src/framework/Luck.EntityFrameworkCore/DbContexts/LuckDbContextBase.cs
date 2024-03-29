﻿using Luck.EntityFrameworkCore.Extensions;
using Luck.Framework.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Luck.EntityFrameworkCore.DbContexts
{
    /// <summary>
    /// 类类上下文
    /// </summary>
    public abstract class LuckDbContextBase(DbContextOptions options, IServiceProvider serviceProvider) : DbContext(options), ILuckDbContext
    {
        // ReSharper disable once MemberCanBePrivate.Global
        protected IServiceProvider ServiceProvider { get; set; } = Check.NotNull(serviceProvider, nameof(serviceProvider));

        // ReSharper disable once PublicConstructorInAbstractClass


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.UseModification();

            modelBuilder.UseDeletion();

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public override int SaveChanges()
        {
            OnBeforeSaveChange();
            return base.SaveChanges();
        }


        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaveChange();
            var count = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            return count;

        }

        public virtual void Rollback()
        {
            ChangeTracker.Clear();

        }


        private void OnBeforeSaveChange()
        {
            ChangeTracker.UpdateModification();
            ChangeTracker.UpdateDeletion();
        }
    }
}
