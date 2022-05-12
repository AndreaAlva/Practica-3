using System;
using System.Runtime.Serialization;

namespace ClientLogic.Exceptions
{
    public class ClientInvalidInputException : Exception
    {
        public ClientInvalidInputException()
        {
        }

        public ClientInvalidInputException(string message) : base(message)
        {
        }
    }
}