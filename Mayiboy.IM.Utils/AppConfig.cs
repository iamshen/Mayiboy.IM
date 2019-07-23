using System.Configuration;
using Framework.Mayiboy.Utility;

namespace Mayiboy.IM.Utils
{
    public class AppConfig
    {
        /// <summary>
        /// 应用标示
        /// </summary>
        public static string AppId => ConfigHelper.GetString("AppId");

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

        /// <summary>
        /// 文件根路径
        /// </summary>
        public static string FileRootPath
        {
            get { return ConfigHelper.GetString("FileRootPath"); }
        }

        /// <summary>
        /// http文件地址
        /// </summary>
        public static string HttpFileUrl
        {
            get { return ConfigHelper.GetString("HttpFileUrl"); }
        }
    }
}