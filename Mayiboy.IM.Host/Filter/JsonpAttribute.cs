using System.Net.Http;
using System.Web.Http.Filters;

namespace Mayiboy.IM.Host
{
    /// <summary>
    /// 兼容跨域请求（jsonp数据格式）
    /// </summary>
    public class JsonpAttribute : ActionFilterAttribute
    {

        /// <summary>
        /// 请求后，检查如果是跨域请求处理返回结果
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var callbackfunction = System.Web.HttpContext.Current.Request["callback"];
            if (!string.IsNullOrEmpty(callbackfunction))
            {
                var content = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;

                //处理原数据
                content = $"{callbackfunction}({content})";

                actionExecutedContext.Response.Content = new StringContent(content);
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}