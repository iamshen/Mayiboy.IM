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
using Framework.Mayiboy.Utility;
using Mayiboy.IM.Contract;
using Mayiboy.IM.Host.Models;
using Mayiboy.IM.Model.Enum;
using Mayiboy.IM.Utils;

namespace Mayiboy.IM.Host.Controllers
{
    public class UserGroupController : BaseApiController
    {
        private readonly IImUserInfoService _imUserInfoService;
        private readonly IUserGroupService _userGroupService;
        private readonly IChannelInfoService _channelInfoService;
        private readonly IGroupInfoService _groupInfoService;

        private string _socketid = "";      //通信标识
        private string _sourcetype = "";    //来源类型
        private string _imuid = "";         //用户标识

        #region ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imUserInfoService"></param>
        public UserGroupController(IImUserInfoService imUserInfoService, IUserGroupService userGroupService, IChannelInfoService channelInfoService, IGroupInfoService groupInfoService)
        {
            _imUserInfoService = imUserInfoService;
            _userGroupService = userGroupService;
            _channelInfoService = channelInfoService;
            _groupInfoService = groupInfoService;
        }
        #endregion

        #region 连接WebSocket
        /// <summary>
        /// 连接WebSocket
        /// </summary>
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public void Connect()
        {
            if (System.Web.HttpContext.Current.IsWebSocketRequest)
            {
                _socketid = GetRequest("socketid");             //通信标识
                _sourcetype = GetRequest("sourcetype");         //来源类型

                #region 检查socketid
                if (string.IsNullOrEmpty(_socketid))
                {
                    System.Web.HttpContext.Current.AcceptWebSocketRequest((e) =>
                    {
                        CancellationToken cancellationToken = new CancellationToken();
                        var data = new { status = 0, msg = "socketid无效", buginfo = "socketid无效" }.ToJson();
                        return e.WebSocket.SendMessage(cancellationToken, data);
                    });
                    System.Web.HttpContext.Current.Response.End();
                    return;
                }
                #endregion

                System.Web.HttpContext.Current.AcceptWebSocketRequest(ProcessChat);
            }
            else
            {
                System.Web.HttpContext.Current.Response.Write("this is websocket connect");
            }
            System.Web.HttpContext.Current.Response.End();
        }
        #endregion

