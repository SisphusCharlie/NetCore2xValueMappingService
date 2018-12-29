using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using ValueMappingCoreAPI.Models;

namespace ValueMappingCoreAPI.Extensions
{
    public static class SystemExtend
    {
        //public static string SetJSONString(this OperateSystem input)
        //{
        //    return JsonConvert.SerializeObject(input);
        //}
        public static byte[] SerializeToBuffer<T>(T obj)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                stream.Flush();
                stream.Close();
                return buffer;
            }
            catch (Exception ex)
            {
                throw new Exception("序列化失败,原因:" + ex.Message);
            }
        }

        public static T DesrializeFromBuffer<T>(T obj, byte[] buffer)
        {
            try
            {
                obj = default(T);
                IFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream(buffer);
                obj = (T)formatter.Deserialize(stream);
                stream.Flush();
                stream.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("反序列化失败,原因:" + ex.Message);
            }
            return obj;
        }
    }
}
