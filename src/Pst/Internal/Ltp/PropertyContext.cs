using System;
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

        internal Segment<byte> Get(PropertyKey key)
        {
            var prop = _bTree.Find((ushort)key);
            if (prop == null)
                return null;

            if (prop.Type.IsVariableLength())
            {
                if ((prop.Hnid & 0x1f) == 0)
                    return _heap[prop.Hnid];
                return _node.FindSubnode(prop.Hnid).GetDataBlock().Data;
            }

            if (prop.Type.GetLength() <= 4)
                return BitConverter.GetBytes(prop.Hnid);

            return _heap[prop.Hnid];
        }

        private void Initialize()
        {
            var block = _node.GetDataBlock();
            _heap = new Heap(block);
            _bTree = new BTree<Property, ushort>(
                _heap,
                b => BitConverter.ToUInt16(b.Array, b.Offset),
                CreateProperty);
        }

        private Property CreateProperty(Segment<byte> bytes) => new Property
        {
            Key = (PropertyKey)BitConverter.ToUInt16(bytes.Array, bytes.Offset),
            Type = (PropertyType)BitConverter.ToUInt16(bytes.Array, bytes.Offset + 2),
            Hnid = BitConverter.ToUInt32(bytes.Array, bytes.Offset + 4)
        };
    }
}
