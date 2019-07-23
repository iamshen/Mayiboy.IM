using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Framework.Mayiboy.Utility;
using Mayiboy.IM.Model.Enum;
using Mayiboy.IM.Utils;
using ServiceStack.Redis;

namespace Mayiboy.IM.Host
{
    /// <summary>
    /// 消息管理
    /// </summary>
    public class ChatManager
    {
        private System.Timers.Timer _timer = new System.Timers.Timer();     //定时器
        private long heartrate = 6;                                        //(单位s)
        private bool _stat = false;                                         //订阅状态

        private RedisClient _redisClient;                                    //Redis连接
        private IRedisSubscription _redisSubscription;
        private CancellationToken _cancellationToken;                       //取消操作
        private WebSocket _webSocket;                                       //通信对象
        private string _channelid;                                          //通道
        private string _imuserid;                                           //用户标识
        private string _socketid;                                           //通信标识
        private string _sourcetype;                                         //来源类型
        private DateTime _heartbeatTime;                                    //心跳时间    


        #region Property
        /// <summary>
        /// 通讯标识
        /// </summary>
        public string SocketId => _socketid;

        /// <summary>
        /// 用户标识
        /// </summary>
        public string ImUserId => _imuserid;

        /// <summary>
        /// 通道标识
        /// </summary>
        public string ChannelId => _channelid;

        /// <summary>
        /// 来源类型
        /// </summary>
        public string SourceType => _sourcetype;

