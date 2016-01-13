using System;

namespace Pst.Internal.Ndb
{
    internal class NbtEntry : BTreeNode
    {
        internal uint Nid { get; set; }
        internal ulong DataBid { get; set; }
        internal ulong SubnodeBid { get; set; }
        internal uint ParentNid { get; set; }

        internal override void Create(byte[] data)
        {
            Nid = BitConverter.ToUInt32(data, 0);
            DataBid = BitConverter.ToUInt64(data, 8);
            SubnodeBid = BitConverter.ToUInt64(data, 16);
            ParentNid = BitConverter.ToUInt32(data, 24);
        }
    }
}
