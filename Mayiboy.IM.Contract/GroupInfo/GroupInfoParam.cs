using System;
using System.Collections.Generic;

namespace Mayiboy.IM.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public class QueryGroupUserRequest : PageRequest
    {
        /// <summary>
        /// 组标识
        /// </summary>
        public Guid GroupId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int? Gender { get; set; }

        /// <summary>
        /// 角色类型（0：普通用户；1：组长；2：管理员；）
        /// </summary>
        public int? RoleUser { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class QueryGroupUserResponse : PageResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ImUserInfoDto> EntityList { get; set; }
    }
}