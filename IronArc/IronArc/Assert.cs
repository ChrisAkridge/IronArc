using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc
{
    /// <summary>
    /// A helper class allowing less verbose error creation.
    /// </summary>
    public static class Assert
    {
        public static void IsTrue(bool condition, string errorType)
        {
            if (errorType == null)
            {
                new SystemError("AssertIsTrue_NullErrorTypeString", "").WriteToError();
            }

            if (!condition)
            {
                new SystemError(errorType, "").WriteToError();
            }
        }

        public static void IsTrue(bool condition)
        {
            if (!condition)
            {
                throw new Exception();
            }
        }
    }
}
