namespace Pst.Internal
{
    internal enum PageType : byte
    {
        BBT = 0x80,
        NBT = 0x081,
        FMap = 0x82,
        PMap = 0x83,
        AMap = 0x84,
        FPMap = 0x85,
        DL = 0x86
    }
}
