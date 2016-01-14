namespace Pst.Internal.Ndb
{
    internal enum NidType : uint
    {
        Hid = 0x00,
        Internal = 0x01,
        NormalFolder = 0x02,
        SearchFolder = 0x03,
        NormalMessage = 0x04,
        Attachment = 0x05,
        SearchUpdateQueue = 0x06,
        SearchCriteriaObject = 0x07,
        AssociatedMessage = 0x08,
        ContentsTableIndex = 0x0a,
        ReceiveFolderTable = 0x0b,
        OutgoingQueueTable = 0x0c,
        HierarchyTable = 0x0d,
        ContentsTable = 0x0e,
        AssociatedContentsTable = 0x0f,
        SearchContentsTable = 0x10,
        AttachmentTable = 0x11,
        RecipientTable = 0x12,
        SearchTableIndex = 0x13,
        Ltp = 0x1f
    }
}
