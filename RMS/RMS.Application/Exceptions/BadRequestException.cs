using System;

namespace RMS.Application.Exceptions
{
    // By creating our own custom Exception class, we can easily tell our Middleware
    // exactly what kind of error happened without guessing!
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}
