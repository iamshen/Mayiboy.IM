using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mayiboy.IM.Utils
{
    public static class WebSocketHelper
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="websocket"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task SendMessage(this WebSocket websocket, CancellationToken cancellationToken, string content)
        {
            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(content));
            await websocket.SendAsync(buffer, WebSocketMessageType.Text, true, cancellationToken);
        }
    }
}