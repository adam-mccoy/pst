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

        internal PageType Type { get; }
        internal ushort Signature { get; }
        internal uint Crc { get; }
        internal ulong Bid { get; }
    }
}
