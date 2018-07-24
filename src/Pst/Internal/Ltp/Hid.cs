using System.Diagnostics;

namespace Pst.Internal.Ltp
{
    [DebuggerDisplay("Type: {Type}, Index: {Index}, Block Index: {BlockIndex}")]
    internal struct Hid
    {
        private uint _value;

        public Hid(uint value)
        {
            _value = value;
        }

        public int Type => (int)(_value & 0x1f);
        public int Index => (int)(_value >> 5 & 0x1f);
        public int BlockIndex => (int)(_value >> 16);

        public static implicit operator Hid(uint value) => new Hid(value);
        public static implicit operator uint(Hid hid) => hid._value;
    }
}
