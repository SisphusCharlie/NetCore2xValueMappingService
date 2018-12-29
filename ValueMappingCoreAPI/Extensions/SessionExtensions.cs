using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ValueMappingCoreAPI.Extensions
{
    public static class SessionExtensions
    {
        //    public static void Set(this ISession session, string key, object value)
        //    {
        //        session.SetString(key, JsonConvert.SerializeObject(value));
        //    }

        //public static T Get<T>(this ISession session, string key)
        //{
        //    var value = session.GetString(key);

        //    return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        //}


        //public static T GetProtoBuf<T>(this ISession session, string key) where T : class
        //{
        //    byte[] byteArray = null;
        //    if (session.TryGetValue(key, out byteArray))
        //    {
        //        using (var memoryStream = new MemoryStream(byteArray))
        //        {
        //            var obj = ProtoBuf.Serializer.Deserialize<T>(memoryStream);
        //            return obj;
        //        }
        //    }
        //    return null;
        //}

        //public static void Set<T>(this ISession session, string key, T value) 
        //{
        //    try
        //    {
        //        //using (var memoryStream = new MemoryStream())
        //        //{
        //        //    ProtoBuf.Serializer.Serialize(memoryStream, value);
        //        //    byte[] byteArray = memoryStream.ToArray();
        //        //    session.Set(key, byteArray);
        //        //}
        //        session.SetString(key, JsonConvert.SerializeObject(value));
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}


    }
}
