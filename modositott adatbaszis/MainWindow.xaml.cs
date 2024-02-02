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
using MySql.Data.MySqlClient;

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
        List<Felvetelizo> diakok = new List<Felvetelizo>();
        private void btnImportal_Click(object sender, RoutedEventArgs e)
        {
            if (FelvetelizokLista.Count()>0)
            {
                MessageBoxResult result = MessageBox.Show("Biztos vagy benne, hogy új adatokat importálsz? Az eddigi adataid elfognak veszni.", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    OpenFileDialog valaszt = new OpenFileDialog();
                    valaszt.Filter = "JSON files (*.json)|*.json|CSV files (*.csv)|*.csv";
                    if (valaszt.ShowDialog() == true)
                    {
                        FelvetelizokLista.Clear();
                        string kivalasztottFajl = valaszt.FileName;

                        if (kivalasztottFajl.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                        {
                            string json = File.ReadAllText(valaszt.FileName);
                            List<Felvetelizo> diakok = JsonSerializer.Deserialize<List<Felvetelizo>>(json);
                            foreach (var item in diakok)
                            {
                                FelvetelizokLista.Add(item);
                            }
                            diakok.Clear();
                        }
                        else
                        {
                            foreach (String item in File.ReadAllLines(valaszt.FileName).Skip(1))
                            {
                                FelvetelizokLista.Add(new Felvetelizo(item));
                            }
                        }


                    }
                }
            }
            else
            {
                OpenFileDialog valaszt = new OpenFileDialog();
                valaszt.Filter = "JSON files (*.json)|*.json|CSV files (*.csv)|*.csv";
                if (valaszt.ShowDialog() == true)
                {
                    string kivalasztottFajl = valaszt.FileName;

                    if (kivalasztottFajl.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        string json = File.ReadAllText(valaszt.FileName);
                        List<Felvetelizo> diakok = JsonSerializer.Deserialize<List<Felvetelizo>>(json);
                        foreach (var item in diakok)
                        {
                            FelvetelizokLista.Add(item);
                        }
                        diakok.Clear();
                    }
                    else
                    {
                        foreach (String item in File.ReadAllLines(valaszt.FileName).Skip(1))
                        {
                            FelvetelizokLista.Add(new Felvetelizo(item));
                        }
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

        private async void btnExportaljson_Click(object sender, RoutedEventArgs e)
        {
            var opciok = new JsonSerializerOptions();
            opciok.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            opciok.WriteIndented = true;
            string adatokSorai = JsonSerializer.Serialize(FelvetelizokLista, opciok);
            var lista = new List<string>();
            lista.Add(adatokSorai);
            File.WriteAllLines("json_export.json", lista);

            labMentes2.Foreground = Brushes.Green;
            await Task.Delay(1000);
            labMentes2.Foreground = Brushes.Transparent;
        }

        private async void btnExportalAdatbazis_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=felvetelizo;User ID=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Töröld az összes rekordot a Felvetelizok táblából
                    string deleteQuery = "DELETE FROM Felvetelizok";
                    using (MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, connection))
                    {
                        deleteCmd.ExecuteNonQuery();
                    }

                    Console.WriteLine("Az összes rekord törölve a Felvetelizok táblából.");

                    // Most már hozzáadhatod az új rekordokat
                    foreach (IFelvetelizo felvetelizo in FelvetelizokLista)
                    {
                        string insertQuery = "INSERT INTO Felvetelizok (OM_Azonosito, Neve, ErtesitesiCime, Email, SzuletesiDatum, Matematika, Magyar) " +
                                             $"VALUES ('{felvetelizo.OM_Azonosito}', '{felvetelizo.Neve}', '{felvetelizo.ErtesitesiCime}', '{felvetelizo.Email}', " +
                                             $"'{felvetelizo.SzuletesiDatum.ToString("yyyy-MM-dd")}', {felvetelizo.Matematika}, {felvetelizo.Magyar})";

                        using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                        {
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                    labMentes3.Foreground = Brushes.Green;
                    await Task.Delay(1000);
                    labMentes3.Foreground = Brushes.Transparent;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba a kapcsolódás vagy adatok mentése során: " + ex.Message);
                }
            }


        }
    }
}
