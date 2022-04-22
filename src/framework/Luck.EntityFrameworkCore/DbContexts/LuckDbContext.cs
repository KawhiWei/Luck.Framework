using Luck.EntityFrameworkCore.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.EntityFrameworkCore.DbContexts
{
    public class LuckDbContext : DbContext, ILuckDbContext
    {
        protected LuckDbContext()
        {
        }

        protected IServiceProvider ServiceProvider { get; set; }
        private IMediator? _mediator;
        protected LuckDbContext(DbContextOptions options, IServiceProvider serviceProvider) : base(options)
        {
            ServiceProvider = serviceProvider;
            _mediator = ServiceProvider.GetService<IMediator>();
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


        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaveChange();

            if (this._mediator != null)
            {
                await _mediator.DispatchDomainEventsAsync(this, cancellationToken);
            }

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
