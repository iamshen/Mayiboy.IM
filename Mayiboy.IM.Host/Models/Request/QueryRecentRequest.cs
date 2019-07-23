namespace Mayiboy.IM.Host.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class QueryRecentRequest
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public string ImUserId { get; set; }

        /// <summary>
        /// 通道标识
        /// </summary>
        public string ChannelId { get; set; }

        /// <summary>
        /// 最近信息条数
        /// </summary>
        public int TopCount { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class QueryGroupRecentRequest
    {
        /// <summary>
        /// 组标识
        /// </summary>
        public string GroupId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class QueryHistoryRequest
    {
        /// <summary>
        /// 组标识
        /// </summary>
        public string groupId { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        public string imuid { get; set; }

        /// <summary>
        /// 最小时间戳
        /// </summary>
        public string mintimestamp { get; set; }
    }

    /// <summary>
    /// 查询所有用户标示
    /// </summary>
    public class QueryAllUserRequest
    {
        /// <summary>
        /// 组标识
        /// </summary>
        public string groupId { get; set; }
    }
}