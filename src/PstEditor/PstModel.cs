using System.Collections.ObjectModel;

namespace PstEditor
{
    public class PstModel
    {
        public ObservableCollection<FolderModel> Folders { get; set; }
    }
}
