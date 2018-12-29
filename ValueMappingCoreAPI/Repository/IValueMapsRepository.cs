using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValueMappingCoreAPI.Models;

namespace ValueMappingCoreAPI.Repository
{
    public interface IValueMapsRepository//操作集合存储库的接口
    {
        Task<ValueMaps> GetByIdAsync(int id);
        Task<List<ValueMaps>> ListAsync();
        Task AddAsync(ValueMaps vm);
        Task DeleteAsync(int id);
        Task UpdateAsync(ValueMaps vm);
        //Task PublishAsync(ValueMaps vm);
        //Task PublishBatchAsync(List<ValueMaps> vm);
        Tuple<List<ValueMaps>, int> PageList(int pageindex, int pagesize);
    }
}
