using Pst;
using System.Collections.ObjectModel;
using System.Linq;

namespace PstEditor
{
    public class FolderModel
    {
        private Folder _folder;

        public FolderModel(Folder folder)
        {
            _folder = folder;
        }

        public string Name
        {
            get { return _folder.Name; }
        }

        public int MessageCount
        {
            get { return _folder.ItemCount; }
        }

        public ObservableCollection<FolderModel> Folders
        {
            get
            {
                if (_folder.HasSubfolders)
                    return new ObservableCollection<FolderModel>(_folder.Folders.Select(f => new FolderModel(f)));
                return null;
            }
        }
    }
}
