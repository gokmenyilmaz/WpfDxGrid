using DevExpress.Xpf.Grid;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfApp1
{
    public class HandleToIndexConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var handle = (int)values[0];
            var grid = (GridControl)values[1];
            return grid.GetRowVisibleIndexByHandle(handle).ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            w1.DeleteRow(w1.FocusedRowHandle);
        }
    }
}