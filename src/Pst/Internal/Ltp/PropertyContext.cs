using System;
using System.Collections.Generic;
using System.Linq;
using Pst.Internal.Ndb;

namespace Pst.Internal.Ltp
{
    internal class PropertyContext
    {
        private readonly Node _node;
        private Heap _heap;
        private BTree<Property, ushort> _bTree;
        private IPstReader _pstReader;

        internal PropertyContext(
            Node node,
            IPstReader reader)
        {
            _node = node;
            _pstReader = reader;
            Initialize();
        }

        internal IList<byte> Get(PropertyKey key)
        {
            var prop = _bTree.Find((ushort)key);
            if (prop == null)
                return null;

            if (prop.Type.IsVariableLength())
            {
                if ((prop.Hnid & 0x1f) == 0)
                    return _heap[prop.Hnid];
                return _pstReader.FindBlock(prop.Hnid).Data;
            }

            if (prop.Type.GetLength() <= 4)
                return BitConverter.GetBytes(prop.Hnid).ToList();

            return _heap[prop.Hnid];
        }

        private void Initialize()
        {
            var block = _pstReader.FindBlock(_node.DataBid);
            _heap = new Heap(block);
            _bTree = new BTree<Property, ushort>(
                _heap,
                b => BitConverter.ToUInt16(b.ToArray(), 0),
                CreateProperty);
        }

        private Property CreateProperty(IList<byte> bytes)
        {
            var buffer = bytes.ToArray();
            return new Property
            {
                Key = (PropertyKey)BitConverter.ToUInt16(buffer, 0),
                Type = (PropertyType)BitConverter.ToUInt16(buffer, 2),
                Hnid = BitConverter.ToUInt32(buffer, 4)
            };
        }
    }
}
