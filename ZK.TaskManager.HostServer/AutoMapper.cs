using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZK.TaskManager.Core.Models;
using ZK.TaskManager.HostServer.Model;

namespace ZK.TaskManager.HostServer
{
    public class AutoMapper
    {
        public static IMapper Mapper;
        public static void Init()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<JobModel, JobViewModel>();
            });
            Mapper = config.CreateMapper();
        }
    }
}
