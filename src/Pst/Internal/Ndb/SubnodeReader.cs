using System;

namespace Pst.Internal.Ndb
{
    internal class SubnodeReader
    {
        private ulong _bid;
        private IPstReader _reader;

        internal SubnodeReader(ulong bid, IPstReader reader)
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
            if (slEntry == null)
                return null;

            return new Node(nid, slEntry.DataBid, slEntry.SubnodeBid, _reader);
        }

        private SlEntry FindLeafEntry(Block block, Nid nid)
        {
            var numEntries = BitConverter.ToUInt16(block.Data, 2);

            for (var i = 0; i < numEntries; i++)
            {
                var entryNid = BitConverter.ToUInt32(block.Data, 8 + i * 24);
                if (entryNid == nid)
                {
                    return new SlEntry
                    {
                        Nid = BitConverter.ToUInt32(block.Data, 8 + i * 24),
                        DataBid = BitConverter.ToUInt64(block.Data, 16 + i * 24),
                        SubnodeBid = BitConverter.ToUInt64(block.Data, 24 + i * 24)
                    };
                }
            }

            return null;
        }

        private SiEntry FindIntermediateEntry(Block block, Nid nid)
        {
            var numEntries = BitConverter.ToUInt16(block.Data, 2);

            for (var i = 0; i < numEntries; i++)
            {
                var entryNid = BitConverter.ToUInt32(block.Data, 8 + i * 16);
                if (entryNid > nid)
                {
                    return new SiEntry
                    {
                        Nid = BitConverter.ToUInt32(block.Data, 8 + (i - 1) * 16),
                        Bid = BitConverter.ToUInt64(block.Data, 16 + (i - 1) * 16)
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
