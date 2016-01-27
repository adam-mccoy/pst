using Pst.Internal;

namespace Pst.Extensions
{
    internal static class ByteArrayExtensions
    {
        internal static Segment<byte> Segment(this byte[] array)
        {
            return Segment(array, 0, array.Length);
        }

        internal static Segment<byte> Segment(this byte[] array, int offset, int count)
        {
            return new Segment<byte>(array, offset, count);
        }
    }
}
