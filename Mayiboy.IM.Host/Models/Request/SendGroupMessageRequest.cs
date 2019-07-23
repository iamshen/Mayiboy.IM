namespace Mayiboy.IM.Host.Models
{
    /// <summary>
    /// 发送消息
    /// </summary>
    public class SendGroupMessageRequest
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

    /// <summary>
    /// 发送消息响应
    /// </summary>
    public class SendMessageResponse
    {

    }
}