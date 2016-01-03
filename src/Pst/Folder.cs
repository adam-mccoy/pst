using System;
using System.Collections.Generic;

namespace Pst
{
    public class Folder
    {
        public long Id { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public int ItemCount { get; set; }
        public int UnreadCount { get; set; }
        public bool HasSubfolders { get; set; }
        public int TotalSize { get; set; }
        public long TotalSizeExtended { get; set; }
        public DateTime LastModified { get; set; }

        public ICollection<Folder> Folders { get; set; }
    }
}
