using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueMappingCoreAPI.Services.CacheHelper
{
    public class MemoryCacheHelper : ICacheHelper
    {
        public MemoryCacheHelper(/*MemoryCacheOptions options*/)//这里可以做成依赖注入，但没打算做成通用类库，所以直接把选项直接封在帮助类里边
        {
            //this._cache = new MemoryCache(options);
            this._cache = new MemoryCache(new MemoryCacheOptions());
        }


        private IMemoryCache _cache;


        public bool Exists(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));


            object v = null;
            return this._cache.TryGetValue<object>(key, out v);
        }


        public T GetCache<T>(string key) where T : class
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));


            T v = null;
            this._cache.TryGetValue<T>(key, out v);


            return v;
        }


        public void SetCache(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));


            object v = null;
            if (this._cache.TryGetValue(key, out v))
                this._cache.Remove(key);
            this._cache.Set<object>(key, value);
        }


        public void SetCache(string key, object value, double expirationMinute)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));


            object v = null;
            if (this._cache.TryGetValue(key, out v))
                this._cache.Remove(key);
            DateTime now = DateTime.Now;
            TimeSpan ts = now.AddMinutes(expirationMinute) - now;
            this._cache.Set<object>(key, value, ts);
        }


        public void SetCache(string key, object value, DateTimeOffset expirationTime)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));


            object v = null;
            if (this._cache.TryGetValue(key, out v))
                this._cache.Remove(key);


            this._cache.Set<object>(key, value, expirationTime);
        }


        public void RemoveCache(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));


            this._cache.Remove(key);
        }


        public void Dispose()
        {
            if (_cache != null)
                _cache.Dispose();
            GC.SuppressFinalize(this);
        }
    }

}