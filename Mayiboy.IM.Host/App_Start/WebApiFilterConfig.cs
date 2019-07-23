using System.Web.Http.Filters;

namespace Mayiboy.IM.Host
{
    /// <summary>
    /// 
    /// </summary>
    public class WebApiFilterConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public static void RegisterGlobalFilters(HttpFilterCollection config)
        {
            //config.Add(new EncryptResultAttribute());
            config.Add(new HandleErrorAttribute());
        }
    }
}