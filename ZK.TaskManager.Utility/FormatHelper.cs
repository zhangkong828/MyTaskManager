using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.Utility
{
    public class FormatHelper
    {

        public static byte[] StringToBytes(string str)
        {
            return StringToBytes(str, Encoding.UTF8);
        }
        public static byte[] StringToBytes(string str, Encoding encoding)
        {
            return encoding.GetBytes(str);
        }

        public static string BytesToString(byte[] bytes)
        {
            return BytesToString(bytes, Encoding.UTF8);
        }
        public static string BytesToString(byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes);
        }

        #region Json 序列化 反序列化
        /// <summary>
        /// Json 序列化
        /// </summary>
        public static string JsonSerializer(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Json 反序列化
        /// </summary>
        public static T JsonDeserializer<T>(string jsonstr)
        {
            if (string.IsNullOrEmpty(jsonstr))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(jsonstr);
        }
        #endregion
    }
}
