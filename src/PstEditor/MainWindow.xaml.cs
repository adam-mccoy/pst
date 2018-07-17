using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Pst;

namespace PstEditor
{
    public partial class MainWindow : Window
    {
        private PstFile _currentFile;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = ".pst",
                Filter = "Outlook files|*.pst|All files|*.*"
            };
            if (dialog.ShowDialog().GetValueOrDefault())
                OpenFile(dialog.FileName);
        }

        private void OpenFile(string path)
        {
            _currentFile = new PstFile(File.OpenRead(path));
            PopulateFolders();
        }

        private void PopulateFolders()
        {
            var store = _currentFile.MessageStore;
            var root = store.RootFolder;
            var folders = new ObservableCollection<FolderModel>(root.Folders.Select(f => new FolderModel(f)));
            DataContext = new PstModel { Folders = folders };
        }
    }
}
