using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Project.Model; 
using Project.Commands; 

namespace Project.ViewModel
{
    internal class LoginViewModel: BaseViewModel
    {
        private string _email;
        private string _parola;
        private string _message;
        private bool _isMessageVisible;

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

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public bool IsMessageVisible
        {
            get => _isMessageVisible;
            set
            {
                _isMessageVisible = value;
                OnPropertyChanged(nameof(IsMessageVisible));

            }
        }

        public ICommand LoginCommand { get; }
        public ICommand HideMessageCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new BaseCommand(ExecuteLogin, CanExecuteLogin);
            HideMessageCommand = new BaseCommand(HideMessage);
            IsMessageVisible = false;
        }

        private void ExecuteLogin(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Parola))
            {
                Message = "E-mail si parola sunt obligatorii";
                IsMessageVisible = true;
                return;
            }

            var mediciModel = new MediciModel();
            try
            {
                var medici = mediciModel.GetAllMedici();
                var user = medici.FirstOrDefault(u => u.Email == Email && u.Parola == Parola);
                if (user != null)
                {
                    Message = "Autentificare reusita";
                    IsMessageVisible = true;
                }
                else
                {
                    Message = "E-mail sau parola incorecta";
                    IsMessageVisible = true;
                }
            }
            catch (Exception ex)
            {
                Message = $"Eroare la autentificare: {ex.Message}";
                IsMessageVisible = true;
            }
        }
        public void HideMessage(object parameter)
        {
            IsMessageVisible = false; 
            }

        private bool CanExecuteLogin(object parameter)
        {
            return true;
        }
    }
}