        #region 处理消息
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task ProcessChat(AspNetWebSocketContext context)
        {
            WebSocket socket = context.WebSocket;
            ChatManager cm = ChatManager.CreateInstance(_socketid, _imuid, socket, _sourcetype);

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
                    cm.PublishMessage(new { data = content }.ToJson());
                }
            }
        }
        #endregion

        #region 查询所有用户
        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<List<UserInfoModel>> QueryAllUser(QueryAllUserRequest request)
        {
            var list = new List<UserInfoModel>();
            var data = _userGroupService.QueryAllUser(request.groupId.ToGuid());
            if (data != null)
            {
                foreach (var item in data)
                {
                    var entity = new UserInfoModel();
                    entity.ImUserId = item.ImUserId;
                    entity.ImUserName = item.ImUserName;
                    entity.UserHeadimg = item.UserHeadimg;
                    entity.UserId = item.UserId;
                    entity.UserType = item.UserType;
                    entity.Remark = item.Remark;
                    entity.Online = (CacheManager.RedisChat.Get(item.ImUserId.ToString().ToLower().AddCachePrefix("Online")).IsNullOrEmpty() ? 0 : 1);
                    list.Add(entity);
                }
            }

            return Success(list);
        }

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet, Jsonp]
        public ApiResult<List<UserInfoModel>> QueryAllUser(string groupId)
        {
            var list = new List<UserInfoModel>();
            var data = _userGroupService.QueryAllUser(groupId.ToGuid());
            if (data != null)
            {
                foreach (var item in data)
                {
                    var entity = new UserInfoModel();
                    entity.ImUserId = item.ImUserId;
                    entity.ImUserName = item.ImUserName;
                    entity.UserHeadimg = item.UserHeadimg;
                    entity.UserId = item.UserId;
                    entity.UserType = item.UserType;
                    entity.Remark = item.Remark;
                    entity.Online = (CacheManager.RedisChat.Get(item.ImUserId.ToString().ToLower().AddCachePrefix("Online")).IsNullOrEmpty() ? 0 : 1);
                    list.Add(entity);
                }
            }

            return Success(list);
        }

        #endregion

        #region 获取组用户信息
        /// <summary>
        /// 获取组用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<GroupUserInfoResponse> GroupUserInfo(GroupUserInfoRequest request)
        {
            var response = new GroupUserInfoResponse();

            if (request == null)
            {
                return Error<GroupUserInfoResponse>("1", "参数无能为空");
            }
            var usergroup = _userGroupService.FindUserGroup(request.ImUserId.ToGuid(), request.GroupId.ToGuid());
            if (usergroup == null)
            {
                return Error<GroupUserInfoResponse>("2", "用户组不存在");
            }

            var userinfo = _imUserInfoService.Find(request.ImUserId.ToGuid());
            if (userinfo == null)
            {
                return Error<GroupUserInfoResponse>("3", "用户信息不存在");
            }

            response.ImUserId = userinfo.ImUserId;
            response.GroupId = usergroup.GroupId;
            response.NickName = usergroup.NickName;
            response.UserGroupId = usergroup.UserGroupId;
            response.RoleType = usergroup.RoleType;
            response.UserHeadimg = userinfo.UserHeadimg;

            return Success(response);
        }

        /// <summary>
        /// 获取组用户信息
        /// </summary>
        /// <param name="imUserId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet, Jsonp]
        public ApiResult<GroupUserInfoResponse> GroupUserInfo(string imUserId, string groupId)
        {
            var response = new GroupUserInfoResponse();
            var usergroup = _userGroupService.FindUserGroup(imUserId.ToGuid(), groupId.ToGuid());
            if (usergroup == null)
            {
                return Error<GroupUserInfoResponse>("2", "用户组不存在");
            }

            var userinfo = _imUserInfoService.Find(imUserId.ToGuid());
            if (userinfo == null)
            {
                return Error<GroupUserInfoResponse>("3", "用户信息不存在");
            }

            response.ImUserId = userinfo.ImUserId;
            response.GroupId = usergroup.GroupId;
            response.NickName = usergroup.NickName;
            response.UserGroupId = usergroup.UserGroupId;
            response.RoleType = usergroup.RoleType;
            response.UserHeadimg = userinfo.UserHeadimg;

            return Success(response);
        }
        #endregion

        #region 添加用户组
        /// <summary>
        /// 添加用户组
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<AddUserGroupResponse> Add(AddUserGroupRequest request)
        {
            var response = new AddUserGroupResponse();
            var entity = _userGroupService.FindUserGroup(request.ImUserId.ToGuid(), request.GroupId.ToGuid());
            if (entity == null)
            {
                //新增
                var guid = _userGroupService.SaveUserGroup(request.GroupId.ToGuid(), request.ImUserId.ToGuid(), request.NickName, request.RoleType.ToInt32());
                if (!guid.IsNullGuid())
                {
                    //新增用户通知
                    var userinfo = _imUserInfoService.Find(request.ImUserId.ToGuid());
                    var headimg = (userinfo == null ? "" : userinfo.UserHeadimg);
                    var msgcontent = NewsTypeHelper.ToAddUser(request.ImUserId.ToString(), "0", request.NickName, headimg);
                    var channelId = _groupInfoService.GetChannelId(request.GroupId.ToGuid());
                    RedisHelper.Publish(channelId, msgcontent);
                }
            }
            else
            {
                //修改
                if (entity.NickName != request.NickName)
                {
                    _userGroupService.SaveUserGroup(request.GroupId.ToGuid(), request.ImUserId.ToGuid(), request.NickName, request.RoleType.ToInt32());
                }
            }

            return Success(response);
        }

        /// <summary>
        /// 添加用户组
        /// </summary>
        /// <param name="groupId">组标识</param>
        /// <param name="imUserId">用户标识</param>
        /// <param name="nickName">昵称</param>
        /// <param name="roleType">角色类型</param>
        /// <returns></returns>
        [HttpGet, Jsonp]
        public ApiResult<AddUserGroupResponse> Add(string groupId, string imUserId, string nickName, string roleType)
        {
            var response = new AddUserGroupResponse();

            var entity = _userGroupService.FindUserGroup(imUserId.ToGuid(), groupId.ToGuid());

            if (entity == null)
            {
                //新增
                var guid = _userGroupService.SaveUserGroup(groupId.ToGuid(), imUserId.ToGuid(), nickName, roleType.ToInt32());

                if (!guid.IsNullGuid())
                {
                    //新增用户通知
                    var userinfo = _imUserInfoService.Find(imUserId.ToGuid());
                    var headimg = (userinfo == null ? "" : userinfo.UserHeadimg);
                    var msgcontent = NewsTypeHelper.ToAddUser(imUserId, "0", nickName, headimg);
                    var channelId = _groupInfoService.GetChannelId(groupId.ToGuid());
                    RedisHelper.Publish(channelId, msgcontent);
                }
            }
            else
            {
                //修改
                if (entity.NickName != nickName)
                {
                    _userGroupService.SaveUserGroup(groupId.ToGuid(), imUserId.ToGuid(), nickName, roleType.ToInt32());
                }
            }

            return Success(response);
        }
        #endregion

        #region 查询用户组
        /// <summary>
        /// 查询用户组
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<UserGroupResponse> QueryUserGroup(UserGroupRequest request)
        {
            var response = new UserGroupResponse();

            var usergroup = _userGroupService.FindUserGroup(request.ImUserId.ToGuid(), request.GroupId.ToGuid());

            if (usergroup == null)
            {
                return Error<UserGroupResponse>("1", "用户组不存在");
            }

            response.UserGroupId = usergroup.UserGroupId;
            response.NickName = usergroup.NickName;
            response.RoleType = usergroup.RoleType;

            return Success(response);
        }

        /// <summary>
        /// 查询用户组
        /// </summary>
        /// <param name="groupId">组标识</param>
        /// <param name="imUserId">用户标识</param>
        /// <returns></returns>
        [HttpGet, Jsonp]
        public ApiResult<UserGroupResponse> QueryUserGroup(string groupId, string imUserId)
        {
            var response = new UserGroupResponse();

            var usergroup = _userGroupService.FindUserGroup(imUserId.ToGuid(), groupId.ToGuid());

            if (usergroup == null)
            {
                return Error<UserGroupResponse>("1", "用户组不存在");
            }

            response.UserGroupId = usergroup.UserGroupId;
            response.NickName = usergroup.NickName;
            response.RoleType = usergroup.RoleType;

            return Success(response);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Remove()
        {
            return "";
        }
    }
}
