using System;
using System.Collections.Generic;
using Pst.Internal.Ndb;

namespace Pst.Internal.Ltp
{
    internal class Heap
    {
        private readonly List<HeapPage> _pages = new List<HeapPage>();
        private readonly Node _node;
        private readonly IPstReader _reader;

        public Heap(Node node, IPstReader reader)
        {
            _node = node;
            _reader = reader;
            Initialize();
        }

        internal byte ClientSignature { get; private set; }

        internal Hid UserRoot { get; private set; }

        internal Segment<byte> this[Hid hid]
        {
            get
            {
                var page = _pages[hid.BlockIndex];
                return page[hid.Index - 1];
            }
        }

        private void Initialize()
        {
            var block = _reader.FindBlock(_node.DataBid);

            if (_node.DataBid.Type == BlockType.Internal)
            {
                BuildDataTreePages(block);
                ClientSignature = _pages[0].Data[3];
                UserRoot = _pages[0].Data.Slice(4, 4).ToUInt32();
            }
            else
            {
                _pages.Add(new HeapPage(block.Data));
                ClientSignature = block.Data[3];
                UserRoot = block.Data.Slice(4, 4).ToUInt32();
            }
        }

        private void BuildDataTreePages(Block block)
        {
            var level = block.Data[1];
            if (level != 0x01 && level != 0x02)
                throw new Exception("Invalid level");

            var numEntities = block.Data.Slice(2, 2).ToUInt16();
            for (var i = 0; i < numEntities; i++)
            {
                var bid = new Bid(block.Data.Slice(8 + i * 8, 8).ToUInt64());
                var innerBlock = _reader.FindBlock(bid);
                if (level == 0x02)
                    BuildDataTreePages(innerBlock);
                else
                    _pages.Add(new HeapPage(innerBlock.Data));
            }
        }
    }
}
