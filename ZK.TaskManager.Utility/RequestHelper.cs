using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ZK.TaskManager.Utility
{
    public class RequestHelper
    {
        public static int GetInt(string keyName)
        {
            var val = GetValue(keyName);

            if (string.IsNullOrEmpty(val))
            {
                return 0;
            }

            var ret = 0;

            int.TryParse(val, out ret);

            return ret;
        }

        public static decimal GetDecimal(string keyName)
        {
            var val = GetValue(keyName);

            if (string.IsNullOrEmpty(val))
            {
                return 0;
            }

            var ret = 0M;

            decimal.TryParse(val, out ret);

            return ret;
        }

        public static bool GetBool(string keyName)
        {
            var val = GetValue(keyName);

            if (string.IsNullOrEmpty(val))
            {
                return false;
            }

            var ret = false;

            bool.TryParse(val, out ret);

            return ret;
        }


        public static DateTime GetDateTime(string keyName)
        {
            var val = GetValue(keyName);

            if (string.IsNullOrEmpty(val))
            {
                return new DateTime(1753, 1, 1);
            }

            DateTime ret;

            if (!DateTime.TryParse(val, out ret))
            {
                ret = new DateTime(1753, 1, 1);
            }

            return ret;
        }

        public static string GetValue(string keyName, bool needUrlDecode = false)
        {
            var val = HttpContext.Current.Request[keyName];

            if (!string.IsNullOrEmpty(val))
            {
                if (needUrlDecode)
                {
                    val = HttpUtility.UrlDecode(val.Trim());
                }
                else
                {
                    val = val.Trim();
                }

                // return HttpUtility.HtmlEncode(val);
                return val;
            }

            return val;
        }

        public static string GetRequestIp()
        {
            var ip = string.Empty;

            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"]))
            {
                ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
            }
            if (string.IsNullOrEmpty(ip))
            {
                ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            }

            if (ip == "::1")
            {
                ip = "127.0.0.1";
            }

            return ip;
        }


    }
}
