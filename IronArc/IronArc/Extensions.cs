using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// ReSharper disable InconsistentNaming

namespace IronArc
{
    public static class Extensions
    {
        public static byte[] ToLPString(this string text)
        {
            int textLength = Encoding.UTF8.GetByteCount(text);
            return BitConverter.GetBytes(textLength).Concat(Encoding.UTF8.GetBytes(text)).ToArray();
        }

        public static ulong GetSizeInBytes(this OperandSize size)
        {
            switch (size)
            {
                case OperandSize.Byte: return 1UL;
                case OperandSize.Word: return 2UL;
                case OperandSize.DWord: return 4UL;
                case OperandSize.QWord: return 8UL;
                default:
                    throw new ArgumentException($"Implementation error: Invalid size {size}");
            }
        }

        // https://stackoverflow.com/a/27238358/2709212
        public static unsafe float ToFloatBitwise(this uint bits) => *(float*)&bits;

        // https://stackoverflow.com/a/27238358/2709212
        public static unsafe uint ToUIntBitwise(this float bits) => *(uint*)&bits;

        public static T TryDequeue<T>(this Queue<T> queue) => queue.Count > 0 ? queue.Dequeue() : default;

        public static IEnumerable<T> AsEnumerable<T>(this T item)
        {
            yield return item;
        }
    }
}
