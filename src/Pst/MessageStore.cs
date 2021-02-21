using Pst.Internal;
using Pst.Internal.Ltp;
using Pst.Internal.Ndb;

namespace Pst
{
    public class MessageStore
    {
        private readonly IPstReader _pstReader;
        private Nid _nid;
        private Node _node;
        private PropertyContext _context;

        internal MessageStore(Nid nid, IPstReader reader)
        {
            _nid = nid;
            _pstReader = reader;
            Initialize();
        }

        public byte[] RecordKey => _context.Get(PropertyKey.RecordKey)?.ToArray();

        public string DisplayName => _context.Get(PropertyKey.DisplayName)?.ToString(_pstReader);

        private Folder _rootFolder;
        public Folder RootFolder
        {
            get
            {
                if (_rootFolder == null)
                {
                    var prop = _context.Get(PropertyKey.IpmSubTreeEntryId);
                    Nid nid = prop.Value.Slice(20, 4);
                    _rootFolder = new Folder(nid, _pstReader);
                }
                return _rootFolder;
            }
        }

        public Folder SearchFolder { get; set; }
        public Folder RecycleBin { get; set; }

        private void Initialize()
        {
            _node = _pstReader.FindNode(_nid);
            _context = new PropertyContext(_node, _pstReader);
        }
    }
}
