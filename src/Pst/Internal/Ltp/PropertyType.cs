namespace Pst.Internal.Ltp
{
    internal enum PropertyType : ushort
    {
        Integer16 = 0x02,
        Integer32 = 0x03,
        Floating32 = 0x04,
        Floating64 = 0x05,
        Currency = 0x06,
        FloatingTime = 0x07,
        ErrorCode = 0x0a,
        Boolean = 0x0b,
        Integer64 = 0x14,
        String = 0x1f,
        String8 = 0x1e,
        Time = 0x40,
        Guid = 0x48,
        ServerId = 0xfb,
        Restriction = 0xfd,
        RuleAction = 0xfe,
        Binary = 0x102,
        MultipleInteger16 = 0x1002,
        MultipleInteger32 = 0x1003,
        MultipleFloating32 = 0x1004,
        MultipleFloating64 = 0x1005,
        MultipleCurrency = 0x1006,
        MultipleFloatingTime = 0x1007,
        MultipleInteger64 = 0x1014,
        MultipleString = 0x101f,
        MultipleString8 = 0x101e,
        MultipleTime = 0x1040,
        MultipleGuid = 0x1048,
        MultipleBinary = 0x1102,
        Unspecified = 0x00,
        Null = 0x01,
        Object = 0x0d
    }
}
