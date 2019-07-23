using System;

namespace Mayiboy.IM.Utils.Exception
{
    /// <summary>
    /// 自定义异常
    /// </summary>
    public class BaseException : ApplicationException
    {
        public readonly int ExceptionCode = -1;
        public readonly string ExceptionMessage;
        public readonly object ResultData;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public BaseException(int code, string message)
        {
            this.ExceptionCode = code;
            this.ExceptionMessage = message;
        }

        public BaseException(string message)
        {
            this.ExceptionMessage = message;
        }

        public BaseException(object data)
        {
            this.ResultData = data;
        }
    }
}