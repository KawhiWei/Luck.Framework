using JetBrains.Annotations;
using Luck.MongoDB;
using Luck.MongoDB.DbContexts;
using MongoDB.Driver;

namespace Luck.UnitTest.MongoDbTests;

public class TestMongoDbMemoryDataBaseContext : MongoDbContextBase
{
    public TestMongoDbMemoryDataBaseContext([NotNull] MongoContextOptions options) : base(options)
    {

    }
    /// <summary>
    /// MongoTest
    /// </summary>
    public IMongoCollection<Order> Orders => Collection<Order>("orders");
    
    
}