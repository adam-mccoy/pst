using System;
using System.Collections.Generic;
using Pst.Internal.Ndb;
using Pst.Extensions;

namespace Pst.Internal.Ltp
{
    internal class Heap
    {
        private readonly ushort _mapOffset;
        private readonly Block _block;
        private readonly ushort[] _allocations;

        public Heap(Block block)
        {
            _block = block;
            _mapOffset = BitConverter.ToUInt16(_block.Data, 0);

            _allocations = new ushort[AllocatedCount + 1];
            for (var i = 0; i < AllocatedCount + 1; i++)
                _allocations[i] = BitConverter.ToUInt16(_block.Data, _mapOffset + 4 + i * 2);
        }

        internal byte ClientSignature
        {
            get { return _block.Data[3]; }
        }

        internal uint UserRoot
        {
            get { return BitConverter.ToUInt32(_block.Data, 4); }
        }

        internal int AllocatedCount
        {
            get { return BitConverter.ToUInt16(_block.Data, _mapOffset); }
        }

        internal int FreedCount
        {
            get { return BitConverter.ToUInt16(_block.Data, _mapOffset + 2); }
        }

        internal IList<byte> this[uint hid]
        {
            get
            {
                var i = (hid >> 5) - 1;
                var offset = _allocations[i];
                return _block.Data.Segment(offset, _allocations[i + 1] - offset);
            }
        }
    }
}
