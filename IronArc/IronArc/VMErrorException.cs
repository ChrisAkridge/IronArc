using System;
// ReSharper disable InconsistentNaming

namespace IronArc
{
    public sealed class VMErrorException : Exception
    {
        public Error Error { get; }
        public string DefaultMessage => ErrorMessages.GetDefaultMessage(Error);

        public VMErrorException(Error error) => Error = error;

        public VMErrorException(Error error, string message) : base(message) => Error = error;
    }
}
