using System;

namespace Mayiboy.IM.Host.Models
{
    /// <summary>
    /// 通道消息
    /// </summary>
    public class ChannelMessageVo
    {

        /// <summary>
        /// 用户标识
        /// </summary>
        public Guid ImUserId { get; set; }

        /// <summary>
        /// 消息类型（1：文本；2：语音；3：图片；4：表情；）
        /// </summary>
        public int MsgType { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string MsgContent { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public long Timestamp { get; set; }
    }
}