using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueMappingCoreAPI.Models
{
    [Serializable]
    public class ValueMaps
    {
        public int Id { get; set; }
        public int SystemId { get; set; }

        public OperateSystem System { get; set; }
        //[Remote(action:"VerifyVF",controller:"ValueMaps")]
        public string ValuationFunction { get; set; }
        public double? Threshold { get; set; }
        public string Uid { get; set; }
        public DateTime? TransdateTime { get; set; }
    }
}
