using System;

namespace Mayiboy.IM.Contract
{
    /// <summary>
    /// 响应基类接口
    /// </summary>
    public interface IResponse
    {

    }

    /// <summary>
    /// 响应基类
    /// </summary>
    [Serializable]
    public class Response : IResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public Response()
        {
            IsSuccess = true;
        }

        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 返回Code
        /// </summary>
        public string MessageCode { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string MessageText { get; set; }
    }
}