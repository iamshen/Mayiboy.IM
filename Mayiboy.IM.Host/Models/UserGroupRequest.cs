using System;

namespace Mayiboy.IM.Host.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UserGroupRequest
    {
        /// <summary>
        /// 组标识
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        public string ImUserId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UserGroupResponse
    {
        /// <summary>
        /// 用户组标识
        /// </summary>
        public Guid UserGroupId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        public int RoleType { get; set; }

    }
}