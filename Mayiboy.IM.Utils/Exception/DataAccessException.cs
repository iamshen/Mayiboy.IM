namespace Mayiboy.IM.Utils.Exception
{
    /// <summary>
    /// DataAccess Exception
    /// </summary>
    public class DataAccessException : BaseException
    {
        public DataAccessException(int code, string message) : base(code, message)
        {
        }

        public DataAccessException(string message) : base(message)
        {
        }

        public DataAccessException(object data) : base(data)
        {
        }
    }
}