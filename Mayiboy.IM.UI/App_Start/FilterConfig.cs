using System.Web.Mvc;
using Mayiboy.IM.UI.Filters;

namespace Mayiboy.IM.UI
{
    public class FilterConfig
    {
        /// <summary>
        /// 注册过滤器
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ApplicationErrorAttribute());
            //filters.Add(new LoginAuthAttribute());
        }
    }
}