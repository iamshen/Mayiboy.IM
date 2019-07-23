namespace Mayiboy.IM.Utils.Exception
{
    /// <summary>
    /// Logic 异常
    /// </summary>
    public class LogicException : BaseException
    {
        public LogicException(int code, string message) : base(code, message)
        {
        }

        public LogicException(string message) : base(message)
        {
        }

        public LogicException(object data) : base(data)
        {
        }
    }
}