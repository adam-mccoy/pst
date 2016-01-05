namespace Pst.Internal.Ndb
{
    internal class Bref
    {
        public Bref(ulong bid, ulong ib)
        {
            Bid = bid;
            Ib = ib;
        }

        internal ulong Bid { get; private set; }
        internal ulong Ib { get; private set; }
    }
}
