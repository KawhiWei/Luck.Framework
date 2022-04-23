﻿using Luck.DDD.Domain;
using Luck.DDD.Domain.Repositories;
using Luck.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Luck.EntityFrameworkCore.Repositories
{
    public class EFCoreEntityRepository<TEntity, TKey> : IEntityRepository<TEntity, TKey> where TEntity : class, IEntityWithIdentity where TKey : IEquatable<TKey>
    {

        private readonly LuckDbContextBase _dbContext;

        public LuckDbContextBase DbContext => _dbContext;

        public EFCoreEntityRepository(ILuckDbContext dbContext)
        {
            _dbContext = dbContext as LuckDbContextBase ?? throw new NotSupportedException();
        }

        public TEntity? Find(TKey primaryKey)
        {
            return _dbContext.Find<TEntity>(primaryKey);
        }

        public IQueryable<TEntity> FindAll()
        {
            return _dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return FindAll().Where(predicate);
        }

        public async Task<TEntity?> FindAsync(TKey primaryKey)
        {
            return await _dbContext.FindAsync<TEntity>(primaryKey);
        }

        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }
    }
}
