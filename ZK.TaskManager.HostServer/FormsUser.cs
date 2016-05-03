using Nancy.Security;
using System.Collections.Generic;

namespace ZK.TaskManager.HostServer
{
    public class FormsUser : IUserIdentity
    {
        public string UserName { get; set; }

        public IEnumerable<string> Claims { get; set; }
    }
}
