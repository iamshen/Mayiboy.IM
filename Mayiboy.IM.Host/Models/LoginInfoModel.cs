using System;

namespace Mayiboy.IM.Host.Models
{
    /// <summary>
    /// （key:UserId）
    /// </summary>
    [Serializable]
    public class LoginInfoModel
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserIdentity { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 登陆标识
        /// </summary>
        public string LoginGuiId { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginDateTime { get; set; }

        /// <summary>
        /// 登陆手机品牌
        /// </summary>
        public string PhoneBrand { get; set; }

        /// <summary>
        /// 登陆Ip地址
        /// </summary>
        public string LoginIp { get; set; }

    }
}