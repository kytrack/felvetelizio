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
using System.IO;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<IFelvetelizo> FelvetelizokLista = new ObservableCollection<IFelvetelizo>();
        public MainWindow()
        {
            InitializeComponent();
            dgFelvetelizok.ItemsSource = FelvetelizokLista;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnFelvesz_Click(object sender, RoutedEventArgs e)
        {
            Felvetelizo ujfelvetelizo = new Felvetelizo();
            Adatbekeres secondWindow = new Adatbekeres(ujfelvetelizo);     
            secondWindow.ShowDialog();
            if (ujfelvetelizo.Neve != null)
            {
                FelvetelizokLista.Add(ujfelvetelizo);
            }
                
        }

        private void btnTorol_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Biztos törlöd a kiválasztott elemet?", "Igen", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                int kivalasztott = dgFelvetelizok.SelectedIndex;
                FelvetelizokLista.RemoveAt(kivalasztott);
            }
            
        }

        private void btnImportal_Click(object sender, RoutedEventArgs e)
        {          
            OpenFileDialog valaszt = new OpenFileDialog();
            if (valaszt.ShowDialog() == true)
            {
                FelvetelizokLista.Clear();
                foreach (String item in File.ReadAllLines(valaszt.FileName).Skip(1))
                {
                    FelvetelizokLista.Add(new Felvetelizo(item));
                }
            }   
        }

        private async void btnExportal_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter sw = new StreamWriter("felvetelizok_exportalas.csv");
            foreach (Felvetelizo item in dgFelvetelizok.Items)
            {
                sw.WriteLine(item.CSVSortAdVissza());
            }
            sw.Close();
            labMentes.Foreground = Brushes.Green;
            await Task.Delay(1000);
            labMentes.Foreground = Brushes.Transparent;
        }

        private void btnTalca_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized; 
        }

        private void btnKilep_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
