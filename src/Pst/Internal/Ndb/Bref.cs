namespace Pst.Internal.Ndb
{
    internal struct Bref
    {
        public Bref(ulong bid, ulong ib)
        {
            Bid = bid;
            Ib = ib;
        }

        internal ulong Bid { get; private set; }
        internal ulong Ib { get; private set; }

        public override bool Equals(object obj) => obj is Bref bref && Bid == bref.Bid && Ib == bref.Ib;

        public override int GetHashCode()
        {
            var hashCode = -1068386286;
            hashCode = hashCode * -1521134295 + Bid.GetHashCode();
            hashCode = hashCode * -1521134295 + Ib.GetHashCode();
            return hashCode;
        }
    }
}
