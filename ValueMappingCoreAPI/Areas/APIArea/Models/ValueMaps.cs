using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueMappingCoreAPI.Areas.APIArea.Models
{
    [ProtoContract]
    public class ValueMaps
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string System { get; set; }
        [ProtoMember(3)]
        public string ValuationFunction { get; set; }
        [ProtoMember(4)]
        public double? Threshold { get; set; }
        [ProtoMember(5)]
        public string Uid { get; set; }
        [ProtoMember(6)]
        public DateTime? TransdateTime { get; set; }
    }
}
