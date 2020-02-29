using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.DataContext = new MainWindowVM();

            InitializeComponent();

            //g1.PastingFromClipboard += G1_PastingFromClipboard;

            //w1.ClipboardRowPasting += W1_ClipboardRowPasting;
            //w1.ClipboardRowCellValuePasting += W1_ClipboardRowCellValuePasting;
        }

        private void G1_PastingFromClipboard(object sender, DevExpress.Xpf.Grid.PastingFromClipboardEventArgs e)
        {
        }

        private void W1_ClipboardRowPasting(object sender, DevExpress.Xpf.Grid.ClipboardRowPastingEventArgs e)
        {
        }

        private void W1_ClipboardRowCellValuePasting(object sender, DevExpress.Xpf.Grid.ClipboardRowCellValuePastingEventArgs e)
        {
        }
    }
}