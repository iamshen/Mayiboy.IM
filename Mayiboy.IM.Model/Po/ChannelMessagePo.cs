using System;
using SqlSugar;

namespace Mayiboy.IM.Model.Po
{
    /// <summary>
    /// 通道消息
    /// </summary>
    [SugarTable("ChannelMessage")]
    public class ChannelMessagePo
    {
        /// <summary>
        /// 通道消息标识
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid CmessageId { get; set; }

        /// <summary>
        /// 通道标识
        /// </summary>
        public Guid ChannelId { get; set; }

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
        /// 客户端Ip
        /// </summary>
        public string ClientIp { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public int DeviceType { get; set; }

        /// <summary>
        /// 来源类型
        /// </summary>
        public string SourceType { get; set; }

        /// <summary>
        /// 客户端描述
        /// </summary>
        public string ClientDesc { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// 是否有效（0：无效；1：有效）
        /// </summary>
        public int IsValid { get; set; }
    }
}