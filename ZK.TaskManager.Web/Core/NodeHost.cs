using Consul;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using ZK.TaskManager.Core;
using ZK.TaskManager.Web.Models;

namespace ZK.TaskManager.Web.Core
{
    public class NodeHost
    {
        private static object synObj = new object();
        private static NodeHost _host;
        public static NodeHost Host
        {
            get
            {
                if (_host == null)
                {
                    lock (synObj)
                    {
                        if (_host == null)
                        {
                            _host = new NodeHost();
                        }
                    }
                }
                return _host;
            }
        }
        private ConsulClient client;
        public NodeHost()
        {
            var ConsulAddress = ConfigurationManager.AppSettings["ConsulUrl"];
            Environment.SetEnvironmentVariable("CONSUL_HTTP_ADDR", ConsulAddress);
            client = new ConsulClient();
        }

        public List<ServicesViewModel> GetNodeList()
        {
            var list = new List<ServicesViewModel>();
            try
            {
                var services = client.Agent.Services();
                var checks = client.Agent.Checks().Result.Response;
                foreach (var item in services.Result.Response)
                {
                    var svcID = item.Value.ID;
                    if (item.Value.Service == "taskservice" && checks.ContainsKey("service:" + svcID))
                    {
                        var service = new ServicesViewModel();
                        service.Node = checks["service:" + svcID].Node;
                        service.ServiceId = svcID;
                        service.ServiceName = item.Value.Service;
                        service.ServiceAddress = $"http://{item.Value.Address}:{item.Value.Port}";
                        service.ServiceIP = $"{item.Value.Address}:{item.Value.Port}";
                        service.ServiceState = checks["service:" + svcID].Status.Status;
                        list.Add(service);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.SysLog("Consul GetServicesList", ex);
            }
            return list;
        }

        public bool UnRegisterService(string sid)
        {
            try
            {
                return client.Agent.ServiceDeregister(sid).Result.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Log.SysLog("Consul UnRegisterService", ex);
                return false;
            }
          
        }

    }
}