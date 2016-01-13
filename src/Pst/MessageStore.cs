using Pst.Internal;
using Pst.Internal.Ltp;
using System.Linq;
using System.Text;

namespace Pst
{
    public class MessageStore
    {
        private readonly PropertyContext _context;

        internal MessageStore(PstReader reader)
        {
            var block = reader.ReadBlock(0x21);
            _context = new PropertyContext(block, reader);
        }

        public byte[] RecordKey
        {
            get { return _context.Get(PropertyKey.RecordKey).ToArray(); }
        }

        public string DisplayName
        {
            get
            {
                var bytes = _context.Get(PropertyKey.DisplayName).ToArray();
                return Encoding.Unicode.GetString(bytes);
            }
        }

        public Folder RootFolder { get; set; }
        public Folder SearchFolder { get; set; }
        public Folder RecycleBin { get; set; }
    }
}
