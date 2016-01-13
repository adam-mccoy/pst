using Pst.Internal;
using Pst.Internal.Ltp;
using Pst.Internal.Ndb;
using System.Linq;
using System.Text;

namespace Pst
{
    public class MessageStore
    {
        private readonly Node _node;
        private readonly PropertyContext _context;

        internal MessageStore(uint nid, PstReader reader)
        {
            _node = reader.FindNode(nid);
            var block = reader.ReadBlock(_node.DataBid);
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
