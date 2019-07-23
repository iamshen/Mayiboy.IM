using System;
using System.Configuration;
using ServiceStack.Redis;

namespace Mayiboy.IM.Utils
{
    /// <summary>
    /// Redis订阅帮助类
    /// </summary>
    public class RedisHelper
    {
        private static readonly PooledRedisClientManager RedisClientPool;

        static RedisHelper()
        {
            var redishost = ConfigurationManager.AppSettings["redis.server"].ToString();

            RedisClientPool = CreateManager(redishost);
        }

        private static PooledRedisClientManager CreateManager(string redishost)
        {
            string[] writeHosts = redishost.Split(new string[] { ",", "，" }, StringSplitOptions.RemoveEmptyEntries);

            return new PooledRedisClientManager(writeHosts, writeHosts, new RedisClientManagerConfig
            {
                MaxWritePoolSize = 100000,
                MaxReadPoolSize = 100000,
                AutoStart = true,
                DefaultDb = 1,
            });
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public static RedisClient GetRedisClient()
        {
            return (RedisClient)RedisClientPool.GetClient();
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="channel">通道</param>
        /// <param name="onMessage">接受消息</param>
        /// <param name="onSubscribe">订阅</param>
        /// <param name="onUnSubscribe">取消订阅</param>
        public static void Subscription(string channel, Action<string, string> onMessage, Action<string> onSubscribe, Action<string> onUnSubscribe)
        {
            Subscription(onMessage, onSubscribe, onUnSubscribe, channel.ToLower());
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="channels">通道集合</param>
        /// <param name="onMessage">接受消息</param>
        /// <param name="onSubscribe">订阅</param>
        /// <param name="onUnSubscribe">取消订阅</param>
        public static void Subscription(Action<string, string> onMessage, Action<string> onSubscribe, Action<string> onUnSubscribe, params string[] channels)
        {
            using (RedisClient redisClient = (RedisClient)RedisClientPool.GetClient())
            {
                var redis = redisClient.CreateSubscription();
                redis.OnMessage = onMessage;
                redis.OnSubscribe = onSubscribe;
                redis.OnUnSubscribe = onUnSubscribe;
                redis.SubscribeToChannels(channels);
            }
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="fromChannels">通道</param>
        public static void UnSubscription(params string[] fromChannels)
        {
            using (RedisClient redisClient = (RedisClient)RedisClientPool.GetClient())
            {
                redisClient.UnSubscribe(fromChannels);
            }
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="tochannel">通道</param>
        /// <param name="content">内容</param>
        public static long Publish(string tochannel, string content)
        {
            using (RedisClient redisClient = (RedisClient)RedisClientPool.GetClient())
            {
                return redisClient.PublishMessage(tochannel.ToLower(), content);
            }
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="tochannel">通道</param>
        /// <param name="message">消息</param>
        public static long Publish(string tochannel, byte[] message)
        {
            using (RedisClient redisClient = (RedisClient)RedisClientPool.GetClient())
            {
                return redisClient.Publish(tochannel.ToLower(), message);
            }
        }
    }
}