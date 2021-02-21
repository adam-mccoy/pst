namespace Pst.Internal.Ndb
{
    internal struct Bref
    {
        public Bref(Bid bid, ulong ib)
        {
            Bid = bid;
            Ib = ib;
        }

        internal Bid Bid { get; }
        internal ulong Ib { get; }

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
