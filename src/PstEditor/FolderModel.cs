using System.Collections.ObjectModel;
using System.Linq;
using Pst;

namespace PstEditor
{
    public class FolderModel
    {
        private Folder _folder;

        public FolderModel(Folder folder)
        {
            _folder = folder;
        }

        public string Name => _folder.Name;

        public int MessageCount => _folder.ItemCount;

        public ObservableCollection<FolderModel> Folders
        {
            get
            {
                if (_folder.HasSubfolders)
                    return new ObservableCollection<FolderModel>(_folder.Folders.Select(f => new FolderModel(f)));
                return null;
            }
        }

        public ObservableCollection<MessageModel> Messages => new ObservableCollection<MessageModel>(_folder.Messages.Select(m => new MessageModel(m)));
    }
}
