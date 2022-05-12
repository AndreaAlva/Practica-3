using System;
using System.Runtime.Serialization;

namespace ClientLogic.Exceptions
{
    public class ClientNotFoundException : Exception
    {
        public ClientNotFoundException()
        {
        }

        public ClientNotFoundException(string message) : base(message)
        {
        }
    }
}