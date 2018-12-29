using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueMappingCoreAPI.Models
{
    [Serializable]
    public class OperateSystem
    {
  
        public int Id { get; set; }

        public string SystemName { get; set; }

        public string Uid { get; set; }
 
        public DateTime? TransdateTime { get; set; }


        public List<ValueMaps> wms { get; set; }
    }
}
