using System;
using System.Collections.Generic;
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
        private Lazy<TableContext> _contents;

        internal Folder(Nid nid, IPstReader reader)
        {
            _nid = nid;
            _pstReader = reader;
            Initialize();
        }

        public string Name => _pstReader.DecodeString(_properties.Get(PropertyKey.DisplayName));
        public int ItemCount => _properties.Get(PropertyKey.ContentCount).ToInt32();
        public int UnreadCount => _properties.Get(PropertyKey.UnreadCount).ToInt32();
        public bool HasSubfolders => _properties.Get(PropertyKey.Subfolders).ToBoolean();

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

        public ICollection<Message> Messages
        {
            get
            {
                var index = _contents.Value.Index;
                var messages = new Message[index.Length];
                return messages;
            }
        }

        private TableContext GetHierarchyTable()
        {
            var htNode = _pstReader.FindNode(Nid.ChangeType(_nid, NidType.HierarchyTable));
            return new TableContext(htNode, _pstReader);
        }

        private TableContext GetContentsTable()
        {
            var ctNode = _pstReader.FindNode(Nid.ChangeType(_nid, NidType.ContentsTable));
            return new TableContext(ctNode, _pstReader);
        }

        private void Initialize()
        {
            var node = _pstReader.FindNode(_nid);
            _properties = new PropertyContext(node, _pstReader);
            _hierarchy = new Lazy<TableContext>(GetHierarchyTable);
            _contents = new Lazy<TableContext>(GetContentsTable);
        }
    }
}
