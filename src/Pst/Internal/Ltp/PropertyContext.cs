using System;
using System.IO;
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
                var subnode = _node.FindSubnode(prop.Hnid);
                var dataStream = subnode.GetDataStream();
                return new BinaryReader(dataStream).ReadBytes((int)dataStream.Length);
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
                b => b.ToUInt16(),
                CreateProperty);
        }

        private Property CreateProperty(Segment<byte> bytes) => new Property
        {
            Key = (PropertyKey)bytes.ToUInt16(),
            Type = (PropertyType)bytes.ToUInt16(2),
            Hnid = bytes.ToUInt32(4)
        };
    }
}
