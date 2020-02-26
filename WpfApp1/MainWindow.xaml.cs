﻿using DevExpress.Xpf.Bars;
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

        public int Indirim { get; set; }

    }

    public partial class MainWindow : Window
    {
     

        public MainWindow()
        {
            InitializeComponent();



         

            var personelListe = new List<Personel>()
            {
                new Personel { Ad="gökmen",Soyad="a",Yas=23,Indirim=0},
                new Personel { Ad = "musa", Soyad = "b", Yas = 44,Indirim=0 },
                new Personel { Ad = "ayhan", Soyad = "c", Yas = 233,Indirim=0 },
                new Personel { Ad = "faruk", Soyad = "d", Yas = 44,Indirim=0 },
                new Personel { Ad = "izzet", Soyad = "e", Yas = 233,Indirim=0 }
            };


            g1.ItemsSource = personelListe;



            w1.PreviewKeyDown += w1_PreviewKeyDown;
          

        }

        private void w1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                w1.MoveNextRow();
            }

            if (e.Key == Key.Left && g1.CurrentColumn.VisibleIndex == 0) e.Handled = true;
            
            if (e.Key == Key.Right && (g1.CurrentColumn.VisibleIndex == w1.VisibleColumns.Count - 1)) e.Handled = true; 
         
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
                    w1.FocusedRowHandle = row.RowHandle;

                    for (int i = 0; i < clipboardData.Count; i++)
                    {
                        
                        for (int j = 0; j < cColLength; j++)
                        {
                            g1.SetCellValue(w1.FocusedRowHandle, w1.VisibleColumns[first_col_index + j], clipboardData[i][j]);
                        }

                        w1.FocusedRowHandle++;
                    }
                }

            }
            w1.FocusedRowHandle--;
        }

        private void copyCellDataItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridCellMenuInfo menuInfo = w1.GridMenu.MenuInfo as GridCellMenuInfo;
            if (menuInfo != null && menuInfo.Row != null)
            {
                string text = "" +
                    g1.GetCellValue(menuInfo.Row.RowHandle.Value, menuInfo.Column as GridColumn).ToString();
                Clipboard.SetText(text);
            }
        }

        private void deleteRowItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridCellMenuInfo menuInfo = w1.GridMenu.MenuInfo as GridCellMenuInfo;
            if (menuInfo != null && menuInfo.Row != null)
                w1.DeleteRow(menuInfo.Row.RowHandle.Value);
        }

        private void paste_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }
    }
}
