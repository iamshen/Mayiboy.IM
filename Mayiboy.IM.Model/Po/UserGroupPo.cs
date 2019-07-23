using System;
using SqlSugar;

namespace Mayiboy.IM.Model.Po
{
    /// <summary>
    /// 用户组
    /// </summary>
    [SugarTable("UserGroup")]
    public class UserGroupPo
    {
        /// <summary>
        /// 用户组标识
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid UserGroupId { get; set; }

        /// <summary>
        /// 组标识
        /// </summary>
        public Guid GroupId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        public Guid ImUserId { get; set; }

        /// <summary>
        /// 角色类型（0：普通用户；1：组长；2：管理员；）
        /// </summary>
        public int RoleType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 是否有效（0：无效；1：有效）
        /// </summary>
        public int IsValid { get; set; }
    }
}