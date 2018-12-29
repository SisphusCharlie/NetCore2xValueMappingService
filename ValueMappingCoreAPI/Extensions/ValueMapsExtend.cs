using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ValueMappingCoreAPI.Models;

namespace ValueMappingCoreAPI.Extensions
{
    public static class ValueMapsExtend
        //<T> where T:class,new()
    {

        public static string SetJSONString(this ValueMaps input)
        {
            return JsonConvert.SerializeObject(input);
        }



        public static T GetOriginal<T>(this ValueMaps input,byte[] byteArray) where T : class
        {
                using (var memoryStream = new MemoryStream(byteArray))
                {
                    var obj = ProtoBuf.Serializer.Deserialize<T>(memoryStream);
                    return obj;
                }

        }

        public static byte[] SetBinary<T>(this ValueMaps input) where T:class,new()
        {
            byte[] byteArray = null;
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    ProtoBuf.Serializer.Serialize(memoryStream, input);
                    byteArray = memoryStream.ToArray();
                    return byteArray;
                }
            }
            catch (Exception)
            {
                return byteArray;
            }

        }
    }
}
