using Luck.Framework.Extensions;
using Luck.Framework.Utilities;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;

namespace Luck.Mongo.DbContexts
{
    public abstract class MongoDbContextBase : IMongoDbContext
    {


        private readonly MongoContextOptions _options;

        protected const string Default_Database_Name = "Luck";

        public MongoDbContextBase([NotNull] MongoContextOptions options)
        {
            _options = options;
            _connectionString = Check.NotNullOrEmpty(_options.ConnectionString, nameof(options.ConnectionString));
        }

        private string _connectionString;

        public virtual IMongoDatabase Database => GetDbContext();

        public virtual IMongoClient MongoClient => Database.Client;

        public virtual IMongoCollection<TEntity> Collection<TEntity>()
        {
            return Database.GetCollection<TEntity>(GetTableName<TEntity>());
        }

        //public virtual IMongoCollection<TEntity> Collection<TEntity>(string tableName)
        //{
        //    return Database.GetCollection<TEntity>(tableName);
        //}

        protected virtual IMongoDatabase GetDbContext()
        {
            var mongoUrl = new MongoUrl(_connectionString);
            var databaseName = mongoUrl.DatabaseName;
            if (databaseName.IsNullOrWhiteSpace())
            {
                databaseName = Default_Database_Name;
            }
            var database = new MongoClient(mongoUrl).GetDatabase(databaseName);
            return database;
        }



        protected virtual string GetTableName<TEntity>()
        {

            Type t = typeof(TEntity);
            return t.Name.ToLower();
        }


    }
}
