using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Mayiboy.Ioc;
using Framework.Mayiboy.Utility;
using Mayiboy.IM.ConstDefine;
using Mayiboy.IM.Contract;
using Mayiboy.IM.DataAccess.Interface;
using Mayiboy.IM.Model.Po;
using Mayiboy.IM.Utils;
using Mayiboy.IM.Utils.Exception;

namespace Mayiboy.IM.Logic.Impl
{
    public class ChannelMessageService : BaseService, IChannelMessageService
    {
        private readonly IChannelMessageRepository _channelMessageRepository;
        private readonly IChannelInfoRepository _channelInfoRepository;

        #region ctor
        public ChannelMessageService(IChannelMessageRepository channelMessageRepository, IChannelInfoRepository channelInfoRepository)
        {
            _channelMessageRepository = channelMessageRepository;
            _channelInfoRepository = channelInfoRepository;
        }
        #endregion

        #region 添加通道消息
        /// <summary>
        /// 添加通道消息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Guid Add(ChannelMessageDto dto)
        {
            if (dto == null)
            {
                throw new LogicException("保存通道消息参数不能为空");
            }

            var entity = dto.As<ChannelMessagePo>();
            entity.CmessageId = Guid.NewGuid();
            entity.IsValid = 1;

            var num = _channelMessageRepository.Insert(entity);
            if (num > 0)
            {
                return entity.CmessageId;
            }
            else
            {
                return Guid.Empty;
            }
        }

        #region 添加通道消息

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
        public void Add(string channelId, string imUserId, string msgType, string msgContent, string deviceType, string sourceType, string clientIp, string clientDesc, long lasttime)
        {
            if (FindStorageStatus(channelId.ToGuid()) == 1)
            {
                //TODO:后期队列处理队执行持久
                var dto = new ChannelMessageDto();
                dto.ChannelId = channelId.ToGuid();
                dto.ImUserId = imUserId.ToGuid();
                dto.MsgType = msgType.ToInt32();
                dto.MsgContent = msgContent;
                dto.ClientIp = clientIp;
                dto.ClientDesc = clientDesc;
                dto.DeviceType = deviceType.ToInt32();
                dto.SourceType = sourceType;
                dto.Timestamp = lasttime;
                Add(dto);
            }
        }
        #endregion

        #endregion

        #region 查询通道消息记录
        /// <summary>
        /// 查询通道消息记录
        /// </summary>
        /// <param name="channelId">通道标识</param>
        /// <param name="lasttime">最后时间戳</param>
        /// <returns></returns>
        public List<ChannelMessageDto> Query(Guid channelId, long lasttime)
        {
            var list = new List<ChannelMessageDto>();
            var datalist = _channelMessageRepository.FindWhere<ChannelMessagePo>(e => e.IsValid == 1 && e.ChannelId == channelId && e.Timestamp > lasttime);
            if (datalist != null)
            {
                list = datalist.Select(e => e.As<ChannelMessageDto>()).ToList();
            }
            return list;
        }

        /// <summary>
        /// 查询最近消息
        /// </summary>
        /// <param name="channelId">通道标识</param>
        /// <param name="topcount">最近信息条数</param>
        /// <returns></returns>
        public List<ChannelMessageDto> QueryRecent(Guid channelId, int topcount)
        {
            var list = new List<ChannelMessageDto>();
            var datalist = _channelMessageRepository.FindTopNum<ChannelMessagePo>(e => e.IsValid == 1 && e.ChannelId == channelId, e => e.Timestamp, SqlSugar.OrderByType.Desc, topcount);
            if (datalist != null)
            {
                list = datalist.Select(e => e.As<ChannelMessageDto>()).ToList();
            }
            return list;
        }

        #endregion

        #region 查询持久状态
        /// <summary>
        /// 查询持久状态
        /// </summary>
        /// <param name="channelId">通道标识</param>
        /// <returns></returns>
        private int FindStorageStatus(Guid channelId)
        {
            var value = CacheManager.RunTimeCache.Get(channelId.ToString(), () =>
            {
                string key = channelId.ToString().AddCachePrefix("StorageStatus");
                return CacheManager.RedisDefault.Get(key, () => _channelInfoRepository.FindStorageStatus(channelId).ToString(), PublicConst.Time.Hour4);
            }, PublicConst.Time.Hour1);
            return value.ToInt32();
        }
        #endregion

    }
}