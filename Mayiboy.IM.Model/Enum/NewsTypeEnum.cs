using System;
using System.ComponentModel;
using Framework.Mayiboy.Utility;

namespace Mayiboy.IM.Model.Enum
{
    /// <summary>
    /// 消息类型帮助类
    /// </summary>
    public class NewsTypeHelper
    {
        #region 连接字符串
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="imuserId">用户标识</param>
        /// <param name="socketId">通信标识</param>
        /// <param name="sourceType">来源类型</param>
        /// <returns></returns>
        public static string ToConnect(string imuserId, string sourceType, string socketId)
        {
            var data = new
            {
                newstype = NewsTypeEnum.Connect,
                data = new
                {
                    imuserid = imuserId,
                    sourcetype = sourceType,
                    sid = socketId,
                    lasttime = DateTime.Now.ToUnix()
                }
            };
            return data.ToJson();
        }
        #endregion

        #region 上线消息字符串
        /// <summary>
        /// 上线消息字符串
        /// </summary>
        /// <param name="imuserId">用户标识</param>
        /// <param name="sourceType">来源类型</param>
        /// <param name="socketId"></param>
        /// <returns></returns>
        public static string ToOnline(string imuserId, string sourceType, string socketId)
        {
            var data = new
            {
                newstype = NewsTypeEnum.Online,
                data = new
                {
                    imuserid = imuserId,
                    sourcetype = sourceType,
                    sid = socketId
                }
            };

            return data.ToJson();
        }
        #endregion

        #region 离线消息字符串
        /// <summary>
        /// 离线消息字符串
        /// </summary>
        /// <param name="imuserId"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public static string ToOffline(string imuserId, string sourceType)
        {
            var data = new
            {
                newstype = NewsTypeEnum.Offline,
                data = new
                {
                    imuserid = imuserId,
                    sourcetype = sourceType
                }
            };

            return data.ToJson();
        }
        #endregion

        #region 消息字符串
        /// <summary>
        /// 消息字符串
        /// </summary>
        /// <param name="imuserId"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public static string ToChat(string imuserId, string sourceType, string content, long lasttime)
        {
            var data = new
            {
                newstype = NewsTypeEnum.Chat,
                data = new
                {
                    imuserid = imuserId,
                    sourcetype = sourceType,
                    datatype = MessageType.Text,
                    content = content,
                    lasttime = lasttime
                }
            };

            return data.ToJson();
        }
        #endregion

        #region 心跳字符串
        /// <summary>
        /// 心跳字符串
        /// </summary>
        /// <returns></returns>
        public static string ToHeartbeat()
        {
            var data = new
            {
                newstype = NewsTypeEnum.Heartbeat,
                data = 1
            };

            return data.ToJson();
        }
        #endregion

        #region 新增用户消息字符串
        /// <summary>
        /// 新增用户消息字符串
        /// </summary>
        /// <param name="imuserId">用户标识</param>
        /// <param name="sourceType">来源类型</param>
        /// <param name="username">用户名</param>
        /// <param name="headimg">头像</param>
        /// <param name="online">现在状态（0：离线，1：在线）</param>
        /// <returns></returns>
        public static string ToAddUser(string imuserId, string sourceType, string username, string headimg, int online = 1)
        {
            var data = new
            {
                newstype = NewsTypeEnum.AddUser,
                data = new
                {
                    sourcetype = sourceType,
                    imuserid = imuserId,
                    username = username,
                    headimg = headimg,
                    online = online
                }
            };
            return data.ToJson();
        }
        #endregion

        #region 移除用户消息字符串
        /// <summary>
        /// 移除用户消息字符串
        /// </summary>
        /// <param name="imuserId"></param>
        /// <returns></returns>
        public static string ToRemoveUser(string imuserId)
        {
            var data = new
            {
                newstype = NewsTypeEnum.RemoveUser,
                data = new { imuserid = imuserId }
            };

            return data.ToJson();
        }
        #endregion

        #region 修改用户字符串

        /// <summary>
        /// 修改用户字符串
        /// </summary>
        /// <param name="imuserId"></param>
        /// <param name="sourceType"></param>
        /// <param name="socketId"></param>
        /// <returns></returns>
        public static string ToModifyUser(string imuserId, string sourceType, string socketId)
        {
            //TODO:修改用户字符串通知消息
            return "";
        }
        #endregion
    }

    /// <summary>
    /// 消息类型
    /// </summary>
    public enum NewsTypeEnum
    {
        /*
        * 1、上线
        * 2、离线
        * 3、聊天消息
        */

        /// <summary>
        /// 连接
        /// </summary>
        [Description("连接")]
        Connect = 0,

        /// <summary>
        /// 上线
        /// </summary>
        [Description("上线")]
        Online = 1,

        /// <summary>
        /// 离线
        /// </summary>
        [Description("离线")]
        Offline = 2,

        /// <summary>
        /// 消息
        /// </summary>
        [Description("消息")]
        Chat = 3,

        /// <summary>
        /// 心跳
        /// </summary>
        [Description("心跳")]
        Heartbeat = 4,

        /// <summary>
        /// 新增用户
        /// </summary>
        [Description("新增用户")]
        AddUser = 5,

        /// <summary>
        /// 移除用户
        /// </summary>
        [Description("移除用户")]
        RemoveUser = 6,

        /// <summary>
        /// 修改用户
        /// </summary>
        [Description("修改用户")]
        ModifyUser = 7
    }

    /// <summary>
    /// 数据类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("文本")]
        Text = 1
    }
}