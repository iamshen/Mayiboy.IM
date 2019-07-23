namespace Mayiboy.IM.Host.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SendMessageRequest
    {
        /// <summary>
        /// 通道
        /// </summary>
        public string ChannelId { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        public string ImUserId { get; set; }

        /// <summary>
        /// 消息类型（1：文本；2：语音；3：图片；4：表情；5：视频；）
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string MsgContent { get; set; }

        /// <summary>
        /// 设备类型（0:未知；1:安卓；2：IOS；3：PC）
        /// </summary>
        public string DeviceType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SourceType { get; set; }
    }
}