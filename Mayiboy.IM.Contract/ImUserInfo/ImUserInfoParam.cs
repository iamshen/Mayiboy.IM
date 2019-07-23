using System;
using System.Collections.Generic;

namespace Mayiboy.IM.Contract
{
    /// <summary>
    /// 查询用户参数
    /// </summary>
    public class QueryImUserInfoRequest : PageRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string ImUserName { get; set; }

        /// <summary>
        /// 用户标示
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户类型(1:内部用户，2：会员用户；3：第三方用户；4：临时用户)
        /// </summary>
        public int? UserType { get; set; }

        /// <summary>
        /// 组标识
        /// </summary>
        public Guid? GroupId { get; set; }
    }

    /// <summary>
    /// 查询用户响应
    /// </summary>
    public class QueryImUserInfoResponse : PageResponse
    {
        /// <summary>
        /// 数据列表
        /// </summary>
        public List<ImUserInfoDto> EntityList { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class QueryGroupRequest : PageRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid ImUserId { get; set; }

        /// <summary>
        /// 组名
        /// </summary>
        public string GroupName { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class QueryGroupResponse : PageResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GroupInfoDto> EntityList { get; set; }
    }
}