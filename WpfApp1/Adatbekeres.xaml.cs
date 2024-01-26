using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
                txtAzonosito.IsEnabled = false;
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
            if (labNev!=null)
            {
                if (!IsValidName(inputName))
                {
                    txtNev.BorderBrush = Brushes.Red;
                    labNev.Visibility = Visibility.Visible;
                }
                else
                {
                    txtNev.BorderBrush = Brushes.Transparent;
                    labNev.Visibility = Visibility.Hidden;
                }
            }
            
        }

        static bool IsValidName(string name)
        {
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
            return !string.IsNullOrEmpty(word) && char.IsUpper(word[0]);
        }

        static bool ContainsDigit(string word)
        {
            return Regex.IsMatch(word, @"\d");
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            string inputEmail = txtEmail.Text;
            if (labEmail!=null)
            {
                if (!IsValidEmail(inputEmail))
                {
                    txtEmail.BorderBrush = Brushes.Red;
                    labEmail.Visibility = Visibility.Visible;
                }
                else
                {
                    txtEmail.BorderBrush = Brushes.Transparent;
                    labEmail.Visibility = Visibility.Hidden;
                }
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
        private  void txtCim_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (labCim!=null)
            {
                if (string.IsNullOrWhiteSpace(txtCim.Text))
                {
                    txtCim.BorderBrush = Brushes.Red;
                    labCim.Visibility = Visibility.Visible;
                }
                else
                {
                    txtCim.BorderBrush = Brushes.Transparent;
                    labCim.Visibility = Visibility.Hidden;
                }
            }

        }
        private void txtAzonosito_TextChanged(object sender, TextChangedEventArgs e)
        {
            string inputNumber = txtAzonosito.Text;

            if (labAzonosito != null)
            {
                if (!IsValidNumber(inputNumber))
                {
                    txtAzonosito.BorderBrush = Brushes.Red;
                    labAzonosito.Visibility = Visibility.Visible;
                }
                else
                {
                    txtAzonosito.BorderBrush = Brushes.Transparent;
                    labAzonosito.Visibility = Visibility.Hidden;
                }
            }
           
        }
        static bool IsValidNumber(string number)
        {
            return !string.IsNullOrEmpty(number) && number.Length == 11 && IsNumeric(number);
        }
        static bool IsNumeric(string value)
        {
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
            if (labMagyar!=null)
            {
                if (IsValidNumber11(inputNumber))
                {
                    txtMagyarpontok.BorderBrush = Brushes.Transparent;
                    labMagyar.Visibility = Visibility.Hidden;
                }
                else
                {

                    txtMagyarpontok.BorderBrush = Brushes.Red;
                    labMagyar.Visibility = Visibility.Visible;
                }
            }
        }

        private void txtMatekpontok_TextChanged(object sender, TextChangedEventArgs e)
        {
            string inputNumber = txtMatekpontok.Text;
            if (labMatek!=null)
            {
                if (IsValidNumber11(inputNumber))
                {
                    txtMatekpontok.BorderBrush = Brushes.Transparent;
                    labMatek.Visibility = Visibility.Hidden;
                }
                else
                {

                    txtMatekpontok.BorderBrush = Brushes.Red;
                    labMatek.Visibility = Visibility.Visible;
                }
            }

        }
        static bool IsValidNumber11(string number)
        {
            if (int.TryParse(number, out int parsedNumber))
            {
                return parsedNumber >= 0 && parsedNumber <= 50;
            }
            return false;
        }     
    }
}
