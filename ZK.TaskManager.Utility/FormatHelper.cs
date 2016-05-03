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

    }
}
