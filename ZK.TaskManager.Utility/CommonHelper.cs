using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.Utility
{
    public class CommonHelper
    {
        public static string GetIP()        
        {
            var HostName = Dns.GetHostName(); //得到主机名 
            var IpEntry = Dns.GetHostEntry(HostName); //得到主机IP
            var strIPAddr = string.Empty;
            foreach (var t in IpEntry.AddressList)
            {
                if (t.AddressFamily == AddressFamily.InterNetwork)
                {
                    strIPAddr = t.ToString();
                }
            }
            return (strIPAddr);
        }
    }
}
