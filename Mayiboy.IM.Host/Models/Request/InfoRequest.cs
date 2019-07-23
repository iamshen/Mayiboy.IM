using System;

namespace Mayiboy.IM.Host.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class InfoRequest
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public string ImUserId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class InfoResponse
    {
        /// <summary>
        /// 即时通讯用户标识
        /// </summary>
        public Guid ImUserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string ImUserName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserHeadimg { get; set; }

        /// <summary>
        /// 性别（0：女；1：男）
        /// </summary>
        public int? Gender { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户类型(1:内部用户，2：会员用户；3：第三方用户；4：临时用户)
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        public string Remark { get; set; }
    }
}