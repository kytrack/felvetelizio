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
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Net.Http.Json;

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
            if (MessageBox.Show("Biztos törlöd a kiválasztott elemet/elemeket?", "Igen", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var selectedItems = dgFelvetelizok.SelectedItems.Cast<IFelvetelizo>().ToList();
                foreach (var item in selectedItems)
                {
                    FelvetelizokLista.Remove(item);
                }
            }
            
        }

        private void btnImportal_Click(object sender, RoutedEventArgs e)
        {
            if (FelvetelizokLista.Count()>0)
            {
                MessageBoxResult result = MessageBox.Show("Biztos vagy benne, hogy új adatokat importálsz? Az eddigi adataid elfognak veszni.", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
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
            }
            else
            {
                OpenFileDialog valaszt = new OpenFileDialog();
                if (valaszt.ShowDialog() == true)
                {
                    foreach (String item in File.ReadAllLines(valaszt.FileName).Skip(1))
                    {
                        FelvetelizokLista.Add(new Felvetelizo(item));
                    }
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

        private void btnSzerkeszt_Click(object sender, RoutedEventArgs e)
        {
            if (dgFelvetelizok.SelectedIndex!=-1)
            {
                Felvetelizo ujfelvetelizo = new Felvetelizo();
                ujfelvetelizo.OM_Azonosito = FelvetelizokLista[dgFelvetelizok.SelectedIndex].OM_Azonosito;
                ujfelvetelizo.Neve = FelvetelizokLista[dgFelvetelizok.SelectedIndex].Neve;
                ujfelvetelizo.Email = FelvetelizokLista[dgFelvetelizok.SelectedIndex].Email;
                ujfelvetelizo.ErtesitesiCime = FelvetelizokLista[dgFelvetelizok.SelectedIndex].ErtesitesiCime;
                ujfelvetelizo.SzuletesiDatum = FelvetelizokLista[dgFelvetelizok.SelectedIndex].SzuletesiDatum;
                ujfelvetelizo.Matematika = FelvetelizokLista[dgFelvetelizok.SelectedIndex].Matematika;
                ujfelvetelizo.Magyar = FelvetelizokLista[dgFelvetelizok.SelectedIndex].Magyar;

                Adatbekeres secondWindow = new Adatbekeres(ujfelvetelizo);
                secondWindow.ShowDialog();

                FelvetelizokLista.Insert(dgFelvetelizok.SelectedIndex, ujfelvetelizo);
                FelvetelizokLista.RemoveAt(dgFelvetelizok.SelectedIndex);
            }
            
        }

        private void btnExportaljson_Click(object sender, RoutedEventArgs e)
        {
            var opciok = new JsonSerializerOptions();
            opciok.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            opciok.WriteIndented = true;
            string adatokSorai = JsonSerializer.Serialize(FelvetelizokLista, opciok);
            var lista = new List<string>();
            lista.Add(adatokSorai);
            File.WriteAllLines("json_export.txt", lista);
        }

        List<Felvetelizo>diakok= new List<Felvetelizo>();
        private void btnImportaljson_Click(object sender, RoutedEventArgs e)
        {
            if (FelvetelizokLista.Count() > 0)
            {
                MessageBoxResult result = MessageBox.Show("Biztos vagy benne, hogy új adatokat importálsz? Az eddigi adataid elfognak veszni.", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    OpenFileDialog valaszt = new OpenFileDialog();
                    if (valaszt.ShowDialog() == true)
                    {
                        FelvetelizokLista.Clear();
                        string json = File.ReadAllText(valaszt.FileName);
                        List<Felvetelizo> diakok = JsonSerializer.Deserialize<List<Felvetelizo>>(json);
                        foreach (var item in diakok)
                        {
                            FelvetelizokLista.Add(item);
                        }
                        diakok.Clear();
                    }
                }
            }
            else
            {
                OpenFileDialog valaszt = new OpenFileDialog();
                if (valaszt.ShowDialog() == true)
                {
                        string json = File.ReadAllText(valaszt.FileName);
                    //MessageBox.Show(json);
                    List<Felvetelizo> diakok = JsonSerializer.Deserialize<List<Felvetelizo>>(json);
                    foreach (var item in diakok)
                    {
                        FelvetelizokLista.Add(item);
                    }
                    diakok.Clear();
                }
            }
        }
    }
}
