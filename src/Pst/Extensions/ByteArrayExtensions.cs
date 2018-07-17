using Pst.Internal;

namespace Pst.Extensions
{
    internal static class ByteArrayExtensions
    {
        internal static Segment<byte> Segment(this byte[] array) => Segment(array, 0, array.Length);

        internal static Segment<byte> Segment(this byte[] array, int offset, int count) => new Segment<byte>(array, offset, count);
    }
}
