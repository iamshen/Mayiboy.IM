using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.WebSockets;
using Framework.Mayiboy.Queue;
using Framework.Mayiboy.Utility;
using Mayiboy.IM.Contract;
using Mayiboy.IM.Host.Models;
using Mayiboy.IM.Model.Enum;
using Mayiboy.IM.Utils;

namespace Mayiboy.IM.Host.Controllers
{
    public class GroupController : BaseApiController
    {
        private readonly IImUserInfoService _imUserInfoService;
        private readonly IUserGroupService _userGroupService;
        private readonly IChannelMessageService _channelMessageService;
        private readonly IGroupInfoService _groupInfoService;

        private string _imuserid = "";      //用户标识
        private string _groupid = "";       //组标识
        private string _sourcetype = "";    //来源类型
        private string _lasttime = "";      //最后消息时间
        private string _channelid = "";     //通道标识

        #region ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imUserInfoService"></param>
        /// <param name="userGroupService"></param>
        /// <param name="channelMessageService"></param>
        /// <param name="groupInfoService"></param>
        public GroupController(IImUserInfoService imUserInfoService, IUserGroupService userGroupService, IChannelMessageService channelMessageService, IGroupInfoService groupInfoService)
        {
            _imUserInfoService = imUserInfoService;
            _userGroupService = userGroupService;
            _channelMessageService = channelMessageService;
            _groupInfoService = groupInfoService;
        }
        #endregion

