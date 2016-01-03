using System;

namespace Pst.Extensions
{
    internal static class ByteArrayExtensions
    {
        internal static ArraySegment<byte> Segment(this byte[] array, int offset, int count)
        {
            return new ArraySegment<byte>(array, offset, count);
        }
    }
}
