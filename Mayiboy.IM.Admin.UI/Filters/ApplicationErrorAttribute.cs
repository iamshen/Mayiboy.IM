using System;
using System.Web.Mvc;
using Framework.Mayiboy.Utility;
using Mayiboy.IM.Admin.UI.Exception;
using Mayiboy.IM.Utils;

namespace Mayiboy.IM.Admin.UI.Filters
{
    /// <summary>
    /// 应用程序异常处理
    /// </summary>
    public class ApplicationErrorAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// 应用程序异常处理
        /// </summary>
        public ApplicationErrorAttribute()
        {

        }

        public override void OnException(ExceptionContext filterContext)
        {
            MvcBaseException.RegisterExceptionHandler(filterContext);

            #region 记录未处理的异常信息
            if (!filterContext.ExceptionHandled)
            {
                var routeData = filterContext.RequestContext.RouteData;

                string controllerName = routeData.Values["controller"].IsNotNull() ? routeData.Values["controller"].ToString() : "";
                string actionName = routeData.Values["action"].IsNotNull() ? routeData.Values["action"].ToString() : "";

                string errormsg = string.Format("ExceptionTime:{0}\r\nIpAddress:{1}\r\nControllerName:{2}\r\nActionName{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), RequestHelper.Ip, controllerName, actionName);

                System.Exception ex = filterContext.Exception;
                ex.Source = errormsg + "\r\n" + ex.Source;

                LogManager.DefaultLogger.Fatal(ex);
            }
            #endregion

            filterContext.ExceptionHandled = true;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult()
                {
                    Data = new
                    {
                        status = -1,
                        msg = filterContext.Exception.Message
                    }
                };
            }
            else
            {

            }

        }
    }
}