        #region 连接WebSocket
        /// <summary>
        /// 连接WebSocket
        /// </summary>
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage Connect()
        {
            if (System.Web.HttpContext.Current.IsWebSocketRequest)
            {
                _groupid = GetRequest("groupid");           //组标识
                _imuserid = GetRequest("imuserid");         //用户标识
                _sourcetype = GetRequest("sourcetype");     //来源类型
                _lasttime = GetRequest("lasttime");         //最后接收消息的时间(如果为空表示没有接收过消息)

                //var token = GetRequest("token");          //凭证

                //构造同意切换至Web Socket的Response.
                var response = Request.CreateResponse(HttpStatusCode.SwitchingProtocols);

                //验证凭证
                if (!CheckToten()) return response;

                //检查用户信息
                if (!CheckUserInfo(_imuserid)) return response;

                //检查组
                if (!CheckGroupInfo(_imuserid, _groupid)) return response;

                System.Web.HttpContext.Current.AcceptWebSocketRequest(ProcessChat);
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
        #endregion

        #region 检查凭证
        /// <summary>
        /// 检查凭证
        /// </summary>
        /// <returns></returns>
        private bool CheckToten()
        {
            //TODO:待处理问题-检查凭证
            return true;
        }
        #endregion

        #region 检查用户信息
        /// <summary>
        /// 检查用户信息
        /// </summary>
        /// <param name="imuserid"></param>
        /// <returns></returns>
        private bool CheckUserInfo(string imuserid)
        {
            //非空校验
            if (imuserid.IsNullOrEmpty())
            {
                System.Web.HttpContext.Current.AcceptWebSocketRequest((e) =>
                {
                    CancellationToken cancellationToken = new CancellationToken();
                    var data = new { status = 0, msg = "用户无效", buginfo = "用户无效" }.ToJson();
                    return e.WebSocket.SendMessage(cancellationToken, data);
                });
                System.Web.HttpContext.Current.Response.End();
                return false;
            }

            //查询用户信息校验
            var userinfo = _imUserInfoService.Find(imuserid.ToGuid());
            if (userinfo == null)
            {
                System.Web.HttpContext.Current.AcceptWebSocketRequest((e) =>
                {
                    CancellationToken cancellationToken = new CancellationToken();
                    var data = new { status = 0, msg = "用户不存在", buginfo = "用户不存在" }.ToJson();
                    return e.WebSocket.SendMessage(cancellationToken, data);
                });
                System.Web.HttpContext.Current.Response.End();

                return false;
            }

            //用户删除状态
            if (userinfo.IsValid == 0)
            {
                System.Web.HttpContext.Current.AcceptWebSocketRequest((e) =>
                {
                    CancellationToken cancellationToken = new CancellationToken();
                    var data = new { status = 0, msg = "用户已被移除,请联系闲僧", buginfo = "非法用户" }.ToJson();
                    return e.WebSocket.SendMessage(cancellationToken, data);
                });
                System.Web.HttpContext.Current.Response.End();

                return false;
            }

            return true;
        }
        #endregion

        #region 检查组信息
        /// <summary>
        /// 检查组信息
        /// </summary>
        /// <param name="imuserid">用户标识</param>
        /// <param name="groupid">组标识</param>
        /// <returns></returns>
        private bool CheckGroupInfo(string imuserid, string groupid)
        {
            if (groupid.IsNullOrEmpty())
            {
                System.Web.HttpContext.Current.AcceptWebSocketRequest((e) =>
                {
                    CancellationToken cancellationToken = new CancellationToken();
                    var data = new { status = 0, msg = "参数无效", buginfo = "参数无效" }.ToJson();
                    return e.WebSocket.SendMessage(cancellationToken, data);
                });
                System.Web.HttpContext.Current.Response.End();
                return false;
            }

            //检查组
            var usergougroup = _userGroupService.FindUserGroup(imuserid.ToGuid(), groupid.ToGuid());
            if (usergougroup == null)
            {
                System.Web.HttpContext.Current.AcceptWebSocketRequest((e) =>
                {
                    CancellationToken cancellationToken = new CancellationToken();
                    var data = new { status = 0, msg = "非法用户", buginfo = "非法用户" }.ToJson();
                    return e.WebSocket.SendMessage(cancellationToken, data);
                });
                System.Web.HttpContext.Current.Response.End();
                return false;
            }
            if (usergougroup.IsValid == 0)
            {
                System.Web.HttpContext.Current.AcceptWebSocketRequest((e) =>
                {
                    CancellationToken cancellationToken = new CancellationToken();
                    var data = new { status = 0, msg = "您已被移除", buginfo = "您已被移除" }.ToJson();
                    return e.WebSocket.SendMessage(cancellationToken, data);
                });
                System.Web.HttpContext.Current.Response.End();
                return false;
            }

            //组信息
            var groupinfo = _groupInfoService.Find(usergougroup.GroupId);
            if (groupinfo == null)
            {
                System.Web.HttpContext.Current.AcceptWebSocketRequest((e) =>
                {
                    CancellationToken cancellationToken = new CancellationToken();
                    var data = new { status = 0, msg = "非法组", buginfo = "非法组" }.ToJson();
                    return e.WebSocket.SendMessage(cancellationToken, data);
                });
                System.Web.HttpContext.Current.Response.End();
                return false;
            }
            if (groupinfo.IsValid == 0)
            {
                System.Web.HttpContext.Current.AcceptWebSocketRequest((e) =>
                {
                    CancellationToken cancellationToken = new CancellationToken();
                    var data = new { status = 0, msg = "组已被移除", buginfo = "组已被移除" }.ToJson();
                    return e.WebSocket.SendMessage(cancellationToken, data);
                });
                System.Web.HttpContext.Current.Response.End();
                return false;
            }

            //检查用户
            var userinfo = _imUserInfoService.Find(imuserid.ToGuid());
            if (!groupinfo.GroupUserTypes.Contains(userinfo.UserType.ToString()))
            {
                System.Web.HttpContext.Current.AcceptWebSocketRequest((e) =>
                {
                    CancellationToken cancellationToken = new CancellationToken();
                    var data = new { status = 0, msg = "用户没有权限", buginfo = "用户没有权限" }.ToJson();
                    return e.WebSocket.SendMessage(cancellationToken, data);
                });
                System.Web.HttpContext.Current.Response.End();
                return false;
            }

            _channelid = groupinfo.ChannelId.ToString();
            return true;
        }
        #endregion

        //--------------------------------------------------------------------------------

        #region 处理消息
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task ProcessChat(AspNetWebSocketContext context)
        {
            WebSocket socket = context.WebSocket;
            ChatManager cm = ChatManager.CreateInstance(_channelid, _imuserid, socket, _sourcetype);

            //连接通知
            cm.SendMessage(NewsTypeHelper.ToConnect(_imuserid, _sourcetype, cm.SocketId));

            //上线前检查是否有中断时的消息
            SendSuspendMsg(cm);

            while (socket.State == WebSocketState.Open)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[2048]);
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, cm.CancelToken);
                if (result.MessageType == WebSocketMessageType.Close || socket.State == WebSocketState.CloseReceived) //如果输入帧为取消帧，发送close命令
                {
                    cm.ClosedConnect();
                }
                else
                {
                    //获取字符串
                    string content = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                    if (content != "1")
                    {
                        //var lasttime = DateTime.Now.ToUnix();
                        //AddMessageInfo(cm.ImUserId.ToGuid(), content, lasttime);
                        //var data = NewsTypeHelper.ToChat(cm.ImUserId, _sourcetype, content, lasttime);
                        //await cm.PublishMessage(data);
                    }
                    else
                    {
                        cm.SendHeartbeat();//答复心跳
                    }
                }
            }
        }
        #endregion

        #region 发送中断时的消息
        /// <summary>
        /// 发送中断时的消息
        /// </summary>
        /// <param name="cm"></param>
        private void SendSuspendMsg(ChatManager cm)
        {
            if (string.IsNullOrEmpty(_lasttime)) return;
            long lasttime;
            if (long.TryParse(_lasttime, out lasttime))
            {
                //查询是否有中断消息
                var channelMessageList = _channelMessageService.Query(cm.ChannelId.ToGuid(), lasttime);
                if (channelMessageList.Count > 0)
                {
                    foreach (var item in channelMessageList)
                    {
                        cm.SendMessage(NewsTypeHelper.ToChat(item.ImUserId.ToString(), _sourcetype, item.MsgContent, item.Timestamp));
                        Thread.Sleep(100);
                    }
                }
            }
        }
        #endregion

        #region 持久组内消息

