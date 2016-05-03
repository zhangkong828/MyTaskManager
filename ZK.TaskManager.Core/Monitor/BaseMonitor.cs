using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.Core
{
    /// <summary>
    /// 基础监控者
    /// </summary>
    public abstract class BaseMonitor
    {
        protected System.Threading.Thread _thread;
        /// <summary>
        /// 间隔时间 （毫秒）
        /// </summary>
        public int Interval { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        public BaseMonitor(string _name, int _interval)
        {
            Name = _name;
            Interval = _interval;

            _thread = new System.Threading.Thread(TryRun);
            _thread.IsBackground = true;
            _thread.Start();
        }

        private void TryRun()
        {
            while (true)
            {
                try
                {
                    Run();
                    System.Threading.Thread.Sleep(Interval);
                }
                catch (Exception ex)
                {
                    Log.SysError("", ex);
                }
            }
        }

        /// <summary>
        /// 执行方法约定
        /// </summary>
        protected abstract void Run();


    }
}
