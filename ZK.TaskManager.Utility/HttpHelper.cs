using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.Utility
{
    public class HttpHelper
    {
        public static string Get(string url)
        {

            try
            {
                var result = "";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "get";
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        result = sr.ReadToEnd();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static string Post(string url, string postData = null)
        {
            try
            {
                var result = "";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "post";
                if (!string.IsNullOrEmpty(postData))
                {
                    var param = postData;
                    byte[] bs = Encoding.UTF8.GetBytes(param);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = bs.Length;
                    using (Stream reqStream = request.GetRequestStream())
                    {
                        reqStream.Write(bs, 0, bs.Length);
                    }
                }
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        result = sr.ReadToEnd();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

    }
}
