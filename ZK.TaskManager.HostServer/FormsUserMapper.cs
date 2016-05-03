using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.HostServer
{
    public class FormsUserMapper : IUserMapper
    {
        private static List<Tuple<string, string, Guid>> users = new List<Tuple<string, string, Guid>>();

        public FormsUserMapper()
        {
            users.Add(new Tuple<string, string, Guid>("admin", "admin", new Guid("18FF111D-DCF5-4FFC-9CFA-4C256E7C9748")));
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            var userRecord = users.Where(u => u.Item3 == identifier).FirstOrDefault();

            return userRecord == null
                       ? null
                       : new FormsUser { UserName = userRecord.Item1 };
        }

        public static Guid? ValidateUser(string username, string password)
        {
            var userRecord = users.Where(u => u.Item1 == username && u.Item2 == password).FirstOrDefault();

            if (userRecord == null)
            {
                return null;
            }

            return userRecord.Item3;
        }
    }
}
