using System.Linq.Expressions;
using Luck.DDD.Domain;
using Luck.DDD.Domain.Repositories;
using Luck.EntityFrameworkCore.DbContexts;
using Luck.Framework.Exceptions;
using MongoDB.Driver;

namespace Luck.MongoDB.Repositories;

public class MongoDbAggregateRootRepository<TEntity, TKey> : AggregateRootRepositoryBase<TEntity, TKey> where TEntity : class, IAggregateRootBase
    where TKey : IEquatable<TKey>
{
    private readonly LuckMongoDbContextBase _dbContext;


    public MongoDbAggregateRootRepository(LuckMongoDbContextBase dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Attach(TEntity entity)
    {
        throw new LuckException("无需实现");
    }

    public override TEntity? Find(TKey primaryKey)
    {
        throw new NotImplementedException();
    }

    public override ValueTask<TEntity?> FindAsync(TKey primaryKey)
    {
        throw new LuckException("暂未实现");
    }

    public override Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        throw new LuckException("暂未实现");
    }

    protected override IQueryable<TEntity> FindQueryable()
    {
        return _dbContext.Collection<TEntity>().AsQueryable();
    }

    public override void Add(TEntity entity)
    {
        _dbContext.Collection<TEntity>().InsertOne(entity);
    }

    public override void Update(TEntity entity)
    {
        _dbContext.Collection<TEntity>().InsertOne(entity);
    }

    public override void Remove(TEntity entity)
    {
        throw new NotImplementedException();
    }
}