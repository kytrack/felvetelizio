using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
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
            if (ujfelvetelizo.Neve!=null)
            {
                txtNev.Text = ujfelvetelizo.Neve;
                txtEmail.Text=ujfelvetelizo.Email;
                txtCim.Text = ujfelvetelizo.ErtesitesiCime;
                txtAzonosito.Text = ujfelvetelizo.OM_Azonosito;
                dpSzuletesiido.SelectedDate = ujfelvetelizo.SzuletesiDatum;
                txtMagyarpontok.Text = ujfelvetelizo.Magyar.ToString();
                txtMatekpontok.Text=ujfelvetelizo.Matematika.ToString();
            }
            this.felvetelizo = ujfelvetelizo;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var alaphatter = btnFelvetel.Background;
            if (txtNev.BorderBrush!=Brushes.Red && txtEmail.BorderBrush != Brushes.Red &&
                txtCim.BorderBrush != Brushes.Red && txtAzonosito.BorderBrush != Brushes.Red &&
                txtMagyarpontok.BorderBrush != Brushes.Red && txtMatekpontok.BorderBrush != Brushes.Red)
            {
                felvetelizo.Neve = txtNev.Text;
                felvetelizo.Email = txtEmail.Text;
                felvetelizo.ErtesitesiCime = txtCim.Text;
                felvetelizo.OM_Azonosito = txtAzonosito.Text;
                felvetelizo.SzuletesiDatum = Convert.ToDateTime(dpSzuletesiido.Text);
                felvetelizo.Matematika = int.Parse(txtMatekpontok.Text);
                felvetelizo.Magyar = int.Parse(txtMagyarpontok.Text);

                labMentes.Foreground = Brushes.Green;
                await Task.Delay(1000);
                labMentes.Foreground = Brushes.Transparent;
                this.Close();
            }
            
        }
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtNev_TextChanged(object sender, TextChangedEventArgs e)
        {
            string inputName = txtNev.Text;

            if (!IsValidName(inputName))
            {
                txtNev.BorderBrush = Brushes.Red;
            }
            else
            {
                txtNev.BorderBrush = Brushes.Transparent;
            }
        }

        static bool IsValidName(string name)
        {
            // Legalább két szóból áll, minden szó nagy kezdőbetűvel kezdődik
            // és nincs benne szám.
            string[] words = name.Split(' ');
            if (words.Length < 2)
            {
                return false;
            }

            foreach (string word in words)
            {
                if (!IsFirstLetterUppercase(word) || ContainsDigit(word))
                {
                    return false;
                }
            }

            return true;
        }

        static bool IsFirstLetterUppercase(string word)
        {
            // Ellenőrizzük, hogy az első karakter egy nagy kezdőbetű-e.
            return !string.IsNullOrEmpty(word) && char.IsUpper(word[0]);
        }

        static bool ContainsDigit(string word)
        {
            // Ellenőrizzük, hogy a szó tartalmaz-e számot.
            return Regex.IsMatch(word, @"\d");
        }


        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            string inputEmail = txtEmail.Text;

            if (!IsValidEmail(inputEmail))
            {
                txtEmail.BorderBrush = Brushes.Red;
            }
            else
            {
                txtEmail.BorderBrush = Brushes.Transparent;
            }
        }
        static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private void txtCim_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtCim.Text))
            {
                txtCim.BorderBrush = Brushes.Red;
            }
            else
            {
                txtCim.BorderBrush = Brushes.Transparent;
            }
        }

        private void txtAzonosito_TextChanged(object sender, TextChangedEventArgs e)
        {
            string inputNumber = txtAzonosito.Text;

            if (!IsValidNumber(inputNumber))
            {
                txtAzonosito.BorderBrush = Brushes.Red;
            }
            else
            {
                txtAzonosito.BorderBrush= Brushes.Transparent;
            }
        }
        static bool IsValidNumber(string number)
        {
            // Ellenőrizzük, hogy csak számokat tartalmaz és pontosan 11 karakter hosszú.
            return !string.IsNullOrEmpty(number) && number.Length == 11 && IsNumeric(number);
        }
        static bool IsNumeric(string value)
        {
            // Ellenőrizzük, hogy a string csak számokat tartalmaz.
            foreach (char c in value)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        private void txtMagyarpontok_TextChanged(object sender, TextChangedEventArgs e)
        {
            string inputNumber=txtMagyarpontok.Text;
            if (IsValidNumber11(inputNumber))
            {
                    txtMagyarpontok.BorderBrush = Brushes.Transparent;
            }
            else
            {
                txtMagyarpontok.BorderBrush= Brushes.Red;
            }
        }

        private void txtMatekpontok_TextChanged(object sender, TextChangedEventArgs e)
        {
            string inputNumber = txtMatekpontok.Text;
            if (IsValidNumber11(inputNumber))
            {
                    txtMatekpontok.BorderBrush = Brushes.Transparent;
            }
            else
            {
                txtMatekpontok.BorderBrush = Brushes.Red;
            }
        }
        static bool IsValidNumber11(string number)
        {
            // Ellenőrizzük, hogy csak számokat tartalmaz és az érték 0 és 50 között van.
            if (int.TryParse(number, out int parsedNumber))
            {
                return parsedNumber >= 0 && parsedNumber <= 50;
            }
            return false;
        }

        private void txtNev_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtNev.BorderBrush==Brushes.Red)
            {
                labNev.Foreground = Brushes.Red;
            }
            else
            {
                labNev.Foreground = Brushes.Transparent;
            }
        }

        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmail.BorderBrush == Brushes.Red)
            {
                labEmail.Foreground = Brushes.Red;
            }
            else
            {
                labEmail.Foreground = Brushes.Transparent;
            }
        }

        private void txtCim_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtCim.BorderBrush == Brushes.Red)
            {
                labCim.Foreground = Brushes.Red;
            }
            else
            {
                labCim.Foreground = Brushes.Transparent;
            }
        }

        private void txtAzonosito_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtAzonosito.BorderBrush == Brushes.Red)
            {
                labAzonosito.Foreground = Brushes.Red;
            }
            else
            {
                labAzonosito.Foreground = Brushes.Transparent;
            }
        }

        private void txtMagyarpontok_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtMagyarpontok.BorderBrush == Brushes.Red)
            {
                labMagyar.Foreground = Brushes.Red;
            }
            else
            {
                labMagyar.Foreground = Brushes.Transparent;
            }
        }

        private void txtMatekpontok_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtMatekpontok.BorderBrush == Brushes.Red)
            {
                labMatek.Foreground = Brushes.Red;
            }
            else
            {
                labMatek.Foreground = Brushes.Transparent;
            }
        }
    }
}
