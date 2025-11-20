using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Luck.Framework;
using Luck.MongoDB;
using Luck.MongoDB.DbContexts;
using Xunit;

namespace Luck.UnitTest
{
    public class MongoDbTest
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TestMongoDbContext _dbContext;

        public MongoDbTest()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddBusinessServices();
            services.AddMongoDbContext<TestMongoDbContext>(options =>
            {
                options.ConnectionString = "mongodb://localhost:27017/LuckTest";
            });
            BsonSerializer.RegisterSerializer(new DateTimeSerializer(DateTimeKind.Local));
            _serviceProvider = services.BuildServiceProvider();
            _dbContext = _serviceProvider.GetService<TestMongoDbContext>()!;
        }

        [Fact]
        public async Task add_user_async()
        {
            var provider = _serviceProvider.GetKeyedService<IAncillaryPaySuccessWithAncillaryScopeProvider>("70");
            if (provider is null)
            {
                Assert.NotNull(provider);
            }

            await provider.AncillaryPaySuccessProviderAsync("", "");
            var user = new User
            {
                Name = "大黄瓜18CM",
                Age = 18,
                IsLock = false,
                Sex = Sex.Male,
                DateTime = DateTime.UtcNow
            };
            await _dbContext.Collection<User>().InsertOneAsync(user);

            Assert.NotEqual(user.Id, string.Empty);
            var bf = Builders<User>.Filter.Eq(o => o.Id, user.Id);
            var find = await _dbContext.Collection<User>().FindAsync(bf);
            var findUser = find.FirstOrDefault();
            Assert.NotNull(findUser);
        }

        [Fact]
        public async Task get_user_async()
        {
            var user = await (await _dbContext.Collection<User>().FindAsync(Builders<User>.Filter.Empty))
                .FirstOrDefaultAsync();

            Assert.NotNull(user);
        }
    }

    public sealed class TestMongoDbContext : MongoDbContextBase
    {
        public TestMongoDbContext([NotNull] MongoContextOptions options) : base(options)
        {
        }
    }

    public class User
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }

        public bool IsLock { get; set; }

        public Sex Sex { get; set; } = Sex.Unknown;

        public DateTime DateTime { get; set; }
    }


    public enum Sex
    {
        Unknown, //中性
        Male, //男
        Female //女
    }
}