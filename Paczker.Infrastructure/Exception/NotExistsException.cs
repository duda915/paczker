namespace Paczker.Infrastructure.Exception
{
    public class NotExistsException : System.Exception
    {
        public NotExistsException(string message) : base(message)
        {
        }
    }
    
    public class MissingVersionException : System.Exception
    {
        public MissingVersionException(string message) : base(message)
        {
        }
    }
}