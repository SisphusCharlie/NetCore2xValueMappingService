using Microsoft.Extensions.Caching.Redis;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueMappingCoreAPI.Services.CacheHelper
{
    public class RedisCacheHelper : ICacheHelper
    {
        public RedisCacheHelper(/*RedisCacheOptions options, int database = 0*/)//这里可以做成依赖注入，但没打算做成通用类库，所以直接把连接信息直接写在帮助类里
        {
            RedisCacheOptions options = new RedisCacheOptions();
            options.Configuration = "127.0.0.1:6379";
            options.InstanceName = "test";
            int database = 0;
            _connection = ConnectionMultiplexer.Connect(options.Configuration);
            _cache = _connection.GetDatabase(database);
            _instanceName = options.InstanceName;
        }


        private IDatabase _cache;


        private ConnectionMultiplexer _connection;


        private readonly string _instanceName;


        private string GetKeyForRedis(string key)
        {
            return _instanceName + key;
        }


        public bool Exists(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));


            return _cache.KeyExists(GetKeyForRedis(key));
        }


        public T GetCache<T>(string key) where T : class
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));


            var value = _cache.StringGet(GetKeyForRedis(key));
            if (!value.HasValue)
                return default(T);


            return JsonConvert.DeserializeObject<T>(value);
        }


        public void SetCache(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));


            if (Exists(GetKeyForRedis(key)))
                RemoveCache(GetKeyForRedis(key));


            _cache.StringSet(GetKeyForRedis(key), JsonConvert.SerializeObject(value));
        }


        public void SetCache(string key, object value, DateTimeOffset expiressAbsoulte)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));


            if (Exists(GetKeyForRedis(key)))
                RemoveCache(GetKeyForRedis(key));


            _cache.StringSet(GetKeyForRedis(key), JsonConvert.SerializeObject(value), expiressAbsoulte - DateTime.Now);
        }


        //public void SetCache(string key, object value, double expirationMinute)
        //{
        //    if (Exists(GetKeyForRedis(key)))
        //        RemoveCache(GetKeyForRedis(key));


        //    DateTime now = DateTime.Now;
        //    TimeSpan ts = now.AddMinutes(expirationMinute) - now;
        //    _cache.StringSet(GetKeyForRedis(key), JsonConvert.SerializeObject(value), ts);
        //}


        public void RemoveCache(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));


            _cache.KeyDelete(GetKeyForRedis(key));
        }


        public void Dispose()
        {
            if (_connection != null)
                _connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}