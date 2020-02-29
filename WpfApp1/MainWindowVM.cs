using DevExpress.Mvvm;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace WpfApp1
{
    public class Personel
    {
        public string Ad { get; set; }

        public int? KafileNo { get; set; }

        public string Soyad { get; set; }
        public int? Yas { get; set; }

        public int? Indirim { get; set; }
    }

    public class MainWindowVM
    {
        public DelegateCommand<PastingFromClipboardEventArgs> PastingFromClipboardCommand =>
          new DelegateCommand<PastingFromClipboardEventArgs>(OnPastingFromClipboard);

        public DelegateCommand<KeyEventArgs> KeyDownCommand => new DelegateCommand<KeyEventArgs>(OnKeyDown);

        private void OnPastingFromClipboard(PastingFromClipboardEventArgs e)
        {
            e.Handled = true;

            var w1 = (e.Source as GridControl).View as TableView;
            var g1 = w1.Grid;

            var clipboardData = GetClipboardData();

            var selectedRows = w1.GetSelectedRows();
            var selectedCells = w1.GetSelectedCells();

            var first_row_handle = selectedRows.First().RowHandle;
            var first_col_index = selectedCells.First().Column.VisibleIndex;

            var cColLength = clipboardData.First().Length;

            w1.FocusedRowHandle = first_row_handle;

            foreach (var row in selectedRows)
            {
                if (row.RowHandle >= w1.FocusedRowHandle)
                {
                    w1.FocusedRowHandle = row.RowHandle;

                    for (int i = 0; i < clipboardData.Count; i++)
                    {
                        for (int j = 0; j < cColLength; j++)
                        {
                            var column = w1.VisibleColumns[first_col_index + j];
                            g1.SetCellValue(w1.FocusedRowHandle, column, clipboardData[i][j]);

                            OnCellValueChanged(column.FieldName, g1.GetRow(w1.FocusedRowHandle), clipboardData[i][j]);
                        }

                        w1.FocusedRowHandle++;
                    }
                }
            }
            w1.FocusedRowHandle--;
        }

        private void OnCellValueChanged(string fieldName, object row, object value)
        {
            if (fieldName == "KafileNo")
            {
                var perRow = (Personel)row;
                var kafileNo = value.ToString().Length == 0 ? -1 : int.Parse(value.ToString());

                var satir = PersonelListe.Where(c => c.KafileNo == kafileNo).FirstOrDefault();
                if (satir != null)
                {
                    perRow.Yas = satir.Yas;
                }
            }
        }

        public List<string[]> GetClipboardData()
        {
            var rawDataStr = Clipboard.GetText();

            List<string[]> clipboardData = new List<string[]>();
            string[] rows = rawDataStr.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (var item in rows)
            {
                clipboardData.Add(item.Split('\t'));
            }

            return clipboardData;
        }

        private void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var w1 = (e.Source as TableView);

                var cells = w1.GetSelectedCells();

                foreach (var cell in cells)
                {
                    w1.Grid.SetCellValue(cell.RowHandle, cell.Column, null);
                }
            }
        }

        public List<Personel> PersonelListe { get; set; }

        public MainWindowVM()
        {
            PersonelListe = new List<Personel>()
            {
                new Personel { Ad="gökmen",Soyad="a",Yas=23,Indirim=0,KafileNo=100},
                new Personel { Ad = "musa", Soyad = "b", Yas = 44,Indirim=0 },
                new Personel { Ad = "ayhan", Soyad = "c", Yas = 233,Indirim=0,KafileNo=200 },
                new Personel { Ad = "faruk", Soyad = "d", Yas = 44,Indirim=0 },
                new Personel { Ad = "izzet", Soyad = "e", Yas = 233,Indirim=0 }
            };

            for (int i = 0; i < 1000 - PersonelListe.Count; i++)
            {
                PersonelListe.Add(new Personel { Ad = "." });
            }
        }
    }
}