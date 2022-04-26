using Luck.Framework.Extensions;
using Luck.Framework.Infrastructure.Caching;
using StackExchange.Redis;
using System.Net;

namespace Luck.Redis.StackExchange
{
    public partial class StackExchangeRedisHash : IRedisHash
    {

        private readonly ConnectionMultiplexer _connectionMultiplexer;

        private readonly EndPoint _endPoint;

        private readonly IDatabase _database;

        public StackExchangeRedisHash(ConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _database = _connectionMultiplexer.GetDatabase();
            _endPoint = _connectionMultiplexer.GetEndPoints().First();
        }

    }
}