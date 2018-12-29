using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueMappingCoreAPI.MiddleWare
{
    public class AntiForgery
    {
        private readonly RequestDelegate _next;


        public AntiForgery(RequestDelegate ext)
        {
            _next=ext;
        }

        public  async Task Invoke(HttpContext hc)
        {
            hc.Response.Headers.Add("Content-Security-Policy","default-src 'self'");
            await _next.Invoke(hc);
        }

    }
}
