using System;
using System.Runtime.Serialization;

namespace ExternalCServices.Exceptions
{

    public class ExternalClientServiceNotFoundException : Exception
    {
        public ExternalClientServiceNotFoundException()
        {
        }

        public ExternalClientServiceNotFoundException(string message) : base(message)
        {
        }
    }
}