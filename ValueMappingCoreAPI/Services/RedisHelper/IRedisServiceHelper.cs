using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueMappingCoreAPI.Services.RedisHelper
{
   public interface IRedisServiceHelper
    {
        Task<string> GetAsync(string key);
        Task SetAsync(string key,string value);
    }
}
