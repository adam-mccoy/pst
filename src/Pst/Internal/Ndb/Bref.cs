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

        public override bool Equals(object obj)
        {
            if (obj == this) return true;

            var bref = obj as Bref;
            if (bref == null) return false;

            return bref.Bid == Bid && bref.Ib == Ib;
        }
    }
}
