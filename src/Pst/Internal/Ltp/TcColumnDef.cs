namespace Pst.Internal.Ltp
{
    internal class TcColumnDef
    {
        internal PropertyKey Tag { get; set; }
        internal ushort Offset { get; set; }
        internal ushort Size { get; set; }
        internal byte CebIndex { get; set; }
    }
}
