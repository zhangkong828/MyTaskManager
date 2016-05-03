using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.HostServer.Modules
{
    public class FormsModule : NancyModule
    {
        public FormsModule() : base("/forms")
        {
            Get["/"] = r =>
            {
                var os = System.Environment.OSVersion;
                if (Context.CurrentUser == null) return "no login";
                var user = Context.CurrentUser.UserName;
                return "Nancy Forms <br/> System:" + os.VersionString + "<br/>" + user + "<br/><a href='/forms/logout'>logout</a>";
            };

            Get["/login"] = r =>
            {
                var error = this.Request.Query.error.HasValue;
                return View["login", new { Errored = error }];
            };

            Post["/login"] = r =>
            {
                var userGuid = FormsUserMapper.ValidateUser((string)this.Request.Form.Username, (string)this.Request.Form.Password);

                if (userGuid == null)
                {
                    return this.Context.GetRedirect("~/Forms/Login?error=true&username=" + (string)this.Request.Form.Username);
                }

                DateTime? expiry = null;
                if (this.Request.Form.RememberMe.HasValue)
                {
                    expiry = DateTime.Now.AddDays(7);
                }

                return this.Login(userGuid.Value, expiry);
            };

            Get["/logout"] = r =>
            {
                return this.Logout("~/Forms/Login");
            };
        }
    }
}
