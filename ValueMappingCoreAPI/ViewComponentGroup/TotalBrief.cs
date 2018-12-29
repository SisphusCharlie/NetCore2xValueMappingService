using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValueMappingCoreAPI.Data;

namespace ValueMappingCoreAPI.ViewComponentGroup
{
    [ViewComponent(Name ="TotalBriefCNT")]
    public class TotalBrief:ViewComponent
    {
        private readonly TradeDbContext context;
        private IMemoryCache memoryCache;
        private string cachekey = "TotalBrief";
        public TotalBrief(TradeDbContext ct, IMemoryCache mCache)
        {
            context = ct;
            memoryCache = mCache;
        }

        public IViewComponentResult Invoke(int days)
        {
            Dictionary<string, int> total = new Dictionary<string, int>();
            if (!memoryCache.TryGetValue(cachekey,out total))
            {
                total = GetSummary(days);
                memoryCache.Set(cachekey,total,TimeSpan.FromSeconds(30));
            }
            return View(total);
        }
        private Dictionary<string, int> GetSummary(int days)
        {
            Dictionary<string, int> totalone = new Dictionary<string, int>();
            var query = from q in context.ValueMaps.Where(x => x.TransdateTime > DateTime.Now.AddDays(-days))
                        group q by q.ValuationFunction into grouped
                        select new
                        {
                            VF = grouped.Key,
                            CNT = grouped.Count()
                        } into step2
                        orderby step2.CNT descending
                        select step2;
            foreach(var item in query)
            {
                totalone.Add(item.VF, item.CNT);
            }
            return totalone;
        }
    }
}
