using System;

namespace Pst.Internal.Ndb
{
    internal class BbtEntry : BTreeNode
    {
        internal Bref Bref { get; private set; }
        internal ushort Length { get; private set; }
        internal ushort RefCount { get; private set; }

        internal override void Create(byte[] data)
        {
            Bref = new Bref(BitConverter.ToUInt64(data, 0), BitConverter.ToUInt64(data, 8));
            Length = BitConverter.ToUInt16(data, 16);
            RefCount = BitConverter.ToUInt16(data, 18);
        }
    }
}
