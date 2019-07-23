namespace Mayiboy.IM.Utils.Exception
{
    /// <summary>
    /// Utils Exception
    /// </summary>
    public class UtilsException : BaseException
    {
        public UtilsException(int code, string message) : base(code, message)
        {
        }

        public UtilsException(string message) : base(message)
        {
        }

        public UtilsException(object data) : base(data)
        {
        }
    }
}