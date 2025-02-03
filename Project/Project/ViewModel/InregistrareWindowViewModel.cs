using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Security;
using System.Windows.Input;
using Project.Commands;
using System.Runtime.CompilerServices;
using Project.Model;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Project.ViewModel
{
    internal class InregistrareWindowViewModel : BaseViewModel
    {
        public string _parola;
        public string _nume;
        public string _prenume;
        public string _email;
        public string _judet;
        public string _adresa;
        public string _cnp;
        public bool _acord;

        public ICommand InregistreazaCont { get; }

        public string Parola
        {
            get => _parola;
            set
            {
                _parola = value;
                OnPropertyChanged(nameof(Parola));
            }
        }

        public string Nume
        {
            get => _nume;
            set
            {
                _nume = value;
                OnPropertyChanged(nameof(Nume));
            }
        }
        public string Prenume
        {
            get => _prenume;
            set
            {
                _prenume = value;
                OnPropertyChanged(nameof(Prenume));
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public string Judet
        {
            get => _judet;
            set
            {
                _judet = value;
                OnPropertyChanged(nameof(Judet));
            }
        }
        public string Adresa
        {
            get => _adresa;
            set
            {
                _adresa = value;
                OnPropertyChanged(nameof(Adresa));
            }
        }
        public string CNP
        {
            get => _cnp;
            set
            {
                _cnp = value;
                OnPropertyChanged(nameof(CNP));
            }
        }

        public bool Acord
        {
            get => _acord;
            set
            {
                _acord = value;
                OnPropertyChanged(nameof(Acord));
            }
        }

        private void InregistreazaPacient()
        {
            if (string.IsNullOrEmpty(Nume) || string.IsNullOrEmpty(Prenume) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Parola) || string.IsNullOrEmpty(Judet) || string.IsNullOrEmpty(Adresa)
                                           || string.IsNullOrEmpty(CNP))
            {
                MessageBox.Show("Toate campurile sunt obligatorii!", "Uuups...Ceva nu a mers bine", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                var mail = new MailAddress(Email);
            }
            catch { 
                MessageBox.Show("Adresa de email nu este una valida", "Uuups...Ceva nu a mers bine", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //parola trebuie sa aiba minim 1 litera mare, 1 caracter special, 1 cifra,1 litera mica si o dimensiune minima de 8 caractere
            string regex = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).{8,}$";
            if (!Regex.IsMatch(Parola, regex))
            {
                MessageBox.Show("Parola trebuia sa contina minim: 1 caracter special, 2 cifre, 3 caractere mici si 2 caractere mari", "Uuups...Ceva nu a mers bine", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (CNP.Length != 13)
            {
                MessageBox.Show("CNP-ul nu este unul corespunzator", "Uuups...Ceva nu a mers bine", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(_acord==false)
            {
                MessageBox.Show("Trebuie sa accepti prelucrarea datelor", "Uuups...Ceva nu a mers bine", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            PacientModel pacient = new PacientModel();
            pacient.Email = Email;
            pacient.Nume = Nume;
            pacient.Prenume = Prenume;
            pacient.Judet = Judet;
            pacient.Adresa = Adresa;
            pacient.Parola = Parola;
            pacient.AdaugaPacientInBazaDeDate();
            EventAggregator.Instance.Publish(new ViewChangeMessage<PacientModel>("HomePageWindow",pacient));
        }
        public InregistrareWindowViewModel()
        {
            InregistreazaCont = new BaseCommand(_ => InregistreazaPacient());
        }
    }
}
