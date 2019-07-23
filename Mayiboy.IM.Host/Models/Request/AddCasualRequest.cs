using System;

namespace Mayiboy.IM.Host.Models
{
    /// <summary>
    /// 添加临时用户参数
    /// </summary>
    public class AddCasualRequest
    {
        /// <summary>
        /// 用户头像地址
        /// </summary>
        public string UserHeadimg { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string ImUserName { get; set; }
    }

    /// <summary>
    /// 添加临时用户响应
    /// </summary>
    public class AddCasualResponse
    {
        /// <summary>
        /// 即时通讯用户标识
        /// </summary>
        public Guid ImUserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string ImUserName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserHeadimg { get; set; }

    }
}