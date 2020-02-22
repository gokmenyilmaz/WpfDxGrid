using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    public static class SendKeys1
    {
        public static void Send(Key key)
        {
            if (Keyboard.PrimaryDevice != null)
            {
                if (Keyboard.PrimaryDevice.ActiveSource != null)
                {
                    KeyEventArgs k = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, key)
                    { RoutedEvent = Keyboard.KeyDownEvent };

                    InputManager.Current.ProcessInput(k);
                }
            }
        }
    }

    public class Personel
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public int Yas { get; set; }

    }

    public partial class MainWindow : Window
    {
     

        public MainWindow()
        {
            InitializeComponent();

            //t1.KeyDown += T1_KeyDown;
            //t1.PreviewKeyDown += T1_PreviewKeyDown;

            g1.PreviewKeyDown += G1_PreviewKeyDown;

            var personelListe = new List<Personel>()
            {
                new Personel { Ad="gökmen",Soyad="a",Yas=23},
                new Personel { Ad = "musa", Soyad = "b", Yas = 44 },
                new Personel { Ad = "ayhan", Soyad = "c", Yas = 233 },
                new Personel { Ad = "faruk", Soyad = "d", Yas = 44 },
                new Personel { Ad = "izzet", Soyad = "e", Yas = 233 }
            };


            g1.ItemsSource = personelListe;

            g1.CurrentColumnChanged += G1_CurrentColumnChanged;

            w1.PreviewKeyDown += W1_PreviewKeyDown;
          

        }

        private void G1_CurrentColumnChanged(object sender, DevExpress.Xpf.Grid.CurrentColumnChangedEventArgs e)
        {
            if (!e.NewColumn.TabStop)
                w1.MoveNextCell();
        }

        private void G1_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            //if (e.Key == Key.Enter)
            //{
            //    //w1.PostEditor();

            //    //SendKeys1.Send(Key.Down);
            //    w1.MoveNextRow();



            //    //e.Handled = true;
            //}

            //if (e.Key == Key.Enter)
            //{
            //    Dispatcher.BeginInvoke(new Action(() =>
            //    {
            //        w1.CommitEditing();
            //        //w1.MoveNextRow();
            //        //g1.SelectedItem = g1.GetRow(w1.FocusedRowHandle);
            //        //e.Handled = true;
            //    }));
            //}

            if (e.Key == Key.Enter)
            {
       
                if (g1.VisibleRowCount == w1.FocusedRowHandle + 1 && g1.CurrentColumn == w1.VisibleColumns.Last() || 
                    g1.VisibleRowCount == 0)
                {
                    w1.AddNewRow();
                    w1.FocusedRowHandle = GridControl.NewItemRowHandle; 
                   

                    e.Handled = true;
                }
            }
        }

        private void W1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
           
        }

    

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

           
        }

        private void g1_PastingFromClipboard(object sender, PastingFromClipboardEventArgs e)
        {
            var seciliHucreler = w1.GetSelectedCells();
            if (seciliHucreler.Count == 1) return;

            e.Handled = true;
           

            var rawDataStr = Clipboard.GetText();

            List<string[]> clipboardData = new List<string[]>();
            string[] rows = rawDataStr.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in rows)
            {
                clipboardData.Add(item.Split('\t'));
            }

            var first_row_handle = seciliHucreler.First().RowHandle;
            var first_col_index = seciliHucreler.First().Column.VisibleIndex;

            var cColLength = clipboardData.First().Length;

            w1.FocusedRowHandle = first_row_handle;

            foreach (var row in seciliHucreler)
            {
                if (row.RowHandle >= w1.FocusedRowHandle)
                {
                    for (int i = 0; i < clipboardData.Count; i++)
                    {
                        
                        for (int j = 0; j < cColLength; j++)
                        {
                            g1.SetCellValue(w1.FocusedRowHandle, w1.VisibleColumns[first_col_index + j], clipboardData[i][j]);
                        }

                        if (w1.FocusedRowHandle+1 > seciliHucreler.Last().RowHandle) return;
                        w1.FocusedRowHandle++;
                    }
                    
                }

            }
            w1.FocusedRowHandle--;
        }
    }
}
