using System;
using System.Collections.Generic;
using System.Linq;
using Pst.Internal;
using Pst.Internal.Ltp;
using Pst.Internal.Ndb;

namespace Pst
{
    public class Folder
    {
        private readonly Nid _nid;
        private readonly IPstReader _pstReader;

        private readonly Lazy<PropertyContext> _properties;
        private readonly Lazy<TableContext> _hierarchy;
        private readonly Lazy<TableContext> _contents;

        internal Folder(Nid nid, IPstReader reader)
        {
            _nid = nid;
            _pstReader = reader;

            _properties = new Lazy<PropertyContext>(GetPropertyContext);
            _hierarchy = new Lazy<TableContext>(GetHierarchyTable);
            _contents = new Lazy<TableContext>(GetContentsTable);
        }

        public string Name => _properties.Value.Get(PropertyKey.DisplayName)?.ToString(_pstReader);
        public int ItemCount => _properties.Value.Get(PropertyKey.ContentCount)?.ToInt32() ?? 0;
        public int UnreadCount => _properties.Value.Get(PropertyKey.UnreadCount)?.ToInt32() ?? 0;
        public bool HasSubfolders => _properties.Value.Get(PropertyKey.Subfolders)?.ToBoolean() ?? false;

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
                var messages = index.Select(i => new Message(i.RowKey, _pstReader)).ToList();
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

        private PropertyContext GetPropertyContext()
        {
            var node = _pstReader.FindNode(_nid);
            return new PropertyContext(node, _pstReader);
        }
    }
}
