using System;
using System.Collections.Generic;
using System.Linq;
using Pst.Internal.Ndb;

namespace Pst.Internal.Ltp
{
    internal class PropertyContext
    {
        private Heap _heap;
        private BTree<Property, ushort> _bTree;
        private IPstReader _pstReader;

        internal PropertyContext(
            Block block,
            IPstReader reader)
        {
            _heap = new Heap(block);
            _pstReader = reader;
            _bTree = new BTree<Property, ushort>(
                _heap,
                b => BitConverter.ToUInt16(b.ToArray(), 0),
                CreateProperty);
        }

        internal IList<byte> Get(PropertyKey key)
        {
            var prop = _bTree.Find((ushort)key);
            if (prop == null)
                return null;

            if ((prop.Hnid & 0x1f) == 0)
                return _heap[prop.Hnid];

            return _pstReader.ReadBlock(prop.Hnid).Data;
        }

        private Property CreateProperty(IList<byte> bytes)
        {
            var buffer = bytes.ToArray();
            return new Property
            {
                Key = BitConverter.ToUInt16(buffer, 0),
                Type = BitConverter.ToUInt16(buffer, 2),
                Hnid = BitConverter.ToUInt32(buffer, 4)
            };
        }
    }
}
