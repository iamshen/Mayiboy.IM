using System;
using System.Collections;
using System.Reflection;

namespace Mayiboy.IM.Logic.Common
{
    public class EntityLogger
    {
        /// <summary>
        /// 修改创建实体默认属性（CrateUserId、CreateTime、IsValid）
        /// </summary>
        /// <param name="entity"></param>
        public static void CreateEntity(object entity)
        {
            var properties = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                switch (prop.Name.ToLower())
                {
                    case "updateuserid":
                    case "createuserid":
                        {
                            if (prop.GetValue(entity) == null || (Guid)prop.GetValue(entity) == Guid.Empty)
                            {
                                prop.SetValue(entity, LoginUserId);
                            }
                            break;
                        }
                    case "updatetime":
                    case "createtime":
                        prop.SetValue(entity, DateTime.Now);
                        break;
                    case "isvalid":
                        prop.SetValue(entity, 1);
                        break;
                }
            }
        }

        /// <summary>
        /// 修改创建实体内嵌实体默认属性（CrateUserId、CreateTime、IsValid）
        /// </summary>
        /// <param name="entity"></param>
        public static void CreateEntityNested(object entity)
        {
            CreateEntity(entity);

            var properties = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties)
            {
                if (prop.PropertyType.Name != "String" && prop.PropertyType.GetInterface("IEnumerable", false) != null)
                {
                    var collection = prop.GetValue(entity) as IEnumerable;
                    if (collection != null)
                    {
                        foreach (var item in collection)
                        {
                            CreateEntityNested(item);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 修改更新实体默认属性（UpateUserId、UpdateTime）
        /// </summary>
        /// <param name="entity"></param>
        public static void UpdateEntity(object entity)
        {
            var properties = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties)
            {
                switch (prop.Name.ToLower())
                {
                    case "updateuserid":
                        {
                            /*当使用多线程执行时需要将用户id传入（因为多线程执行没有上下文，所有获取不到用户信息）
							 */
                            if (prop.GetValue(entity) == null || (Guid)prop.GetValue(entity) == Guid.Empty)
                            {
                                prop.SetValue(entity, LoginUserId);
                            }
                            break;
                        }
                    case "updatetime":
                        prop.SetValue(entity, DateTime.Now);
                        break;
                }
            }
        }

        /// <summary>
        /// 修改更新实体内嵌实体默认属性（UpateUserId、UpdateTime）
        /// </summary>
        /// <param name="entity"></param>
        public static void UpdateEntityNested(object entity)
        {
            UpdateEntity(entity);

            var properties = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                if (prop.PropertyType.Name != "String" && prop.PropertyType.GetInterface("IEnumerable", false) != null)
                {
                    var collection = prop.GetValue(entity) as IEnumerable;

                    if (collection != null)
                    {
                        foreach (var item in collection)
                        {
                            UpdateEntityNested(item);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateInstance<T>()
        {
            var type = typeof(T);
            var entity = (T)type.Assembly.CreateInstance(type.FullName);
            CreateEntity(entity);
            return entity;
        }

        /// <summary>
        /// 登录用户Id
        /// </summary>
        public static Guid LoginUserId
        {
            get
            {
                //var loginuserid = Guid.Empty;
                //var identityvalue = CookieHelper.Get(PublicConst.IdentityCookieKey);

                //if (identityvalue.IsNullOrEmpty())
                //{
                //    return loginuserid;
                //}

                //var cachekey = identityvalue.AddCachePrefix(PublicConst.IdentityCookieKey);

                //var key = identityvalue.AddCachePrefix(PublicConst.LoginUserIdKey);

                //var value = CacheManager.RunTimeCache.Get<string>(key);

                //if (value.IsNullOrEmpty())
                //{
                //    //从Redis中获取
                //    var entity = CacheManager.RedisDefault.Get<AccountModel>(cachekey);

                //    if (entity != null)
                //    {
                //        loginuserid = entity.Id;

                //        CacheManager.RunTimeCache.Set(key, loginuserid.ToString(), PublicConst.Time.Hour1);
                //    }
                //}
                //else
                //{
                //    loginuserid = value.ToGuid();
                //}
                //return loginuserid;

                return Guid.Empty;
            }
        }
    }
}