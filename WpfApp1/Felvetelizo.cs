using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Felvetelizo : IFelvetelizo
    {
        String om_azonosito;
        String neve;
        String ertesitesiCime;
        String email;
        DateTime szuletesiDatum;
        int matematika;
        int magyar;
        
        public Felvetelizo() { }
        public Felvetelizo(string csvString)
        {
            try
            {
                string[] mezok = csvString.Split(';');
                om_azonosito = mezok[0];
                neve = mezok[1];
                ertesitesiCime = mezok[2];
                email = mezok[3];
                szuletesiDatum = DateTime.Parse(mezok[4]);
                if (mezok[5] == "NULL")
                {
                    matematika = -1;
                } else {
                    matematika = int.Parse(mezok[5]);
                }
                if (mezok[6] == "NULL")
                {
                    magyar = -1;
                }
                else
                {
                    magyar = int.Parse(mezok[6]);
                }
            }
            catch (FormatException)
            {
                throw new FormatException("Hibás formátum");
            }
        }
        public Felvetelizo(string OM_Azonosito, string neve, string ertesitesiCime, string email, DateTime szuletesiDatum, int matematika, int magyar)
        {
            this.om_azonosito = OM_Azonosito;
            this.neve = neve;
            this.ertesitesiCime = ertesitesiCime;
            this.email = email;
            this.szuletesiDatum = szuletesiDatum;
            this.matematika = matematika;
            this.magyar = magyar;
        }

        public string OM_Azonosito { get => om_azonosito; set => om_azonosito = value; }
        public string Neve { get => neve; set => neve = value; }
        public string ErtesitesiCime { get => ertesitesiCime; set => ertesitesiCime = value; }
        public string Email { get => email; set => email = value; }
        public DateTime SzuletesiDatum { get => szuletesiDatum; set => szuletesiDatum = value; }
        public int Matematika { get => matematika; set => matematika = value; }
        public int Magyar { get => magyar; set => magyar = value; }

        public string CSVSortAdVissza()
        {
            return $"{OM_Azonosito};{Neve};{ErtesitesiCime};{Email};{SzuletesiDatum};{Matematika};{Magyar}";
        }

        public void ModositCSVSorral(String csvString)
        {
            string[] mezok = csvString.Split(';');
            OM_Azonosito = mezok[0];
            neve = mezok[1];
            ertesitesiCime = mezok[2];
            email = mezok[3];
            szuletesiDatum = DateTime.Parse(mezok[4]);
            if (mezok[5] == "NULL")
            {
                matematika = -1;
            }
            else
            {
                matematika = int.Parse(mezok[5]);
            }
            if (mezok[6] == "NULL")
            {
                magyar = -1;
            }
            else
            {
                magyar = int.Parse(mezok[6]);
            }

        }
    }
}
