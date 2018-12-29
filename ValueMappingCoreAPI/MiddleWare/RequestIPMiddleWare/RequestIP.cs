using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueMappingCoreAPI.MiddleWare.RequestIPMiddleWare
{

    public class RequestIPMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;
        public RequestIPMiddleware(RequestDelegate innext, ILoggerFactory loggerfactory)
        {
            next = innext;
            logger = loggerfactory.CreateLogger<RequestIPMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            logger.LogInformation("User IP:" + context.Connection.RemoteIpAddress.ToString());
            await next.Invoke(context);
        }
    }
}
