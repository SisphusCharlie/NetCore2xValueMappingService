using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueMappingCoreAPI.Services.CacheHelper
{

    public class CacheUntity
    {
        private static ICacheHelper _cache = new RedisCacheHelper();//默认使用Redis


        private static bool isInited = false;
        public static void Init(ICacheHelper cache)
        {
            if (isInited)
                return;
            _cache.Dispose();
            _cache = cache;
            isInited = true;
        }


        public static bool Exists(string key)
        {
            return _cache.Exists(key);
        }


        public static T GetCache<T>(string key) where T : class
        {
            return _cache.GetCache<T>(key);
        }


        public static void SetCache(string key, object value)
        {
            _cache.SetCache(key, value);
        }


        public static void SetCache(string key, object value, DateTimeOffset expiressAbsoulte)
        {
            _cache.SetCache(key, value, expiressAbsoulte);
        }


        //public void SetCache(string key, object value, double expirationMinute)
        //{


        //}


        public static void RemoveCache(string key)
        {
            _cache.RemoveCache(key);
        }


    }
}
