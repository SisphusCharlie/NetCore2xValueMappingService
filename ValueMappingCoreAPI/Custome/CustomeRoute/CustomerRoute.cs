using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueMappingCoreAPI.Custome.CustomeRoute
{
    public class CustomerRoute : IRouteTemplateProvider
    {
        public string Template => "Api/[controller]";

        public int? Order { get; set; }

        public string Name { get; set; }
    }
}
