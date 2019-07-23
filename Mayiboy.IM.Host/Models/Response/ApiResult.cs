namespace Mayiboy.IM.Host.Models
{
    /// <summary>
    /// Result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T>
    {
        /// <summary>
        /// 状态（0：成功；1：参数有误；-1：系统处理异常）
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 响应数据
        /// </summary>
        public T Data { get; set; }
    }
}