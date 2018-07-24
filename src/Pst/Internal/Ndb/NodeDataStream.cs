using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pst.Extensions;

namespace Pst.Internal.Ndb
{
    internal class NodeDataStream : Stream
    {
        private readonly Bid _bid;
        private readonly IPstReader _reader;

        private State _state;

        public NodeDataStream(Bid bid, IPstReader reader)
        {
            _bid = bid;
            _reader = reader;
        }

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => false;

        public override long Length
        {
            get
            {
                EnsureInitialized();
                return _state.TotalLength;
            }
        }

        public override long Position { get; set; }

        public override void Flush() => throw new NotSupportedException();

        public override int Read(byte[] buffer, int offset, int count)
        {
            ValidateReadArguments(buffer, offset, count);
            EnsureInitialized();

            var totalBytesRemaining = _state.TotalLength - Position;
            if (totalBytesRemaining <= 0)
                return -1;

            var totalBytesToCopy = Math.Min(count, totalBytesRemaining);
            var treeItems = _state.FindBlocks(Position, totalBytesToCopy);

            foreach (var ti in treeItems)
            {
                var block = _reader.FindBlock(ti.Bid);
                var blockOffset = (int)(Position - ti.Offset);
                var blockCount = (int)Math.Min(block.Length - blockOffset, totalBytesToCopy);
                Buffer.BlockCopy(block.Data, blockOffset, buffer, offset, blockCount);
                Position += blockCount;
                offset += blockCount;
                totalBytesToCopy -= blockCount;
            }

            return offset;
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        private void ValidateReadArguments(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (count < 0 || count > buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (offset < 0 || offset + count > buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
        }

        private void ValidateSeekArguments(long value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
        }

        private void EnsureInitialized()
        {
            if (_state != null)
                return;

            _state = new State(_bid, _reader);
        }

        private class State
        {
            private readonly IPstReader _reader;
            private readonly List<DataTreeItem> _dataTree = new List<DataTreeItem>();

            public long TotalLength { get; private set; }

            public State(Bid initialBid, IPstReader reader)
            {
                _reader = reader;

                var block = _reader.FindBlock(initialBid);
                if (initialBid.Type == BlockType.External)
                {
                    _dataTree.Add(new DataTreeItem(0, initialBid));
                    TotalLength = block.Length;
                }
                else if (block.Data[0] == 0x01) // data tree
                {
                    TotalLength = block.Data.Segment(4, 4).ToUInt32();
                    long offset = 0;
                    BuildDataTree(block, ref offset);
                }
                else
                {
                    throw new Exception("Invalid block data");
                }
            }

            private void BuildDataTree(Block block, ref long offset)
            {
                var level = block.Data[1];
                if (level != 0x01 && level != 0x02)
                    throw new Exception("Invalid level");

                var numEntities = block.Data.Segment(2, 2).ToUInt16();
                for (var i = 0; i < numEntities; i++)
                {
                    var bid = new Bid(block.Data.Segment(8 + i * 8, 8).ToUInt64());
                    if (level == 0x02)
                    {
                        var b = _reader.FindBlock(bid);
                        BuildDataTree(b, ref offset);
                    }
                    else
                    {
                        var b = _reader.LookupBlock(bid);
                        _dataTree.Add(new DataTreeItem(offset, bid));
                        offset += b.Length;
                    }
                }
            }

            internal IEnumerable<DataTreeItem> FindBlocks(long offset, long count)
            {
                var dataTreeIndex = _dataTree.Select(t => t.Offset).ToArray();
                var firstIndex = Array.BinarySearch(dataTreeIndex, offset);
                if (firstIndex < 0)
                    firstIndex = ~firstIndex - 1;
                var lastIndex = Array.BinarySearch(dataTreeIndex, offset + count);
                if (lastIndex < 0)
                    lastIndex = ~lastIndex;

                return _dataTree.GetRange(firstIndex, lastIndex - firstIndex);
            }
        }

        private struct DataTreeItem
        {
            public DataTreeItem(long offset, Bid bid)
            {
                Offset = offset;
                Bid = bid;
            }

            public long Offset { get; set; }
            public Bid Bid { get; set; }
        }
    }
}
