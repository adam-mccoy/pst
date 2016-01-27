﻿using System;
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

        public ICollection<Folder> Folders { get; set; }

        private void Initialize()
        {
            var node = _pstReader.FindNode(_nid);
            _properties = new PropertyContext(node, _pstReader);

            var htNode = _pstReader.FindNode(Nid.ChangeType(_nid, NidType.HierarchyTable));
        }
    }
}
