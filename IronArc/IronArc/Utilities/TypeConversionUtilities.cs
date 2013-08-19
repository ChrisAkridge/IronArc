using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc.Utilities
{
    public static class TypeConversionUtilities
    {
        #region Boolean-to Conversions
        public static bool ToBool(bool value)
        {
            return value;
        }

        public static byte ToByte(bool value)
        {
            if (value)
            {
                return 0x80;
            }
            return 0x00;
        }

        public static sbyte ToSByte(bool value)
        {
            if (value)
            {
                return -128;
            }
            return 0;
        }

        public static ushort ToUShort(bool value)
        {
            if (value)
            {
                return 0x8000;
            }
            return 0x0000;
        }

        public static short ToShort(bool value)
        {
            if (value)
            {
                return -32768;
            }
            return 0;
        }

        public static uint ToUInt(bool value)
        {
            if (value)
            {
                return 0x80000000;
            }
            return 0x00000000;
        }

        public static int ToInt(bool value)
        {
            if (value)
            {
                return -2147483648;
            }
            return 0;
        }

        public static ulong ToULong(bool value)
        {
            if (value)
            {
                return 0x8000000000000000L;
            }
            return 0x0000000000000000L;
        }

        public static long ToLong(bool value)
        {
            if (value)
            {
                return -9223372036854775808L;
            }
            return 0L;
        }

        public static float ToSingle(bool value)
        {
            byte[] bytes = new byte[4];
            if (value)
            {
                bytes[0] = 0x80;
            }
            return BitConverter.ToSingle(bytes, 0);
        }

        public static double ToDouble(bool value)
        {
            byte[] bytes = new byte[8];
            if (value)
            {
                bytes[0] = 0x80;
            }
            return BitConverter.ToDouble(bytes, 0);
        }

        public static decimal ToDecimal(bool value)
        {
            if (value)
            {
                return 0.0000000000000000000000000001m;
            }
            return 0m;
        }

        public static char ToChar(bool value)
        {
            if (value)
            {
                return '\u8000';
            }
            return '\u0000';
        }

        public static string ToString(bool value)
        {
            return new string(new [] { TypeConversionUtilities.ToChar(value) });
        }
        #endregion

        #region Byte-to Conversions
        public static bool[] ToBoolArray(byte value)
        {
            byte[] byteArray = new[] { value };
            BitArray bitArray = new BitArray(byteArray);

            bool[] result = new bool[8];
            bitArray.CopyTo(result, 0);
            result = result.Reverse().ToArray();
            return result;
        }

        public static byte ToByte(byte value)
        {
            return value;
        }

        public static sbyte ToSByte(byte value)
        {
            return unchecked((sbyte)value);
        }

        public static ushort ToUShort(byte value)
        {
            return value;
        }

        public static short ToShort(byte value)
        {
            return value;
        }

        public static uint ToUInt(byte value)
        {
            return value;
        }

        public static int ToInt(byte value)
        {
            return value;
        }

        public static ulong ToULong(byte value)
        {
            return value;
        }

        public static long ToLong(byte value)
        {
            return value;
        }

        public static float ToSingle(byte value)
        {
            return (float)value;
        }

        public static float ToSingleBinary(byte value)
        {
            byte[] bytes = new byte[] { value, 0, 0, 0 };
            return BitConverter.ToSingle(bytes, 0);
        }

        public static double ToDouble(byte value)
        {
            return (double)value;
        }

        public static double ToDoubleBinary(byte value)
        {
            byte[] bytes = new byte[] { value, 0, 0, 0, 0, 0, 0, 0 };
            return BitConverter.ToDouble(bytes, 0);
        }

        public static decimal ToDecimal(byte value)
        {
            return (decimal)value;
        }

        public static char ToChar(byte value)
        {
            return (char)(ushort)value;
        }

        public static string ToString(byte value)
        {
            return TypeConversionUtilities.ToChar(value).ToString();
        }
        #endregion

        #region UShort-to Conversions
        public static bool[] ToBoolArray(ushort value)
        {
            bool[] result = new bool[16];
            byte[] bytes = TypeConversionUtilities.ToByteArray(value);

            TypeConversionUtilities.ToBoolArray(bytes[0]).CopyTo(result, 0);
            TypeConversionUtilities.ToBoolArray(bytes[1]).CopyTo(result, 8);

            return result;
        }

        public static byte[] ToByteArray(ushort value)
        {
            byte[] result = new byte[2];
            byte high = (byte)(value >> 8);
            byte low = (byte)(value & (byte)255);
            result[0] = high;
            result[1] = low;
            return result;
        }

        public static ushort ToUShort(ushort value)
        {
            return value;
        }

        public static short ToShort(ushort value)
        {
            return unchecked((short)value);
        }

        public static uint ToUInt(ushort value)
        {
            return value;
        }

        public static int ToInt(ushort value)
        {
            return value;
        }

        public static ulong ToULong(ushort value)
        {
            return value;
        }

        public static long ToLong(ushort value)
        {
            return value;
        }

        public static float ToSingle(ushort value)
        {
            return (float)value;
        }

        public static float ToSingleBinary(ushort value)
        {
            byte[] bytes = TypeConversionUtilities.ToByteArray(value);
            byte[] emptyBytes = new byte[] { 0, 0 };
            Array.Resize(ref bytes, 4);
            emptyBytes.CopyTo(bytes, 2);

            return BitConverter.ToSingle(bytes, 0);
        }

        public static double ToDouble(ushort value)
        {
            return (double)value;
        }

        public static double ToDoubleBinary(ushort value)
        {
            byte[] bytes = TypeConversionUtilities.ToByteArray(value);
            byte[] emptyBytes = new byte[] { 0, 0, 0, 0, 0, 0 };
            Array.Resize(ref bytes, 8);
            emptyBytes.CopyTo(bytes, 2);

            return BitConverter.ToDouble(bytes, 0);
        }

        public static decimal ToDecimal(ushort value)
        {
            return (decimal)value;
        }

        public static char ToChar(ushort value)
        {
            return (char)value;
        }

        public static string ToString(ushort value)
        {
            return TypeConversionUtilities.ToChar(value).ToString();
        }
        #endregion

        #region Short-to Conversions

        #endregion

        #region UInt-to Conversions

        #endregion

        #region Int-to Conversions

        #endregion

        #region ULong-to Conversions

        #endregion

        #region Single-to Conversions

        #endregion

        #region Double-to Conversions

        #endregion

        #region Decimal-to Conversons

        #endregion

        #region Char-To Conversions

        #endregion

        #region String-to Conversions

        #endregion
    }
}
