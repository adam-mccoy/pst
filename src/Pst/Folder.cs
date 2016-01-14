using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pst.Internal;
using Pst.Internal.Ltp;
using Pst.Internal.Ndb;

namespace Pst
{
    public class Folder
    {
        private readonly Nid _nid;
        private readonly IPstReader _pstReader;

        private PropertyContext _properties;

        internal Folder(Nid nid, IPstReader reader)
        {
            _nid = nid;
            _pstReader = reader;
            Initialize();
        }

        public string Name
        {
            get { return Encoding.Unicode.GetString(_properties.Get(PropertyKey.DisplayName).ToArray()); }
        }

        public int ItemCount
        {
            get { return BitConverter.ToInt32(_properties.Get(PropertyKey.ContentCount).ToArray(), 0); }
        }

        public int UnreadCount
        {
            get { return BitConverter.ToInt32(_properties.Get(PropertyKey.UnreadCount).ToArray(), 0); }
        }

        public bool HasSubfolders
        {
            get { return BitConverter.ToBoolean(_properties.Get(PropertyKey.Subfolders).ToArray(), 0); }
        }

        public ICollection<Folder> Folders { get; set; }

        private void Initialize()
        {
            var node = _pstReader.FindNode(_nid);
            _properties = new PropertyContext(node, _pstReader);

            var htNode = _pstReader.FindNode(Nid.ChangeType(_nid, NidType.HierarchyTable));
        }
    }
}
