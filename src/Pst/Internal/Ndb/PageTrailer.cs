namespace Pst.Internal.Ndb
{
    internal class PageTrailer
    {
        public PageTrailer(
            PageType type,
            ushort signature,
            uint crc,
            ulong bid)
        {
            Type = type;
            Signature = signature;
            Crc = crc;
            Bid = bid;
        }

        internal PageType Type { get; private set; }
        internal ushort Signature { get; private set; }
        internal uint Crc { get; private set; }
        internal ulong Bid { get; private set; }
    }
}
