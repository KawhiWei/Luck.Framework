using Luck.EntityFrameworkCore.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luck.EntityFrameworkCore.DbContexts
{
    /// <summary>
    /// 类类上下文
    /// </summary>
    public abstract class DbContextBase:DbContext
    {
        protected IMediator? _mediator;
        protected IServiceProvider ServiceProvider { get; set; }

        protected DbContextBase()
        { 
        }
        protected DbContextBase(DbContextOptions options, IServiceProvider serviceProvider) : base(options)
        {
            ServiceProvider = serviceProvider;
            _mediator = ServiceProvider?.GetService<IMediator>();
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
