using System;

namespace Paczker.Infrastructure.Exceptions
{
    public class NothingFoundException : Exception
    {
        public NothingFoundException(string message) : base(message)
        {
        }
    }
}