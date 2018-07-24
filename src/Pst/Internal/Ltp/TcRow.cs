using System.IO;
using System.Linq;
using Pst.Internal.Ndb;

namespace Pst.Internal.Ltp
{
    internal class TcRow
    {
        private readonly int _rowIndex;
        private readonly TcColumnOffsets _offsets;
        private readonly TcColumnDef[] _columnDefs;
        private readonly Segment<byte> _rowData;
        private readonly Heap _heap;
        private readonly Node _node;

        internal TcRow(
            int rowIndex,
            TcColumnOffsets offsets,
            TcColumnDef[] columnDefs,
            Segment<byte> rowData,
            Heap heap,
            Node node)
        {
            _rowIndex = rowIndex;
            _offsets = offsets;
            _columnDefs = columnDefs;
            _rowData = rowData;
            _heap = heap;
            _node = node;
        }

        internal Segment<byte>? GetCell(PropertyKey key)
        {
            var colDef = _columnDefs.FirstOrDefault(d => d.Tag.Key == key);
            if (colDef == null)
                return null;

            if (!ColumnExists(colDef.CebIndex))
                return null;

            var type = colDef.Tag.Type;
            Segment<byte> data;
            var offset = colDef.Offset + (_rowIndex * _offsets.Bm);
            if (!type.IsVariableLength() && type.GetLength() <= 8)
            {
                var length = colDef.Tag.Type.GetLength();
                data = _rowData.Derive(offset, length);
            }
            else
            {
                var hnid = _rowData.ToUInt32(offset);
                if ((hnid & 0x1f) == 0)
                {
                    data = _heap[hnid];
                }
                else
                {
                    var subnode = _node.FindSubnode(hnid);
                    var subStream = subnode.GetDataStream();
                    data = new BinaryReader(subStream).ReadBytes((int)subStream.Length);
                }
            }

            return data;
        }

        private bool ColumnExists(int cebIndex)
        {
            var offset = _offsets.Ones + (_rowIndex * _offsets.Bm);
            return (_rowData[offset + cebIndex / 8] & (1 << (7 - (cebIndex % 8)))) != 0;
        }
    }
}
