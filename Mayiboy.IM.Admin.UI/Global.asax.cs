using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Framework.Mayiboy.Ioc;
using Framework.Mayiboy.Logging;

namespace Mayiboy.IM.Admin.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            LoggerGlobal.GlobalInit();
            AutoMapperConfig.Initialize();

            RegisterAndResolverIoc();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Response.Headers.Remove("Server");
            MvcHandler.DisableMvcResponseHeader = true;//移除X-AspNetMvc-Version HTTP头
        }

        #region 注册容器
        /// <summary>
        /// 注册容器
        /// </summary>
        private void RegisterAndResolverIoc()
        {
            ContainerBuilder builder = new ContainerBuilder();

            //注册mvc容器的实现
            //没有为该对象定义无参数的构造函数。(mvc控制器构造函数注入)
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            TypeFinder typeFinder = new TypeFinder();

            foreach (var item in typeFinder.Assemblies)
            {
                DependencyConfig.RegisterDependency(item, builder);
            }

            var container = builder.Build();

            Framework.Mayiboy.Ioc.DependencyResolver.SetResolver(container);
            System.Web.Mvc.DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
        #endregion
    }
}
