using System;
using System.Diagnostics;

namespace Pst.Internal.Ndb
{
    [DebuggerDisplay("Type: {Type}, Index: {Index}")]
    internal struct Nid
    {
        private uint _value;

        internal Nid(uint value)
        {
            ValidateNid(value);
            _value = value;
        }

        private Nid(NidType type, uint index)
        {
            _value = (uint)type | index << 5;
        }

        internal NidType Type => (NidType)(_value & 0x1f);

        internal uint Index => _value >> 5;

        internal static Nid ChangeType(Nid nid, NidType type) => new Nid(type, nid.Index);

        public static implicit operator Nid(uint value) => new Nid(value);

        public static implicit operator Nid(Segment<byte> bytes)
        {
            var value = BitConverter.ToUInt32(bytes.Array, bytes.Offset);
            return new Nid(value);
        }

        public static implicit operator uint(Nid nid) => nid._value;

        private static void ValidateNid(uint value)
        {
            if (!Enum.IsDefined(typeof(NidType), value & 0x1f))
                throw new Exception("Invalid NID type.");
        }
    }
}
