using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueMappingCoreAPI.Data;
using ValueMappingCoreAPI.Extensions;
using ValueMappingCoreAPI.Models;
using ValueMappingCoreAPI.Services;
using static ValueMappingCoreAPI.Repository.ValueMapsRepository;

namespace ValueMappingCoreAPI.Repository
{
    public class ValueMapsRepository : IValueMapsRepository
        //, ISubscriberService, ICapSubscribe
    {
        private TradeDbContext context;
        //private readonly ICapPublisher _publisher;
        public ValueMapsRepository(TradeDbContext ct)
        {
            context = ct;
        }
        public async Task AddAsync(ValueMaps vm)
        {
            IDbContextTransaction tx = null;
            try
            {
                using (tx = await context.Database.BeginTransactionAsync())
                {
                    context.ValueMaps.Add(vm);
                    await context.SaveChangesAsync();
                    tx.Commit();
                }
            }
            catch (DbUpdateException ex)
            {
                tx.Rollback();
            }
        }

        public Task DeleteAsync(int id)
        {
            var de = GetByIdAsync(id);
            context.Entry(de.Result).State = EntityState.Deleted;
            return context.SaveChangesAsync();
        }

        public Task<ValueMaps> GetByIdAsync(int id)
        {
            return context.ValueMaps.Include(System => System.System).FirstOrDefaultAsync(r => r.Id == id);
        }

        public Task<List<ValueMaps>> ListAsync()
        {
            return context.ValueMaps.Include(System => System.System).ToListAsync();
        }

        public Tuple<List<ValueMaps>, int> PageList(int pageindex, int pagesize)
        {
            var query = context.ValueMaps
            .Include(System => System.System).AsQueryable();
            var count = query.Count();
            var pagecount = count % pagesize == 0 ? count / pagesize : count / pagesize + 1;
            var vms = query.OrderByDescending(r => r.Id)
            .Skip((pageindex - 1) * pagesize)
            .Take(pagesize)
            .ToList();
            return new Tuple<List<ValueMaps>, int>(vms, pagecount);
        }

        //public async Task PublishAsync(ValueMaps vm)
        //{

        //    using (var trans = context.Database.BeginTransaction())
        //    {
        //        //指定发送的消息标题（供订阅）和内容
        //        await _publisher.PublishAsync("xxx.services.account.check", vm);
        //        // 你的业务代码。
        //        trans.Commit();
        //    }
        //    //return Ok();
        //}

        //public async Task PublishBatchAsync(List<ValueMaps> vm)
        //{
        //    using (var trans = context.Database.BeginTransaction())
        //    {
        //        //指定发送的消息标题（供订阅）和内容
        //        await _publisher.PublishAsync("xxx.services.account.check", vm);
        //        // 你的业务代码。
        //        trans.Commit();
        //    }
        //}
        //public interface ISubscriberService
        //{
        //    Task CheckReceivedMessage(ValueMaps VM);
        //}



        public Task UpdateAsync(ValueMaps vm)
        {
            context.Entry(vm).State = EntityState.Modified;
            return context.SaveChangesAsync();
        }
        //[CapSubscribe("xxx.services.account.check")]
        //public async Task CheckReceivedMessage(ValueMaps VM)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine(VM.SetJSONString());
        //    await mailsender.SendEmailAsync(VM.Uid, "xxx.services.account.check.Added one record!", sb.ToString());
        //}
    }
}
