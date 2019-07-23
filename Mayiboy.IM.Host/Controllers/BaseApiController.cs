using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Framework.Mayiboy.Ioc;
using Mayiboy.IM.Contract;
using Mayiboy.IM.Host.Models;
using Mayiboy.IM.Utils;

namespace Mayiboy.IM.Host.Controllers
{
    public class BaseApiController : ApiController
    {
        private readonly IUserGroupService _userGroupService;

        /// <summary>
        /// 
        /// </summary>
        public BaseApiController()
        {
            _userGroupService = ServiceLocater.GetService<IUserGroupService>();
        }

        #region 结果返回
        /// <summary>
        /// 结果返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="status">状态值</param>
        /// <param name="msg">消息</param>
        /// <param name="data"></param>
        /// <returns></returns>
        internal ApiResult<T> Result<T>(string status, string msg, T data)
        {
            return new ApiResult<T>
            {
                Status = status,
                Msg = msg,
                Data = data
            };

        }
        #endregion

        #region 成功返回
        /// <summary>
        /// 成功返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        internal ApiResult<T> Success<T>(string msg, T data)
        {
            return Result<T>("0", msg, data);
        }

        /// <summary>
        /// 成功返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        internal ApiResult<T> Success<T>(T data)
        {
            return Result<T>("0", "ok", data);
        }
        #endregion

        #region 错误返回
        /// <summary>
        /// 错误返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="status">状态码</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        internal ApiResult<T> Error<T>(string status, string msg)
        {
            return Result<T>(status, msg, default(T));
        }

        /// <summary>
        /// 错误返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg">错误消息提示</param>
        /// <returns></returns>
        internal ApiResult<T> Error<T>(string msg)
        {
            return Result<T>("-1", msg, default(T));
        }
        #endregion

        #region 获取参数
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal string GetRequest(string key)
        {
            return System.Web.HttpContext.Current.Request[key];
        }
        #endregion

        #region 获取组用户信息
        /// <summary>
        /// 获取用户组信息
        /// </summary>
        /// <param name="imuid"></param>
        /// <param name="groupid"></param>
        /// <returns></returns>
        internal Hashtable FindUserGroupInfo(Guid imuid, Guid groupid)
        {
            Hashtable ht = new Hashtable();

            var key = string.Concat(imuid.ToString(), ":", groupid.ToString()).AddCachePrefix("tb:UserGroup");
            var data = CacheManager.RunTimeCache.Get(key, () => _userGroupService.FindUserGroup(imuid, groupid), 10);

            if (data != null)
            {
                if (string.IsNullOrEmpty(data.NickName))
                {
                    data.NickName = _userGroupService.UdpateNickName(imuid, groupid);
                }

                ht.Add("ImUserId", data.ImUserId);
                ht.Add("NickName", data.NickName);
                ht.Add("RoleType", data.RoleType);
            }

            return ht;
        }
        #endregion

        public void RedirectUrl(string url)
        {
            System.Web.HttpContext.Current.Response.Clear();//这里是关键，清除在返回前已经设置好的标头信息，这样后面的跳转才不会报错
            System.Web.HttpContext.Current.Response.BufferOutput = true;//设置输出缓冲
            if (!System.Web.HttpContext.Current.Response.IsRequestBeingRedirected)//在跳转之前做判断,防止重复
            {
                System.Web.HttpContext.Current.Response.Redirect(url, true);
            }
        }
    }
}
