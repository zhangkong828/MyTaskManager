using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Conventions;
using System;

namespace ZK.TaskManager.HostServer
{
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            //注册 Forms 认证  到Nancy管道
            container.Register<IUserMapper, FormsUserMapper>();
            var formsAuthConfiguration = new FormsAuthenticationConfiguration()
            {
                RedirectUrl = "~/Forms/Login",
                UserMapper = container.Resolve<IUserMapper>(),
            };
            //启用Forms 认证
            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
            pipelines.OnError += Error;
        }

        private dynamic Error(NancyContext context, Exception ex)
        {
            //异常
            Console.WriteLine(ex.Message);
            return ex.Message;
        }

        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);
        }
    }
}
