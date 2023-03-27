using MongoDB.Driver;

namespace Luck.MongoDB.DbContexts
{
    public interface IMongoDbContext
    {

        IMongoDatabase Database { get; }

        IMongoClient MongoClient { get; }

        IMongoCollection<TEntity> Collection<TEntity>();


    }
}
