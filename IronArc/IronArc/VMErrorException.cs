using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
