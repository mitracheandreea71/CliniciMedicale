using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Project.Model; 
using Project.Commands;
using Project.Windows;
using System.Windows;
using System.Diagnostics.Contracts;
using System.Windows;
using Project.Model;
using Project.Commands;
using Project.View;


namespace Project.ViewModel
{
    internal class LoginViewModel : BaseViewModel
    {
        private string _email;
        private string _parola;
        private string _role;
        private bool _loginSucceeded;

        private MediciModel _medic;
        public bool LoginSucceeded
        {
            get => _loginSucceeded;
            set
            {
                _loginSucceeded = value;
                OnPropertyChanged(nameof(LoginSucceeded));
            }
        }

        public MediciModel Medic
        {
            get => _medic;
            set
            {
                _medic = value;
                OnPropertyChanged(nameof(Medic));
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
        public string Parola
        {
            get => _parola;
            set
            {
                _parola = value;
                OnPropertyChanged(nameof(Parola));
            }
        }

        public string Role
        {
            get => _role;
            set
            {
                _role = value;
                OnPropertyChanged(nameof(Role));
            }
        }
        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new BaseCommand(ExecuteLogin, CanExecuteLogin);
            
        }

        private void ExecuteLogin(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Parola))
            {
                MessageBox.Show("E-mail si parola sunt obligatorii", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                LoginSucceeded = false;
                return;
            }
            else
            {
                var mediciModel = new MediciModel();
                var medici = mediciModel.GetAllMedici();
                var user = medici.FirstOrDefault(u => u.Email == Email && u.Parola == Parola);
                if (user != null)
                {
                    if (user.Titulatura.IndexOf("Dr.", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Role = "Medic";
                        Medic = user;
                    }
                    else if (user.Titulatura.IndexOf("Asist.", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Role = "Asistent";
                    }
                    else if (user.Titulatura.IndexOf("Admin", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Role = "Administrator";
                    }
                    
                    LoginSucceeded = true;
                   
                }
                else
                {
                    MessageBox.Show("E-mail sau parola incorecta", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

                }
            }
          
        }
      

        private bool CanExecuteLogin(object parameter)
        {
            return true;
        }
    }
}

