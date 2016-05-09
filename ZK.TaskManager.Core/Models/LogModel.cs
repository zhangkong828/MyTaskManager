using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.Core.Models
{
    public class LogModel
    {
        public string Id { get; set; }
        public string Time { get; set; }
        public string Msg { get; set; }
        public string ExMessage { get; set; }
        public string Type { get; set; }
        public string JobId { get; set; }
    }
}
