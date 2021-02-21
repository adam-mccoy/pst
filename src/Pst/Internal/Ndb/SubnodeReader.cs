namespace Pst.Internal.Ndb
{
    internal class SubnodeReader
    {
        private readonly Bid _bid;
        private readonly IPstReader _reader;

        internal SubnodeReader(Bid bid, IPstReader reader)
        {
            _bid = bid;
            _reader = reader;
        }

        internal Node FindSubnode(Nid nid)
        {
            var block = _reader.FindBlock(_bid);

            var level = block.Data[1];
            if (level > 0)
            {
                var siEntry = FindIntermediateEntry(block, nid);
                if (siEntry == null)
                    return null;

                block = _reader.FindBlock(siEntry.Bid);
            }

            var slEntry = FindLeafEntry(block, nid);
            return slEntry == null ? null : new Node(nid, slEntry.DataBid, slEntry.SubnodeBid, _reader);
        }

        private static SlEntry FindLeafEntry(Block block, Nid nid)
        {
            var numEntries = block.Data.Slice(2).ToUInt16();

            for (var i = 0; i < numEntries; i++)
            {
                var entryNid = block.Data.Slice(8 + i * 24).ToUInt32();
                if (entryNid == nid)
                {
                    return new SlEntry
                    {
                        Nid = block.Data.Slice(8 + i * 24).ToUInt32(),
                        DataBid = block.Data.Slice(16 + i * 24).ToUInt64(),
                        SubnodeBid = block.Data.Slice(24 + i * 24).ToUInt64()
                    };
                }
            }

            return null;
        }

        private static SiEntry FindIntermediateEntry(Block block, Nid nid)
        {
            var numEntries = block.Data.Slice(2).ToUInt16();

            for (var i = 0; i < numEntries; i++)
            {
                var entryNid = block.Data.Slice(8 + i * 16).ToUInt32();
                if (entryNid > nid)
                {
                    return new SiEntry
                    {
                        Nid = block.Data.Slice(8 + (i - 1) * 16).ToUInt32(),
                        Bid = block.Data.Slice(16 + (i - 1) * 16).ToUInt64()
                    };
                }
            }

            return null;
        }

        private class SiEntry
        {
            internal Nid Nid { get; set; }
            internal ulong Bid { get; set; }
        }

        private class SlEntry
        {
            internal Nid Nid { get; set; }
            internal ulong DataBid { get; set; }
            internal ulong SubnodeBid { get; set; }
        }
    }
}
