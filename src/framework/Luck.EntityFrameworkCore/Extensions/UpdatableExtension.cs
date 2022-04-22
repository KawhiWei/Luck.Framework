using Luck.DDD.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Luck.EntityFrameworkCore.Extensions
{
    public static class UpdatableExtension
    {
        //private const string _createTimePropertyName = "CreationTime";
        //private const string _lastModificationTimePropertyName = "LastModificationTime";

        public static void UseModification(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                //值对象或子类。跳过SoftDeletable逻辑
                if (entityType.IsOwned() || entityType.BaseType != null)
                {
                    continue;
                }
                //if (typeof(IUpdatable).IsAssignableFrom(entityType.ClrType))
                //{
                //    entityType.AddProperty<DateTimeOffset>(_createTimePropertyName);
                //    entityType.AddProperty<DateTimeOffset?>(_lastModificationTimePropertyName);
                //}
            }
        }

        public static void UpdateModification(this ChangeTracker changeTracker)
        {
            foreach (var entry in changeTracker.Entries<IUpdatable>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.UpdateCreation();
                    //entry.CurrentValues[_createTimePropertyName] = DateTimeOffset.Now;
                }
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdateModification();
                    //entry.CurrentValues[_lastModificationTimePropertyName] = DateTimeOffset.Now;
                }
            }
        }
    }
}
