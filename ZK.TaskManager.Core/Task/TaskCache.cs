using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZK.TaskManager.Core.Models;
using System.IO;
using ZK.TaskManager.Utility;

namespace ZK.TaskManager.Core.Task
{
   public  class TaskCache
    {
       public static  Dictionary<string,TaskModel> taskCache;
        public static void Init()
        {

        }

        /// <summary>
        /// 加载本地任务
        /// </summary>
        public static void LoadTask()
        {
            //本地的dll目录
            var taskDlls = Directory.GetDirectories(GlobalConfig.TaskPluginDir);
            foreach (var item in taskDlls)
            {
                var package = Path.Combine(item, "package.json");
                if (!File.Exists(package))
                {
                    Console.WriteLine("该插件缺少package.json文件，无法加载");
                    continue;
                }
                var packagejson = File.ReadAllText(package);
                var packageModel=FormatHelper.JsonDeserializer<PackageModel>(packagejson);
                if (packageModel == null)
                {
                    Console.WriteLine("该插件package.json文件格式不正确，无法解析");
                    continue;
                }

            }
        }
        /// <summary>
        /// 检查本地是否存在任务dll, 
        /// 不存在 则下载
        /// </summary>
        public static void CheckFiles(List<TaskModel> tasklist)
        {
            var needToDown = new List<string>();
            //本地的dll
            var taskDlls = Directory.GetDirectories(GlobalConfig.TaskPluginDirSrc);
            //检查是否存在
            foreach (var item in tasklist)
            {
                //压缩包 解压缩后的文件名
                var taskDllName = Path.GetFileNameWithoutExtension(item.TaskDirName);
                if (!taskDlls.Contains(taskDllName))
                {
                    needToDown.Add(item.Id);
                }
            }
        }


    }
}
