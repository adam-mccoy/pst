using System;

namespace Pst.Internal
{
    internal static class SegmentConversionExtensions
    {
        public static bool ToBoolean(this Segment<byte> segment) => BitConverter.ToBoolean(segment.Array, segment.Offset);
        public static bool ToBoolean(this Segment<byte> segment, int offset) => BitConverter.ToBoolean(segment.Array, segment.Offset + offset);
        public static short ToInt16(this Segment<byte> segment) => BitConverter.ToInt16(segment.Array, segment.Offset);
        public static short ToInt16(this Segment<byte> segment, int offset) => BitConverter.ToInt16(segment.Array, segment.Offset + offset);
        public static int ToInt32(this Segment<byte> segment) => BitConverter.ToInt32(segment.Array, segment.Offset);
        public static int ToInt32(this Segment<byte> segment, int offset) => BitConverter.ToInt32(segment.Array, segment.Offset + offset);
        public static long ToInt64(this Segment<byte> segment) => BitConverter.ToInt64(segment.Array, segment.Offset);
        public static long ToInt64(this Segment<byte> segment, int offset) => BitConverter.ToInt64(segment.Array, segment.Offset + offset);
        public static ushort ToUInt16(this Segment<byte> segment) => BitConverter.ToUInt16(segment.Array, segment.Offset);
        public static ushort ToUInt16(this Segment<byte> segment, int offset) => BitConverter.ToUInt16(segment.Array, segment.Offset + offset);
        public static uint ToUInt32(this Segment<byte> segment) => BitConverter.ToUInt32(segment.Array, segment.Offset);
        public static uint ToUInt32(this Segment<byte> segment, int offset) => BitConverter.ToUInt32(segment.Array, segment.Offset + offset);
        public static ulong ToUInt64(this Segment<byte> segment) => BitConverter.ToUInt64(segment.Array, segment.Offset);
        public static ulong ToUInt64(this Segment<byte> segment, int offset) => BitConverter.ToUInt64(segment.Array, segment.Offset + offset);
    }
}
