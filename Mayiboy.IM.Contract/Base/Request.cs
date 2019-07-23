using System;

namespace Mayiboy.IM.Contract
{
    /// <summary>
    /// 请求基类接口
    /// </summary>
    public interface IRequest
    {

    }

    /// <summary>
    /// 请求基类
    /// </summary>
    [Serializable]
    public class Request : IRequest
    {
        /// <summary>
        /// 应用程序代码
        /// </summary>
        public string AppId { get; set; }
    }
}