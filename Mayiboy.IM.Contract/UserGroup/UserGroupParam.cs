using System.Collections.Generic;

namespace Mayiboy.IM.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public class QueryUserGroupRequest : PageRequest
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class QueryUserGroupResponse : PageResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public List<UserGroupDto> EntityList { get; set; }
    }
}