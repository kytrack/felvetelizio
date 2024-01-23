using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Adatbekeres.xaml
    /// </summary>
    public partial class Adatbekeres : Window
    {
        Felvetelizo felvetelizo;
        public Adatbekeres()
        {
            InitializeComponent();
        }
        public Adatbekeres (Felvetelizo ujfelvetelizo) :this()
        {
            this.felvetelizo = ujfelvetelizo;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {      
            felvetelizo.Neve = txtNev.Text;
            felvetelizo.Email = txtEmail.Text;
            felvetelizo.ErtesitesiCime = txtCim.Text;       
            felvetelizo.OM_Azonosito = txtAzonosito.Text;
            felvetelizo.SzuletesiDatum = Convert.ToDateTime(dpSzuletesiido.Text);
            felvetelizo.Matematika = int.Parse(txtMatekpontok.Text);
            felvetelizo.Magyar = int.Parse(txtMagyarpontok.Text);
            //MessageBox.Show("Sikeres Felvétel");
            this.Close();
        }
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
