using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Pst.Internal;

namespace PstDebugger
{
    public partial class MainWindow : Window
    {
        private PstReader _pstReader;

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
            {
                var stream = File.OpenRead(dialog.FileName);
                _pstReader = new PstReader(stream);
            }
        }

        private void NodeSearch_Click(object sender, RoutedEventArgs e)
        {
            var nid = uint.Parse(nodeSearch.Text, NumberStyles.HexNumber);
            var node = _pstReader.FindNode(nid);
            if (node == null)
                MessageBox.Show("Node not found.");

            blockSearch.Text = node.DataBid.Value.ToString("X");
        }

        private void BlockSearch_Click(object sender, RoutedEventArgs e)
        {
            var bid = uint.Parse(blockSearch.Text, NumberStyles.HexNumber);
            var block = _pstReader.FindBlock(bid);
            if (block == null)
                MessageBox.Show("Block not found.");

            _hexView.Stream = new MemoryStream(block.Data);
        }
    }
}
