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
    public static class SendKeys
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

            t1.KeyDown += T1_KeyDown;

            t1.PreviewKeyDown += T1_PreviewKeyDown;

            var personelListe = new List<Personel>()
            {
                new Personel { Ad="gökmen",Soyad="yılmaz",Yas=23},
                new Personel { Ad = "musa", Soyad = "aydın", Yas = 44 },
                new Personel { Ad = "ayhan", Soyad = "uzun", Yas = 233 }
            };

            g1.ItemsSource = personelListe;

        }

        private void T1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendKeys.Send(Key.Tab);
            }
        }

        private void T1_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

     


      
    }
}
