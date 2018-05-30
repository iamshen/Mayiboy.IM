using System.Configuration;
using Framework.Mayiboy.Utility;

namespace Mayiboy.IM.Utils
{
    public class AppConfig
    {
        /// <summary>
        /// 默认数据库连接字符串
        /// </summary>
        public static string DefatultSqlConnection
        {
            get { return ConfigurationManager.ConnectionStrings["DefatultSqlConnection"].ConnectionString; }
        }

        /// <summary>
        /// 缓存Key前缀
        /// </summary>
        public static string CacheKeyPrefix
        {
            get
            {
                return ConfigHelper.GetString("CacheKeyPrefix");
            }
        }
    }
}