using System.IO;
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

        internal TableContext(Node node, IPstReader reader)
        {
            _node = node;
            _reader = reader;
            Initialize();
        }

        internal TcIndexItem[] Index => _rowIndex.GetAll().Select(t => new TcIndexItem { RowKey = t.Key, RowIndex = t.Value }).ToArray();

        internal TcRow[] Rows
        {
            get
            {
                var rowCount = _rowIndex.GetAll().Count();
                var rows = Enumerable.Range(0, rowCount).Select(i => new TcRow(i, _columnOffsets, _columnDefs, _rowData, _heap, _node));

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
            _heap = new Heap(_node, _reader);
            var tableHeader = _heap[_heap.UserRoot];
            _numColumns = tableHeader[1];
            _columnOffsets = new TcColumnOffsets(
                tableHeader.ToInt16(2),
                tableHeader.ToInt16(4),
                tableHeader.ToInt16(6),
                tableHeader.ToInt16(8));

            var rowIndexHid = tableHeader.ToUInt32(10);
            _rowIndex = new BTree<uint, uint>(
                _heap,
                rowIndexHid,
                s => s.ToUInt32(),
                s => s.ToUInt32(4));

            var rowDataHnid = tableHeader.ToUInt32(14);
            if (rowDataHnid != 0)
            {
                if ((rowDataHnid & 0x1f) == 0)
                {
                    _rowData = _heap[rowDataHnid];
                }
                else
                {
                    var subnode = _node.FindSubnode(rowDataHnid);
                    var dataStream = subnode.GetDataStream();
                    _rowData = new BinaryReader(dataStream).ReadBytes((int)dataStream.Length);
                }
            }

            _columnDefs = new TcColumnDef[_numColumns];
            for (var i = 0; i < _numColumns; i++)
            {
                var offsetBase = 22 + (i * 8);
                var tagType = tableHeader.ToUInt16(offsetBase);
                var tagKey = tableHeader.ToUInt16(offsetBase + 2);
                var dataOffset = tableHeader.ToUInt16(offsetBase + 4);
                var dataLength = tableHeader[offsetBase + 6];
                var cebIndex = tableHeader[offsetBase + 7];
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
