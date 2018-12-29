using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueMappingCoreAPI.Services
{
    public class RedisService
    {
        static IDatabase redis;
        static object lobject = new object();
        readonly string connection = "127.0.0.1:6379,abortConnect = false,connectRetry = 3,connectTimeout = 3000,defaultDatabase = 1,syncTimeout = 3000,version = 3.2.1,responseTimeout = 3000";

        public RedisService()
        {
            if (redis == null)
            {
                lock (lobject)
                {
                    if (redis == null)
                    {
                        ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(connection);
                        redis = connectionMultiplexer.GetDatabase();
                    }
                }
            }

        }

        public IDatabase Redis { get { return redis; } }
    }
}
