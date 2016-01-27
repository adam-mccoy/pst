﻿using Pst.Internal;
using Pst.Internal.Ltp;
using Pst.Internal.Ndb;
using System.Linq;
using System.Text;

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

        public byte[] RecordKey
        {
            get
            {
                return _context.Get(PropertyKey.RecordKey).ToArray();
            }
        }

        public string DisplayName
        {
            get
            {
                var prop = _context.Get(PropertyKey.DisplayName);
                return Encoding.Unicode.GetString(prop.Array, prop.Offset, prop.Count);
            }
        }

        public Folder RootFolder { get; set; }
        public Folder SearchFolder { get; set; }
        public Folder RecycleBin { get; set; }

        private void Initialize()
        {
            _node = _pstReader.FindNode(_nid);
            _context = new PropertyContext(_node, _pstReader);
        }
    }
}
