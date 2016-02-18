using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace PstEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog().GetValueOrDefault())
                OpenFile(dialog.FileName);
        }

        private void OpenFile(string path)
        {
        }
    }
}
