using Luck.Framework.Domian;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using System.Reflection;

namespace Luck.EntityFrameworkCore.Extensions
{
    public static class SoftDeletableExtension
    {
        private const string _softDeletedPropertyName = "Deleted";

#pragma warning disable CS8602 // 解引用可能出现空引用。
        private static readonly MethodInfo _propertyMethod = typeof(EF).GetMethod(nameof(EF.Property)).MakeGenericMethod(typeof(bool));
#pragma warning restore CS8602 // 解引用可能出现空引用。


        public static void UseDeletion(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                //值对象或子类。跳过SoftDeletable逻辑
                if (entityType.IsOwned() || entityType.BaseType != null)
                {
                    continue;
                }

                if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddProperty<bool>(_softDeletedPropertyName);
                    //entityType.AddProperty<DateTimeOffset?>(_deletionTimePropertyName);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(GetIsDeletedRestriction(entityType.ClrType));
                }
            }
        }

        public static void UpdateDeletion(this ChangeTracker changeTracker)
        {
            foreach (var entry in changeTracker.Entries<ISoftDeletable>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues[_softDeletedPropertyName] = false;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues[_softDeletedPropertyName] = true;

                        entry.Entity.UpdateDeletion();
                        //entry.CurrentValues[_deletionTimePropertyName] = DateTimeOffset.Now;

                        //值对象的删除判断
                        if (entry.References != null)
                        {
                            foreach (var e in entry.References)
                            {
                                if (e.TargetEntry != null && e.TargetEntry.Metadata.IsOwned())
                                    e.TargetEntry.State = EntityState.Modified;
                            }
                        }
                        break;
                }
            }
        }

        private static LambdaExpression GetIsDeletedRestriction(Type type)
        {
            var parm = Expression.Parameter(type, "it");
            // EF.Property<bool>(post, "Deleted")
            var prop = Expression.Call(_propertyMethod, parm, Expression.Constant(_softDeletedPropertyName));

            // EF.Property<bool>(post, "Deleted") == false
            var condition = Expression.MakeBinary(ExpressionType.Equal, prop, Expression.Constant(false));

            // post => EF.Property<bool>(post, "Deleted") == false
            var lambda = Expression.Lambda(condition, parm);

            return lambda;
        }
    }
}
