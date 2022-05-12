using System;
using System.Runtime.Serialization;

namespace ExternalCServices.Exceptions
{
    public  class ExternalClientServiceException : Exception
    {
        public ExternalClientServiceException()
        {
        }

        public ExternalClientServiceException(string message) : base(message)
        {
        }
    }
}