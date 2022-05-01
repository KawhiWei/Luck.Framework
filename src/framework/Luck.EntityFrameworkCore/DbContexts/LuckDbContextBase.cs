using Luck.EntityFrameworkCore.Extensions;
using Luck.Framework.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.EntityFrameworkCore.DbContexts
{
    /// <summary>
    /// 类类上下文
    /// </summary>
    public abstract class LuckDbContextBase : DbContext, ILuckDbContext
    {
        protected IMediator _mediator;
        protected IServiceProvider ServiceProvider { get; set; }

        public LuckDbContextBase(DbContextOptions options, IServiceProvider serviceProvider) : base(options)
        {
            ServiceProvider = Check.NotNull(serviceProvider,nameof(serviceProvider));
            _mediator = Check.NotNull(ServiceProvider.GetService<IMediator>()!, nameof(_mediator));
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
            
            await _mediator.DispatchDomainEventsAsync(this, cancellationToken);

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
