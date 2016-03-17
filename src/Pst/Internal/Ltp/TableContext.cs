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
        private TcColumnDef[] _columnDefs;
        private TcColumnOffsets _columnOffsets;
        private BTree<uint, uint> _rowIndex;
        private Segment<byte> _rowData;

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
                return _rowIndex.GetAll().Select(t => new TcIndexItem { RowKey = t.Key, RowIndex = t.Value }).ToArray();
            }
        }

        internal TcRow[] Rows
        {
            get
            {
                var rowCount = _rowIndex.GetAll().Count();
                var rows = Enumerable.Range(0, rowCount).Select(i =>
                {
                    return new TcRow(i, _columnOffsets, _columnDefs, _rowData, _heap, _node, _reader);
                });

                return rows.ToArray();
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
            _columnOffsets = new TcColumnOffsets(
                BitConverter.ToInt16(tableHeader.Array, tableHeader.Offset + 2),
                BitConverter.ToInt16(tableHeader.Array, tableHeader.Offset + 4),
                BitConverter.ToInt16(tableHeader.Array, tableHeader.Offset + 6),
                BitConverter.ToInt16(tableHeader.Array, tableHeader.Offset + 8));

            var rowIndexHid = BitConverter.ToUInt32(tableHeader.Array, tableHeader.Offset + 10);
            _rowIndex = new BTree<uint, uint>(
                _heap,
                rowIndexHid,
                s => BitConverter.ToUInt32(s.Array, s.Offset),
                s => BitConverter.ToUInt32(s.Array, s.Offset + 4));

            var rowDataHnid = BitConverter.ToUInt32(tableHeader.Array, tableHeader.Offset + 14);
            if ((rowDataHnid & 0x1f) == 0)
            {
                _rowData = _heap[rowDataHnid];
            }
            else
            {
                var subnode = _node.FindSubnode(rowDataHnid);
                var dataBlock = _reader.FindBlock(subnode.DataBid);
                _rowData = dataBlock.Data;
            }

            _columnDefs = new TcColumnDef[_numColumns];
            for (int i = 0; i < _numColumns; i++)
            {
                var offset = tableHeader.Offset + 22 + (i * 8);
                var tagType = BitConverter.ToUInt16(tableHeader.Array, offset);
                var tagKey = BitConverter.ToUInt16(tableHeader.Array, offset + 2);
                var dataOffset = BitConverter.ToUInt16(tableHeader.Array, offset + 4);
                var dataLength = tableHeader.Array[offset + 6];
                var cebIndex = tableHeader.Array[offset + 7];
                var def = new TcColumnDef
                {
                    Tag = new ColumnTag
                    {
                        Type = (PropertyType)tagType,
                        Key = (PropertyKey)tagKey
                    },
                    Offset = dataOffset,
                    Size = dataLength,
                    CebIndex = cebIndex
                };
                _columnDefs[i] = def;
            }
        }
    }
}
