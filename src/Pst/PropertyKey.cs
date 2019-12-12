namespace Pst
{
    public enum PropertyKey : ushort
    {
        RecordKey = 0xff9,
        DisplayName = 0x3001,
        IpmSubTreeEntryId = 0x35e0,
        IpmWastebasketEntryId = 0x35e5,
        FinderEntryId = 0x35e7,
        ContentCount = 0x3602,
        UnreadCount = 0x3603,
        Subfolders = 0x360a,
        Hidden = 0x10f4,

        LtpRowId = 0x67f2,
        LtpRowVer = 0x67f3,

        Subject = 0x0037,
        Importance = 0x0017,
        Priority = 0x0026,
        MessageClass = 0x001a,
        ReadReceiptsRequested = 0x0029,
        Sensitivity = 0x0036
    }
}
