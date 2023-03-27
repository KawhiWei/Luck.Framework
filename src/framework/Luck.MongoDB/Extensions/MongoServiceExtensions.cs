using Luck.MongoDB;
using Luck.MongoDB.DbContexts;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using StringObjectIdIdGeneratorConvention = MongoDB.Bson.Serialization.Conventions.StringObjectIdIdGeneratorConvention;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MongoServiceExtensions
    {
        private const string Pack = "mongo:pack";

        /// <summary>
        /// 添加Mongo上下文
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddMongoDbContext<TContext>(this IServiceCollection serviceCollection, Action<MongoContextOptions>? options)
            where TContext : MongoDbContextBase
        {
            var options1 = new MongoContextOptions();
            options?.Invoke(options1);
            serviceCollection.AddSingleton(options1);

            serviceCollection.TryAdd(new ServiceDescriptor(typeof(MongoDbContextBase), typeof(TContext), ServiceLifetime.Singleton));
            if (typeof(MongoDbContextBase) != typeof(TContext))
            {
                serviceCollection.TryAdd(new ServiceDescriptor(typeof(TContext), (IServiceProvider p) => p.GetService<MongoDbContextBase>()!, ServiceLifetime.Singleton));
            }

            //serviceCollection.AddSingleton<MongoDbContextBase, TContext>();
            ConventionRegistry.Register($"{Pack}:{ObjectId.GenerateNewId()}", new ConventionPack
            {
                new CamelCaseElementNameConvention(), // 驼峰名称格式
                new IgnoreExtraElementsConvention(true), // 忽略掉实体中不存在的字段
                new NamedIdMemberConvention("Id", "ID"), // _id映射为实体中的ID或者Id
                new StringObjectIdIdGeneratorConvention() //ObjectId → String mapping ObjectId
            }, o => true);
            return serviceCollection;
        }
    }
}