using DevExpress.Mvvm;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace WpfApp1
{
    public class Personel
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public int Yas { get; set; }

        public int Indirim { get; set; }
    }

    public class MainWindowVM
    {
        public DelegateCommand<PastingFromClipboardEventArgs> PastingFromClipboardCommand =>
          new DelegateCommand<PastingFromClipboardEventArgs>(OnPastingFromClipboard);

        public DelegateCommand<ClipboardRowPastingEventArgs> RowPastingCommand =>
            new DelegateCommand<ClipboardRowPastingEventArgs>(OnClipboardRowPasting);

        public DelegateCommand<ClipboardRowCellValuePastingEventArgs> RowCellValuePastingCommand =>
            new DelegateCommand<ClipboardRowCellValuePastingEventArgs>(OnRowCellValuePasting);

        public DelegateCommand<KeyEventArgs> KeyDownCommand => new DelegateCommand<KeyEventArgs>(OnKeyDown);

        private void OnPastingFromClipboard(PastingFromClipboardEventArgs obj)
        {
        }

        private void OnClipboardRowPasting(ClipboardRowPastingEventArgs obj)
        {
        }

        private void OnRowCellValuePasting(ClipboardRowCellValuePastingEventArgs e)
        {
            if (e.Column.FieldName == "Ad")
            {
                var satir = PersonelListe.Where(c => c.Ad == e.Value.ToString());
            }
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
                new Personel { Ad="gökmen",Soyad="a",Yas=23,Indirim=0},
                new Personel { Ad = "musa", Soyad = "b", Yas = 44,Indirim=0 },
                new Personel { Ad = "ayhan", Soyad = "c", Yas = 233,Indirim=0 },
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