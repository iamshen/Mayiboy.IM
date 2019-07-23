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
using Mayiboy.IM.Contract;

namespace Mayiboy.IM.Host.Controllers
{
    public class UserOnlineController : BaseApiController
    {
        private string _UserName = "";
        private readonly IImUserInfoService _imUserInfoService;

        #region ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imUserInfoService"></param>
        public UserOnlineController(IImUserInfoService imUserInfoService)
        {
            _imUserInfoService = imUserInfoService;
        }
        #endregion

        /// <summary>
        /// 链接
        /// </summary>
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public void Connect()
        {
            if (System.Web.HttpContext.Current.IsWebSocketRequest)
            {
                string token = GetRequest("token");
                string groupid = GetRequest("groupId");
                System.Web.HttpContext.Current.AcceptWebSocketRequest(ProcessChat);
            }
            else
            {
                System.Web.HttpContext.Current.Response.Write("this is websocket connect");
            }
            System.Web.HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task ProcessChat(AspNetWebSocketContext context)
        {
            WebSocket socket = context.WebSocket;
            CancellationToken cancellationToken = new CancellationToken();
            //ChatManager.AddUser(_UserName, socket);

            //某人登陆后，给群里其他人发  登陆消息
            //await ChatManager.SendMessage(cancellationToken, $"{DateTime.Now.ToString("yyyyMMdd-HHmmss:fff")} {this._UserName} 进入聊天室");

            while (socket.State == WebSocketState.Open)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[2048]);
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, cancellationToken);
                if (result.MessageType == WebSocketMessageType.Close) //如果输入帧为取消帧，发送close命令
                {
                    //放在前面移除和发消息，  因为直接关浏览器会导致CloseAsync异常
                    //ChatManager.RemoveUser(_UserName);
                    //await ChatManager.SendMessage(cancellationToken, $"{DateTime.Now.ToString("yyyyMMdd-HHmmss:fff")} {this._UserName} 离开聊天室");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, String.Empty, cancellationToken);
                }
                else//获取字符串
                {
                    string userMsg = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                    string content = $"{DateTime.Now.ToString("yyyyMMdd-HHmmss:fff")} {this._UserName} 发送了：{userMsg}";
                    //await ChatManager.SendMessage(cancellationToken, content);
                }
            }
        }
    }
}
