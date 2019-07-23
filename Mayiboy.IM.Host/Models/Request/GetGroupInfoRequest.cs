using System;

namespace Mayiboy.IM.Host.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetGroupInfoRequest
    {
        /// <summary>
        /// 组标识
        /// </summary>
        public string GroupId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetGroupInfoResponse
    {
        /// <summary>
        /// 通道标识
        /// </summary>
        public Guid ChannelId { get; set; }
    }
}