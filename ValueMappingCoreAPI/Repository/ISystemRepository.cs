using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValueMappingCoreAPI.Models;

namespace ValueMappingCoreAPI.Repository
{
    public interface ISystemRepository
    {
        Task<List<OperateSystem>> ListAsync();
        OperateSystem FindById(int id);
    }
}
