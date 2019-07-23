using System;

namespace Mayiboy.IM.Host.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UserInfoModel
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
        /// 用户标识
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户类型(1:内部用户，2：会员用户；3：第三方用户；4：临时用户)
        /// </summary>
        public int? UserType { get; set; }

        /// <summary>
        /// 角色类型（0：普通用户；1：组长；2：管理员；）
        /// </summary>
        public int? RoleType { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 在线状态（0：离线；1：在线）
        /// </summary>
        public int Online { get; set; }
    }
}