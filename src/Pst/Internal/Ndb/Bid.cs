using System.Diagnostics;

namespace Pst.Internal.Ndb
{
    [DebuggerDisplay("Type: {Type}, Value: {Value}")]
    internal struct Bid
    {
        internal Bid(ulong value)
        {
            Value = value;
        }

        internal BlockType Type => (BlockType)(Value & 0x02);

        internal ulong Value { get; }

        public static implicit operator Bid(ulong value) => new Bid(value);

        public static implicit operator ulong(Bid bid) => bid.Value;
    }
}
