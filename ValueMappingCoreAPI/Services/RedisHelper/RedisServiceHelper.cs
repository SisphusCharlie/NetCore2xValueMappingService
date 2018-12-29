using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValueMappingCoreAPI.Services.RedisHelper;

namespace ValueMappingCoreAPI.Services
{
    public class RedisServiceHelper : IRedisServiceHelper
    {
        private readonly IDatabase _database;

        public RedisServiceHelper(IDatabase database)
        {
            _database = database;
        }

        public async Task<string> GetAsync(string key)
        {
            return await _database.StringGetAsync(key); 
        }

        public async Task SetAsync(string key,string value)
        {
           await _database.StringSetAsync(key, value);
        }
    }
}
