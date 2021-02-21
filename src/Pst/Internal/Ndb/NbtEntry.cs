namespace Pst.Internal.Ndb
{
    internal class NbtEntry
    {
        public NbtEntry(Segment<byte> data)
        {
            Nid = data.ToUInt32(0);
            DataBid = data.ToUInt64(8);
            SubnodeBid = data.ToUInt64(16);
            ParentNid = data.ToUInt32(24);
        }

        internal Nid Nid { get; }
        internal ulong DataBid { get; }
        internal ulong SubnodeBid { get; }
        internal Nid ParentNid { get; }
    }
}
