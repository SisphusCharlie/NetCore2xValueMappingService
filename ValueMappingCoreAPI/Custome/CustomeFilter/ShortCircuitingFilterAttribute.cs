using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueMappingCoreAPI.CustomeFilter
{
    public class ShortCircuitingFilterAttribute : Attribute, IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext context)
        {

        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            //context.Result = new ContentResult()
            //{
            //    Content = "Resource unavailable"
            //};
            //context.Result = null;//null值不会短路，赋值才会截断
        }
    }
}
