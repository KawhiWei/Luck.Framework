using Luck.Framework.Infrastructure.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luck.Redis.StackExchange
{
    public partial class  StackExchangeRedisCache:ICache
    {
        public Task<bool> SetStringAsync(string key, string cacheValue, TimeSpan? expiration)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetStringAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<long> GetKeyLengAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<long> SetStringRangeAsync(string key, long offest, string value)
        {
            throw new NotImplementedException();
        }

        public Task<string> StringGetRangeAsync(string key, long start, long end)
        {
            throw new NotImplementedException();
        }
    }
}
