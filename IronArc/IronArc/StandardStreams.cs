using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
    /// <summary>
    /// Contains the three standard console streams: in, out, and error.
    /// </summary>
	[Obsolete]
    public static class StandardStreams
    {
        public static Stream StreamIn { get; private set; }
        public static Stream StreamOut { get; private set; }
        public static Stream StreamError { get; private set; }

        static StandardStreams()
        {
            StreamIn = new Stream(1024);
            StreamOut = new Stream(1024);
            StreamError = new Stream(1024);
        }
        
        public static Stream GetStream(byte streamNumber)
        {
            switch (streamNumber)
            {
                case 0:
                    return StreamIn;
                case 1:
                    return StreamOut;
                case 2:
                    return StreamError;
                default:
                    new SystemError("InvalidStreamNumber", string.Format("There is no standard stream with the number {0}.", streamNumber)).WriteToError();
                    return null;
            }
        }
    }
}
