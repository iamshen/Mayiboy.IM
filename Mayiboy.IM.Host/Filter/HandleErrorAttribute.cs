using System.Web.Http.Filters;
using ServiceStack.Logging;

namespace Mayiboy.IM.Host
{
    /// <summary>
    /// 全局异常处理
    /// </summary>
    public class HandleErrorAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //LogManager.DefaultLogger.FatalFormat("Im.Nuoeu.WebHost应用程序出错:{0}", actionExecutedContext.Exception.ToString());

            base.OnException(actionExecutedContext);
        }
    }
}