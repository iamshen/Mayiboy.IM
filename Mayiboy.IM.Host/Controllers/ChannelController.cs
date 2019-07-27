using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Framework.Mayiboy.Utility;
using Mayiboy.IM.Contract;
using Mayiboy.IM.Host.Models;
using Mayiboy.IM.Model.Enum;
using Mayiboy.IM.Utils;

namespace Mayiboy.IM.Host.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ChannelController : BaseApiController
    {
        private readonly IChannelMessageService _channelMessageService;
        private readonly IChannelInfoService _channelInfoService;


        #region ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelMessageService"></param>
        public ChannelController(IChannelMessageService channelMessageService, IChannelInfoService channelInfoService)
        {
            _channelMessageService = channelMessageService;
            _channelInfoService = channelInfoService;
        }
        #endregion

        #region 发送消息
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<SendMessageResponse> Send(SendMessageRequest request)
        {
            var response = new SendMessageResponse();

            var lasttime = DateTime.Now.ToUnix();
            var data = NewsTypeHelper.ToChat(request.ImUserId, request.SourceType, request.MsgContent, lasttime);
            RedisHelper.Publish(request.ChannelId, data);
            _channelMessageService.Add(request.ChannelId, request.ImUserId, request.MsgType, request.MsgContent, request.DeviceType, request.SourceType, RequestHelper.Ip, "", lasttime);
            return Success(response);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="channelId">通道标识</param>
        /// <param name="imUserId">用户标识</param>
        /// <param name="msgType">消息类型（1：文本；2：语音；3：图片；4：表情；5：视频；）</param>
        /// <param name="msgContent">内容</param>
        /// <param name="deviceType">设备类型（0:未知；1:安卓；2：IOS；3：PC）</param>
        /// <param name="sourceType">来源类型_备用字段</param>
        /// <returns></returns>
        [HttpGet, Jsonp]
        public ApiResult<SendMessageResponse> Send(string channelId, string imUserId, string msgType, string msgContent, string deviceType, string sourceType)
        {
            var response = new SendMessageResponse();
            var lasttime = DateTime.Now.ToUnix();
            var data = NewsTypeHelper.ToChat(imUserId, sourceType, msgContent, lasttime);
            RedisHelper.Publish(channelId, data);
            _channelMessageService.Add(channelId, imUserId, msgType, msgContent, deviceType, sourceType, RequestHelper.Ip, "", lasttime);
            return Success(response);
        }

        #endregion

        #region 查询最近消息
        /// <summary>
        /// 查询最近消息
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<List<ChannelMessageVo>> QueryRecent(QueryRecentRequest request)
        {
            var datalist = new List<ChannelMessageVo>();
            var list = _channelMessageService.QueryRecent(request.ChannelId.ToGuid(), 15);
            if (list != null)
            {
                datalist = list.Select(e => new ChannelMessageVo
                {
                    ImUserId = e.ImUserId,
                    MsgType = e.MsgType,
                    MsgContent = e.MsgContent,
                    Timestamp = e.Timestamp
                }).ToList();

                //翻转消息循序
                datalist.Reverse();
            }
            return Success(datalist);
        }

        /// <summary>
        /// 查询最近15条消息
        /// </summary>
        /// <param name="ChannelId">通道标识</param>
        /// <returns></returns>
        [HttpGet, Jsonp]
        public ApiResult<List<ChannelMessageVo>> QueryRecent(string ChannelId)
        {
            var datalist = new List<ChannelMessageVo>();
            var list = _channelMessageService.QueryRecent(ChannelId.ToGuid(), 15);
            if (list != null)
            {
                datalist = list.Select(e => new ChannelMessageVo
                {
                    ImUserId = e.ImUserId,
                    MsgType = e.MsgType,
                    MsgContent = e.MsgContent,
                    Timestamp = e.Timestamp
                }).ToList();

                //翻转消息循序
                datalist.Reverse();
            }
            return Success(datalist);
        }
        #endregion

        #region 上翻历史消息
        /// <summary>
        /// 上翻历史消息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<List<ChannelMessageVo>> QueryHistory(QueryHistoryRequest request)
        {
            var list = new List<ChannelMessageVo>();
            //var list = _groupMessageService.QueryHistory(groupId.ToGuid(), imuid.ToGuid(), long.Parse(mintimestamp));

            return Success(list);
        }

        /// <summary>
        /// 上翻历史消息
        /// </summary>
        /// <returns></returns>
        [HttpGet, Jsonp]
        public ApiResult<List<ChannelMessageVo>> QueryHistory(string groupId, string imuid, string mintimestamp)
        {
            var list = new List<ChannelMessageVo>();
            //var list = _groupMessageService.QueryHistory(groupId.ToGuid(), imuid.ToGuid(), long.Parse(mintimestamp));

            return Success(list);
        }
        #endregion
    }
}
