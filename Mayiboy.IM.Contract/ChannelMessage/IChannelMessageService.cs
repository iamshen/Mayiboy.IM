using System;
using System.Collections.Generic;
using Framework.Mayiboy.Ioc;

namespace Mayiboy.IM.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public interface IChannelMessageService : IBaseService, IDependency
    {
        /// <summary>
        /// 添加通道消息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Guid Add(ChannelMessageDto dto);

        /// <summary>
        /// 添加通道消息
        /// </summary>
        /// <param name="channelId">通道标识</param>
        /// <param name="imUserId">用户标识</param>
        /// <param name="msgType">消息类型（1：文本；2：语音；3：图片；4：表情；5：视频；）</param>
        /// <param name="msgContent">消息内容</param>
        /// <param name="deviceType">设备类型（0:未知；1:安卓；2：IOS；3：PC）</param>
        /// <param name="sourceType">来源类型_备用字段</param>
        /// <param name="clientIp">客户端Ip</param>
        /// <param name="clientDesc">客户端详情</param>
        /// <param name="lasttime">时间戳</param>
        void Add(string channelId, string imUserId, string msgType, string msgContent, string deviceType, string sourceType, string clientIp, string clientDesc, long lasttime);

        /// <summary>
        /// 查询通道消息记录
        /// </summary>
        /// <param name="channelId">通道标识</param>
        /// <param name="lasttime">最后时间戳</param>
        /// <returns></returns>
        List<ChannelMessageDto> Query(Guid channelId, long lasttime);

        /// <summary>
        /// 查询最近消息
        /// </summary>
        /// <param name="channelId">通道标识</param>
        /// <param name="topcount">最近信息条数</param>
        /// <returns></returns>
        List<ChannelMessageDto> QueryRecent(Guid channelId, int topcount);
    }
}