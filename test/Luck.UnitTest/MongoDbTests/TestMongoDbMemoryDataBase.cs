using System;
using System.Threading.Tasks;
using Luck.TestBase;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Luck.UnitTest.MongoDbTests;

public class TestMongoDbMemoryDataBase : IntegratedTest<MongoDbTestModule>
{
    
    [Fact]
    public  async Task Connection_Test_Open()
    {
        var id = Guid.NewGuid().ToString();
        var testMongoDbMemoryDataBaseContext = ServiceProvider.GetService<TestMongoDbMemoryDataBaseContext>();
        if (testMongoDbMemoryDataBaseContext is null)
            throw new ArgumentNullException(nameof(testMongoDbMemoryDataBaseContext));
        await  testMongoDbMemoryDataBaseContext.Orders.InsertOneAsync(new Order()
        {
            Id = id,
            Name = "A002",
            Number = 10,
        });
        var result = await testMongoDbMemoryDataBaseContext.Orders.Find(x => x.Id == id).SingleOrDefaultAsync();
    }
    
}