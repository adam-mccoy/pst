using System;
using System.Diagnostics;

namespace Pst.Internal.Ltp
{
    [DebuggerDisplay("Type: {Type}, Index: {Index}, Block Index: {BlockIndex}")]
    internal struct Hid
    {
        internal static Hid Zero = new Hid(0);

        private uint _value;

        public Hid(uint value)
        {
            _value = value;
        }

        public int Type
        {
            get
            {
                Validate();
                return (int)(_value & 0x1f);
            }
        }

        public int Index
        {
            get
            {
                Validate();
                return (int)(_value >> 5 & 0x1f);
            }
        }

        public int BlockIndex
        {
            get
            {
                Validate();
                return (int)(_value >> 16);
            }
        }

        public static implicit operator Hid(uint value) => new Hid(value);
        public static implicit operator uint(Hid hid) => hid._value;
        public static bool operator ==(Hid hid1, Hid hid2) => hid1.Equals(hid2);
        public static bool operator !=(Hid hid1, Hid hid2) => !(hid1 == hid2);

        public override bool Equals(object obj) =>
            obj is Hid hid && hid._value == _value;

        public override int GetHashCode() => -1939223833 + _value.GetHashCode();

        private void Validate()
        {
            if ((_value & 0x1f) != 0) throw new Exception("Invalid HID Type");
            if ((_value >> 5 & 0x1f) == 0) throw new Exception("Invalid HID Index");
        }
    }
}
