using System;

namespace Pst.Internal.Ndb
{
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

        internal NidType Type
        {
            get { return (NidType)(_value & 0x1f); }
        }

        internal uint Index
        {
            get { return _value >> 5; }
        }

        internal static Nid ChangeType(Nid nid, NidType type)
        {
            return new Nid(type, nid.Index);
        }

        public static implicit operator Nid(uint value)
        {
            return new Nid(value);
        }

        public static implicit operator uint(Nid nid)
        {
            return nid._value;
        }

        private static void ValidateNid(uint value)
        {
            if (!Enum.IsDefined(typeof(NidType), value & 0x1f))
                throw new Exception("Invalid NID type.");
        }
    }
}
