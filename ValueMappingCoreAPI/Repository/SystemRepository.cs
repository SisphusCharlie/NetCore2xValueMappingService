using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValueMappingCoreAPI.Data;
using ValueMappingCoreAPI.Models;

namespace ValueMappingCoreAPI.Repository
{
    public class SystemRepository : ISystemRepository
    {
        private TradeDbContext context;
        public SystemRepository(TradeDbContext _context)
        {
            context = _context;
        }

        public Task<List<OperateSystem>> ListAsync()
        {
            return context.OperateSystem.ToListAsync();
        }
        public OperateSystem FindById(int id)
        {
            return context.OperateSystem.Where(x => x.Id == id).First();
        }

    }
}
