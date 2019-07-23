using System;

namespace Mayiboy.IM.Host.Models
{
    /// <summary>
    /// 添加用户组
    /// </summary>
    public class AddUserGroupRequest
    {
        /// <summary>
        /// 组标识
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        public string ImUserId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 角色类型（0：普通用户；1：组长；2：管理员；）
        /// </summary>
        public string RoleType { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class AddUserGroupResponse
    {
        /// <summary>
        /// 用户组标识
        /// </summary>
        public Guid UserGroupId { get; set; }
    }
}