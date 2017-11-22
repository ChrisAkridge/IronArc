using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
    /// <summary>
    /// The type for errors that occur in the VM's processor.
    /// </summary>
	[Obsolete]
    public sealed class SystemError
    {
        public string Type { get; private set; }
        public string Message { get; private set; }

        public SystemError(string type, string message)
        {
			Type = type;
			Message = message;
        }

        public void WriteToError()
        {
            Stream error = StandardStreams.StreamError;
            error.WriteString(string.Format("Error {0}: {1} ({2})", Type.GetHashCode(), Message, Type));
        }
    }
}
