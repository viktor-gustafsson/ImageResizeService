using System;

namespace ImageResizeService.Infrastructure.Exceptions
{
    public class InvalidImageFormatException : Exception
    {
        public InvalidImageFormatException()
        {
        }

        public InvalidImageFormatException(string message) :
            base(message)
        {
        }

        public InvalidImageFormatException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}