        /// <summary>
        /// 传播有关应取消操作的通知的标记
        /// </summary>
        public CancellationToken CancelToken => _cancellationToken;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelid">通道</param>
        /// <param name="imuserid">用户标识</param>
        /// <param name="socket">通信对象</param>
        /// <param name="sourcetype">来源类型</param>
        private ChatManager(string channelid, string imuserid, WebSocket socket, string sourcetype)
        {
            this._redisClient = RedisHelper.GetRedisClient();
            this._socketid = Guid.NewGuid().ToString().ToLower();
            this._cancellationToken = new CancellationToken();
            this._webSocket = socket;
            this._channelid = channelid.ToLower();
            this._sourcetype = sourcetype;
            this._imuserid = imuserid.ToLower();

            this._timer.Enabled = true;
            this._timer.Interval = 1000 * heartrate;//5分钟
            this._timer.Start();
            this._timer.Elapsed += Checklive;
            this._heartbeatTime = DateTime.Now;
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="channelid">通道标识</param>
        /// <param name="imuserid">用户标识</param>
        /// <param name="socket">通信对象</param>
        /// <param name="sourcetype">来源类型</param>
        /// <returns></returns>
        public static ChatManager CreateInstance(string channelid, string imuserid, WebSocket socket, string sourcetype)
        {
            var instance = new ChatManager(channelid, imuserid, socket, sourcetype);
            instance.Subscription();
            return instance;
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        private void Subscription()
        {
            Task.Run(() =>
            {
                try
                {
                    _redisSubscription = _redisClient.CreateSubscription();
                    _redisSubscription.OnMessage = this.OnMessage;
                    _redisSubscription.OnSubscribe = this.OnSubscribe;
                    _redisSubscription.OnUnSubscribe = this.OnUnSubscribe;
                    _redisSubscription.SubscribeToChannels(this._channelid, this._socketid);
                }
                catch (Exception ex)
                {
                    LogManager.DefaultLogger.ErrorFormat($"[ThreadId：{Thread.CurrentThread.ManagedThreadId}]订阅异常：{ex.Message}");
                }
            });
        }

        /// <summary>
        /// 消息事件
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        private void OnMessage(string channel, string msg)
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[2048]);
            buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg));
            try
            {
                if (channel.ToLower() == _socketid && msg == "1")
                {
                    Debug.WriteLine("TCP保活----------------------------");
                    return;
                }

                if (_webSocket != null && _webSocket.State == WebSocketState.Open)
                {
                    _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, _cancellationToken);
                }
            }
            catch (ObjectDisposedException ex)
            {
                _redisClient.Dispose();
                _redisSubscription.Dispose();
                ClearTimer();
                LogManager.DefaultLogger.ErrorFormat($"异常订阅线程ID:{Thread.CurrentThread.ManagedThreadId.ToString()};对象已释放:errormsg:{ex.Message}");
            }
            catch (Exception ex)
            {
                _redisClient.Dispose();
                _redisSubscription.Dispose();
                ClearTimer();
                LogManager.DefaultLogger.ErrorFormat($"异常订阅线程ID:{Thread.CurrentThread.ManagedThreadId.ToString()};channel:{channel};errormsg:{ex.Message}");
            }
            finally
            {
                if (channel.ToLower() == _socketid && msg == "stop")
                {
                    _redisSubscription.UnSubscribeFromAllChannels(); //解除封锁线程 
                    _redisClient.Dispose();
                    _redisSubscription.Dispose();
                }
            }
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="channel"></param>
        private void OnSubscribe(string channel)
        {
            _stat = true;
            Debug.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId}；开始订阅：{channel}");
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="channel"></param>
        private void OnUnSubscribe(string channel)
        {
            Debug.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId}；取消订阅：{channel}");
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="content"></param>
        public void PublishMessage(string content)
        {
            #region 检查是否订阅成功5秒等待时间 0.1秒检查操作
            int num = 0;
            while (!_stat)
            {
                num++;
                Thread.Sleep(100);
                if (_stat || num == 50)
                {
                    break;
                }
            }
            #endregion

            try
            {
                RedisHelper.Publish(_channelid, content);
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat($"异常订阅线程ID:{Thread.CurrentThread.ManagedThreadId.ToString()};channel:{_channelid};content:{content};发布消息异常:{ex.Message}");
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="content"></param>
        public void SendMessage(string content)
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[2048]);
            buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(content));

            try
            {
                if (_webSocket != null && _webSocket.State != WebSocketState.Aborted)
                {
                    _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, _cancellationToken).Wait(_cancellationToken);
                }
            }
            catch (Exception ex)
            {
                //TODO待优化处理
                LogManager.DefaultLogger.ErrorFormat($"异常线程ID：{Thread.CurrentThread.ManagedThreadId.ToString()}发送消息;channel:{_channelid};消息：{content},errormsg:{ex.Message}");
            }
        }

        /// <summary>
        /// 答复心跳
        /// </summary>
        public void SendHeartbeat()
        {
            string key = _imuserid.AddCachePrefix("Online");
            if (CacheManager.RedisChat.Get(key).ToInt32() == 0)
            {
                //发送上线通知
                PublishMessage(NewsTypeHelper.ToOnline(_imuserid, _sourcetype, _socketid));
            }

            CacheManager.RedisChat.Set(key, "1", (int)heartrate);
            _heartbeatTime = DateTime.Now;
            SendMessage(NewsTypeHelper.ToHeartbeat());
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <returns></returns>
        public void ClosedConnect()
        {
            try
            {
                //清除定时器
                ClearTimer();

                //取消订阅
                RedisHelper.Publish(_socketid, "stop");

                //移除在线状态
                CacheManager.RedisChat.Del(_imuserid.AddCachePrefix("Online"));

                //发送离线通知
                PublishMessage(NewsTypeHelper.ToOffline(_imuserid, _sourcetype));

                //TODO:webSocket 移除处理待优化
                if (_webSocket != null)
                {
                    _webSocket.CloseAsync(WebSocketCloseStatus.Empty, "", _cancellationToken);
                    _webSocket.Abort();
                    _webSocket = null;
                }
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat($"关闭连接异常：error:{ex.Message}");
                _redisClient.Dispose();
            }
        }

        /// <summary>
        /// 检查存活
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void Checklive(object source, ElapsedEventArgs e)
        {
            RedisHelper.Publish(_socketid, "1");
            if ((DateTime.Now - _heartbeatTime).TotalSeconds > heartrate)
            {
                ClosedConnect();
            }
        }

        #region 清除定时器
        /// <summary>
        /// 清除定时器
        /// </summary>
        public void ClearTimer()
        {
            try
            {
                _timer.Elapsed -= Checklive;
                _timer.Stop();
                _timer.Close();
                _timer.Dispose();
            }
            catch
            {
                Thread.Sleep(1000 * 3);//休息3秒再清除定时器
                try
                {
                    _timer.Elapsed -= Checklive;
                    _timer.Stop();
                    _timer.Close();
                    _timer.Dispose();
                }
                catch (Exception ex)
                {
                    LogManager.DefaultLogger.DebugFormat("ClearTimer Error:{0}", new { _imuserid, _channelid, _sourcetype, err = ex.ToString() }.ToJson());
                }
            }
        }

        #endregion

    }
}