using Pst.Internal;

namespace Pst
{
    public class MessageStore
    {
        internal MessageStore(PstReader reader)
        {

        }

        public byte[] RecordKey { get; set; }
        public string DisplayName { get; set; }

        public Folder RootFolder { get; set; }
        public Folder SearchFolder { get; set; }
        public Folder RecycleBin { get; set; }
    }
}