        /// <summary>
        /// 持久组内消息
        /// </summary>
        /// <param name="imuserid"></param>
        /// <param name="content"></param>
        /// <param name="timestamp"></param>
        private void AddMessageInfo(Guid imuserid, string content, long timestamp)
        {
            var entity = new ChannelMessageDto
            {
                //GroupId = _groupid.ToGuid(),
                //ImUserId = imuserid,
                //MsgType = 1,
                //MsgContent = content,
                //Timestamp = timestamp
            };

            //QueueManager.GroupMessage.Enqueue(new MessageInfo("MessageInfo", entity, null, ThreadQueueHelper.AddMessageInfo));
        }
        #endregion

        //-------------------------------------------------------------------------------

        #region 发送消息
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<SendMessageResponse> Send(SendGroupMessageRequest request)
        {
            var response = new SendMessageResponse();
            if (request == null)
            {
                return Error<SendMessageResponse>("1", "参数无能为空");
            }

            if (request.GroupId.IsNullOrEmpty())
            {
                return Error<SendMessageResponse>("2", "组标识不能为空");
            }

            var channelId = _groupInfoService.GetChannelId(request.GroupId.ToGuid());
            if (channelId.IsNotNullOrEmpty())
            {
                var lasttime = DateTime.Now.ToUnix();
                var data = NewsTypeHelper.ToChat(request.ImUserId, request.SourceType, request.MsgContent, lasttime);
                RedisHelper.Publish(channelId, data);
                _channelMessageService.Add(channelId, request.ImUserId, request.MsgType, request.MsgContent, request.DeviceType, request.SourceType, RequestHelper.Ip, "", lasttime);
            }
            return Success(response);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="groupId">组标识</param>
        /// <param name="imUserId">用户标识</param>
        /// <param name="msgType">消息类型（1：文本；2：语音；3：图片；4：表情；5：视频；）</param>
        /// <param name="msgContent">内容</param>
        /// <param name="deviceType">设备类型（0:未知；1:安卓；2：IOS；3：PC）</param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        [HttpGet, Jsonp]
        public ApiResult<SendMessageResponse> Send(string groupId, string imUserId, string msgType, string msgContent, string deviceType, string sourceType)
        {
            var response = new SendMessageResponse();
            if (groupId.IsNullOrEmpty())
            {
                return Error<SendMessageResponse>("1", "组标识不能为空");
            }

            if (imUserId.IsNullOrEmpty())
            {
                return Error<SendMessageResponse>("2", "用户不能为空");
            }

            var channelId = _groupInfoService.GetChannelId(groupId.ToGuid());
            if (channelId.IsNotNullOrEmpty())
            {
                var lasttime = DateTime.Now.ToUnix();
                var data = NewsTypeHelper.ToChat(imUserId, sourceType, msgContent, lasttime);
                RedisHelper.Publish(channelId, data);
                _channelMessageService.Add(channelId, imUserId, msgType, msgContent, deviceType, sourceType, RequestHelper.Ip, "", lasttime);
            }

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
        public ApiResult<List<ChannelMessageVo>> QueryRecent(QueryGroupRecentRequest request)
        {
            var datalist = new List<ChannelMessageVo>();
            var channelId = _groupInfoService.GetChannelId(request.GroupId.ToGuid());
            var list = _channelMessageService.QueryRecent(channelId.ToGuid(), 15);
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
        /// <param name="groupId">组标识</param>
        /// <returns></returns>
        [HttpGet, Jsonp]
        public ApiResult<List<ChannelMessageVo>> QueryRecent(string groupId)
        {
            var datalist = new List<ChannelMessageVo>();
            var channelId = _groupInfoService.GetChannelId(groupId.ToGuid());
            var list = _channelMessageService.QueryRecent(channelId.ToGuid(), 15);
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

        #region 根据用户组获取通道标识
        /// <summary>
        /// 根据用户组获取通道标识
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private ApiResult<GetGroupInfoResponse> GetChannelId(GetGroupInfoRequest request)
        {
            var response = new GetGroupInfoResponse();
            if (request == null || request.GroupId.IsNullOrEmpty())
            {
                return Error<GetGroupInfoResponse>("1", "参数不能为空");
            }

            var channelId = _groupInfoService.GetChannelId(request.GroupId.ToGuid());

            response.ChannelId = channelId.ToGuid();

            return Success(response);
        }

        /// <summary>
        /// 根据用户组获取通道标识
        /// </summary>
        /// <param name="groupId">组标识</param>
        /// <returns></returns>
        [HttpGet, Jsonp]
        private ApiResult<GetGroupInfoResponse> GetChannelId(string groupId)
        {
            var response = new GetGroupInfoResponse();
            if (groupId.IsNullOrEmpty())
            {
                return Error<GetGroupInfoResponse>("1", "参数不能为空");
            }

            var channelId = _groupInfoService.GetChannelId(groupId.ToGuid());

            response.ChannelId = channelId.ToGuid();

            return Success(response);
        }
        #endregion

        private string GetConnectionId()
        {
            //TODO:待实现，使用用户凭证换取临时连接标识
            return "";
        }
    }
}
