namespace Pst.Internal.Ndb
{
    internal class BbtEntry
    {
        public BbtEntry(Bref bref, ushort length, ushort refCount)
        {
            Bref = bref;
            Length = length;
            RefCount = refCount;
        }

        public BbtEntry(Segment<byte> data)
        {
            Bref = new Bref(data.ToUInt64(0), data.ToUInt64(8));
            Length = data.ToUInt16(16);
            RefCount = data.ToUInt16(18);
        }

        internal Bref Bref { get; private set; }
        internal ushort Length { get; private set; }
        internal ushort RefCount { get; private set; }
    }
}
