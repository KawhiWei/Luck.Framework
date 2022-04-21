using Luck.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Luck.EntityFrameworkCore.DbContexts
{
    public class LuckDbContext : DbContext, ILuckDbContext
    {
        protected LuckDbContext()
        {
        }
        protected LuckDbContext(DbContextOptions options) : base(options)
        {
        }
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
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaveChange();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnBeforeSaveChange();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaveChange();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
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
