using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ValueMappingCoreAPI.Custome.ExceptionHandler
{
    public class ExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var error = new
            {
                message = exception.Message
            };
            var result = new JsonResult(error);
            result.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Result = result;
        }
    }
}
