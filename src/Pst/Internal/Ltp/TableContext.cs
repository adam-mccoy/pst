using System;
using System.Linq;
using Pst.Internal.Ndb;

namespace Pst.Internal.Ltp
{
    internal class TableContext
    {
        private readonly Node _node;
        private readonly IPstReader _reader;

        private Heap _heap;
        private int _numColumns;
        private ushort[] _rgib = new ushort[4];
        private BTree<uint, uint> _rowIndex;

        internal TableContext(
            Node node,
            IPstReader reader)
        {
            _node = node;
            _reader = reader;
            Initialize();
        }

        internal TcIndexItem[] Index
        {
            get
            {
                return _rowIndex.GetAll().Select(t => new TcIndexItem { RowKey = t.Item1, RowIndex = t.Item2 }).ToArray();
            }
        }

        internal TcIndexItem SearchIndex(uint key)
        {
            var index = _rowIndex.Find(key);
            return new TcIndexItem
            {
                RowKey = key,
                RowIndex = index
            };
        }

        private void Initialize()
        {
            var block = _reader.FindBlock(_node.DataBid);
            _heap = new Heap(block);
            var tableHeader = _heap[_heap.UserRoot];
            _numColumns = tableHeader[1];
            for (int i = 0; i < 4; i++)
                _rgib[i] = BitConverter.ToUInt16(tableHeader.Array, tableHeader.Offset + 2 + (i * 2));

            var rowIndexHid = BitConverter.ToUInt32(tableHeader.Array, tableHeader.Offset + 10);
            _rowIndex = new BTree<uint, uint>(
                _heap,
                rowIndexHid,
                s => BitConverter.ToUInt32(s.Array, s.Offset),
                s => BitConverter.ToUInt32(s.Array, s.Offset + 4));
        }
    }
}
