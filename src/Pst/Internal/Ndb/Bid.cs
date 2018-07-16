namespace Pst.Internal.Ndb
{
    internal struct Bid
    {
        internal Bid(ulong value)
        {
            Value = value;
        }

        internal BlockType Type => (BlockType)(Value & 0x02);

        internal ulong Value { get; }

        public override string ToString() => $"Type: {Type}, Value: 0x{Value:x8}";

        public static implicit operator Bid(ulong value) => new Bid(value);

        public static implicit operator ulong(Bid bid) => bid.Value;
    }
}
