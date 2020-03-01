using DevExpress.Mvvm;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace WpfApp1
{
    public class MainWindowVM : MyBindableBase
    {
        private ObservableCollection<Personel> personelListe;

        public MainWindowVM()
        {
            PersonelListe = new ObservableCollection<Personel>()
            {
                new Personel { AdSoyad="gökmen",Yas=23,Indirim=0,KafileNo=100},
                new Personel { AdSoyad = "musa", Yas = 44,Indirim=0, BaşlangıçSaati=DateTime.Now },
                new Personel { AdSoyad = "ayhan", Yas = 233,Indirim=0,KafileNo=200 },
                new Personel { AdSoyad = "faruk",  Yas = 44,Indirim=0 },
                new Personel { AdSoyad = "izzet", Yas = 233,Indirim=0 }
            };

            for (int i = 0; i < 1000 - PersonelListe.Count; i++)
            {
                PersonelListe.Add(new Personel { AdSoyad = "." });
            }
        }

        public DelegateCommand<CellValueChangedEventArgs> CellValueChangedCommand =>
                                    new DelegateCommand<CellValueChangedEventArgs>(OnKeyCellValueChanged);

        public DelegateCommand<TableView> IcerigiTemizleCommand => new DelegateCommand<TableView>(OnIcerikTemizle);

        public DelegateCommand<KeyEventArgs> KeyDownCommand => new DelegateCommand<KeyEventArgs>(OnKeyDown);

        public DelegateCommand<PastingFromClipboardEventArgs> PastingFromClipboardCommand =>
                        new DelegateCommand<PastingFromClipboardEventArgs>(OnPastingFromClipboard);

        public ObservableCollection<Personel> PersonelListe { get => personelListe; set => SetProperty(ref personelListe, value); }

        public DelegateCommand<TableView> SilCommand => new DelegateCommand<TableView>(OnSil);

        public DelegateCommand<TableView> UsteSatirEkleCommand => new DelegateCommand<TableView>(OnUsteSatirEkle);

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

        private void OnCellValueChanged(string fieldName, object row, object value)
        {
            var seciliSatir = (Personel)row;

            if (fieldName == "KafileNo" && value != null)
            {
                var perRow = (Personel)row;
                var kafileNo = value.ToString().Length == 0 ? -1 : int.Parse(value?.ToString());

                var satir = PersonelListe.Where(c => c.KafileNo == kafileNo).FirstOrDefault();
                if (satir != null)
                {
                    perRow.Yas = satir.Yas;
                }
            }

            if (fieldName == nameof(seciliSatir.BitişSaati))
            {
                var index = personelListe.IndexOf(seciliSatir);

                if (seciliSatir.BaşlangıçSaati.GetValueOrDefault() > seciliSatir.bitişSaati.GetValueOrDefault())
                {
                    seciliSatir.bitişSaati = seciliSatir.BitişSaati?.AddDays(1);
                }

                if (index > 0)
                {
                    var sonrakiSatir = PersonelListe[index + 1];
                    sonrakiSatir.BaşlangıçSaati = seciliSatir.BitişSaati;

                    sonrakiSatir.bitişSaati = seciliSatir.BitişSaati?.Date;
                }
            }
        }

        private void OnIcerikTemizle(TableView w1)
        {
            var cells = w1.GetSelectedCells();

            foreach (var cell in cells)
            {
                w1.Grid.SetCellValue(cell.RowHandle, cell.Column, null);
            }
        }

        private void OnKeyCellValueChanged(CellValueChangedEventArgs e)
        {
            var row = e.Cell.Row;
            var value = e.Cell.Value;
            var fieldName = e.Cell.Property;

            OnCellValueChanged(fieldName, row, value);
        }

        private void OnKeyDown(KeyEventArgs e)
        {
            var w1 = (e.Source as TableView);
            var g1 = w1.Grid;

            if (e.Key == Key.Enter)
            {
                w1.MoveNextRow();
                g1.UnselectAll();
                w1.SelectCell(w1.FocusedRowHandle, (GridColumn)g1.CurrentColumn);
            }

            if (e.Key == Key.Delete)
            {
                var cells = w1.GetSelectedCells();

                foreach (var cell in cells)
                {
                    w1.Grid.SetCellValue(cell.RowHandle, cell.Column, null);
                }
            }
        }

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

        private void OnSil(TableView obj)
        {
            var x = obj.GetSelectedRows().Select(c => c.Row).ToList();
            x.ForEach(item =>
            {
                PersonelListe.Remove(item as Personel);
            });
        }

        private void OnUsteSatirEkle(TableView w1)
        {
            var yeni = new Personel();
            yeni.AdSoyad = "kemalettin";

            var aktifIndex = PersonelListe.IndexOf((Personel)w1.Grid.SelectedItem);

            var ustIndex = aktifIndex;
            if (ustIndex >= 0)
            {
                PersonelListe.Insert(ustIndex, yeni);
            }

            w1.FocusedRowHandle--;

            w1.Grid.UnselectAll();

            w1.SelectCell(w1.FocusedRowHandle, (GridColumn)w1.Grid.CurrentColumn);
        }
    }

    public class Personel : MyBindableBase
    {
        public DateTime? başlangıçSaati;
        public DateTime? bitişSaati;

        public string AdSoyad { get; set; }

        public DateTime? BaşlangıçSaati { get => başlangıçSaati; set => SetProperty(ref başlangıçSaati, value); }
        public DateTime? BitişSaati { get => bitişSaati; set => SetProperty(ref bitişSaati, value); }
        public int? Indirim { get; set; }
        public int? KafileNo { get; set; }

        public int? Yas { get; set; }
    }
}