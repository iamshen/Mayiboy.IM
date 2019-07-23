using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Linq;

namespace Mayiboy.IM.Utils
{
    public static class ExtHelper
    {
        #region 转换数据类型
        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T As<T>(this object value)
        {
            return AutoMapper.Mapper.Map<T>(value);
        }

        #endregion

        #region 添加缓存前缀
        /// <summary>
        /// 添加缓存前缀
        /// </summary>
        /// <param name="value">key</param>
        /// <param name="businessstr">业务字符串</param>
        /// <param name="block">冒号分割（默认分割）</param>
        /// <returns></returns>
        public static string AddCachePrefix(this string value, string businessstr = "", bool block = true)
        {
            if (block)
            {
                return (AppConfig.CacheKeyPrefix + ":" + businessstr + ":" + value).ToLower();

            }
            else
            {
                return (AppConfig.CacheKeyPrefix + businessstr + value).ToLower();
            }
        }
        #endregion

        /// <summary>
        /// Merge Cache key
        /// </summary>
        /// <param name="value">key</param>
        /// <param name="businessstr">业务字符串</param>
        /// <param name="cacheKeyPrefix">前缀</param>
        /// <returns></returns>
        public static string AddCachePrefix(this string value, string businessstr, string cacheKeyPrefix, bool block = true)
        {
            if (block)
            {
                return cacheKeyPrefix + ":" + businessstr + ":" + value;

            }
            else
            {
                return cacheKeyPrefix + businessstr + value;
            }
        }

        #region 获取RequestHeaders值
        /// <summary>
        /// 获取RequestHeaders值
        /// </summary>
        /// <param name="requestheaders"></param>
        /// <param name="name">name</param>
        /// <returns></returns>
        public static string GetValue(this HttpRequestHeaders requestheaders, string name)
        {
            IEnumerable<string> values;

            if (requestheaders.TryGetValues(name, out values))
            {
                return values.First();
            }

            return "";
        }
        #endregion

        #region 返回非NULL的字符串
        /// <summary>
        /// 返回非NULL的字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjToString(this object obj)
        {
            if (obj == null) return string.Empty;

            return obj.ToString();
        }
        #endregion

        #region 转换Guid
        /// <summary>
        /// 转换Guid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Guid ToGuid(this string value)
        {
            var guid = Guid.Empty;

            if (string.IsNullOrEmpty(value))
            {
                return guid;
            }

            Guid.TryParse(value, out guid);
            return guid;
        }
        #endregion

        #region 是否为空Guid 
        /// <summary>
        /// 是否为空Guid 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNullGuid(this Guid input)
        {
            return input == Guid.Empty;
        }
        #endregion
    }
}