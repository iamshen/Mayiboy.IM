using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using Framework.Mayiboy.Ioc;
using Framework.Mayiboy.Logging;

namespace Mayiboy.IM.Host
{
    /// <summary>
    /// 
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 
        /// </summary>
        protected void Application_Start()
        {
            LoggerGlobal.GlobalInit();
            AutoMapperConfig.Initialize();
            RegisterAndResolverIoc();

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        #region 移除不必要的请求头
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }
        #endregion

        #region 注册容器
        /// <summary>
        /// 注册容器
        /// </summary>
        private void RegisterAndResolverIoc()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            TypeFinder typeFinder = new TypeFinder();

            foreach (var item in typeFinder.Assemblies)
            {
                DependencyConfig.RegisterDependency(item, builder);
            }

            var container = builder.Build();
            DependencyResolver.SetResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
        #endregion
    }
}
