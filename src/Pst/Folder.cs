using System;
using System.Collections.Generic;
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
        private Lazy<TableContext> _hierarchy;

        internal Folder(Nid nid, IPstReader reader)
        {
            _nid = nid;
            _pstReader = reader;
            Initialize();
        }

        public string Name
        {
            get
            {
                var prop = _properties.Get(PropertyKey.DisplayName);
                return Encoding.Unicode.GetString(prop.Array, prop.Offset, prop.Count);
            }
        }

        public int ItemCount
        {
            get
            {
                var prop = _properties.Get(PropertyKey.ContentCount);
                return BitConverter.ToInt32(prop.Array, prop.Offset);
            }
        }

        public int UnreadCount
        {
            get
            {
                var prop = _properties.Get(PropertyKey.UnreadCount);
                return BitConverter.ToInt32(prop.Array, prop.Offset);
            }
        }

        public bool HasSubfolders
        {
            get
            {
                var prop = _properties.Get(PropertyKey.Subfolders);
                return BitConverter.ToBoolean(prop.Array, prop.Offset);
            }
        }

        public ICollection<Folder> Folders
        {
            get
            {
                if (_nid.Type != NidType.NormalFolder)
                    throw new InvalidOperationException();

                var index = _hierarchy.Value.Index;
                var folders = new Folder[index.Length];
                for (var i = 0; i < index.Length; i++)
                    folders[i] = new Folder(index[i].RowKey, _pstReader);
                return folders;
            }
        }

        private TableContext GetHierarchyTable()
        {
            var htNode = _pstReader.FindNode(Nid.ChangeType(_nid, NidType.HierarchyTable));
            return new TableContext(htNode, _pstReader);
        }

        private void Initialize()
        {
            var node = _pstReader.FindNode(_nid);
            _properties = new PropertyContext(node, _pstReader);
            _hierarchy = new Lazy<TableContext>(GetHierarchyTable);
        }
    }
}
