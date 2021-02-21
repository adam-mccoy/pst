using Pst.Internal;
using Pst.Internal.Ltp;

namespace Pst
{
    public class Recipient
    {
        private readonly TcRow _row;
        private readonly IPstReader _pstReader;

        internal Recipient(TcRow row, IPstReader pstReader)
        {
            _row = row;
            _pstReader = pstReader;
        }

        public RecipientType? Type => (RecipientType?) _row.GetCell(PropertyKey.RecipientType)?.ToUInt32();
        public string DisplayName => _row.GetCell(PropertyKey.DisplayName)?.ToString(_pstReader);
        public string EmailAddress => _row.GetCell(PropertyKey.EmailAddress)?.ToString(_pstReader);
    }
}
