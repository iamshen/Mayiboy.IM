using System;

namespace Mayiboy.IM.Host.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GroupUserInfoRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string ImUserId { get; set; }

        /// <summary>
        /// 组标识
        /// </summary>
        public string GroupId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GroupUserInfoResponse
    {
        /// <summary>
        /// 用户组标识
        /// </summary>
        public Guid UserGroupId { get; set; }

        /// <summary>
        /// 组标识
        /// </summary>
        public Guid GroupId { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        public Guid ImUserId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户类型（0：普通用户；1：组长；2：管理员；）
        /// </summary>
        public int RoleType { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserHeadimg { get; set; }
    }
}