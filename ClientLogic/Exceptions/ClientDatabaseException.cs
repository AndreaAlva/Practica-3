using System;
using System.Runtime.Serialization;

namespace ClientLogic.Exceptions
{
    public class ClientDatabaseException : Exception
    {
        public ClientDatabaseException()
        {
        }

        public ClientDatabaseException(string message) : base(message)
        {
        }
    }
}