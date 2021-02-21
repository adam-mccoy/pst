namespace Pst.Internal
{
    internal static class ByteArrayExtensions
    {
        internal static Segment<byte> Slice(this byte[] array, int offset) => new Segment<byte>(array, offset, array.Length - offset);
        internal static Segment<byte> Slice(this byte[] array, int offset, int count) => new Segment<byte>(array, offset, count);
    }
}
