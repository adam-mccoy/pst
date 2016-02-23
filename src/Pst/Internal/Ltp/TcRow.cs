using System;

namespace Pst.Internal.Ltp
{
    internal class TcRow
    {
        private ushort[] _rgib;
        private TcColumnDef[] _columnDefs;

        internal TcRow(ushort[] rgib, TcColumnDef[] columnDefs)
        {
            _rgib = rgib;
            _columnDefs = columnDefs;
        }

        internal Segment<byte> GetCell(PropertyKey key)
        {
            throw new NotImplementedException();
        }
    }
}
