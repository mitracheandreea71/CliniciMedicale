using Project.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Project.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        private LoginViewModel viewModel;
        public LoginView()
        {
            InitializeComponent();
            viewModel = new LoginViewModel();
            this.DataContext = viewModel;
            if (viewModel is INotifyPropertyChanged notifier)
            {
                notifier.PropertyChanged += ViewModel_PropertyChanged;
            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LoginViewModel.LoginSucceeded))
            {
                if (viewModel.LoginSucceeded)
                {
                   
                    Window loginWindow = Window.GetWindow(this);
                    Window newWindow = null;

                    switch (viewModel.Role)
                    {
                        case "Medic":
                            newWindow = new MedicWindow();
                            break;
                        case "Asistent":
                            newWindow = new AsistentWindow();
                            break;
                        case "Administrator":
                            newWindow = new AdminWindow();
                            break;
                        default:
                            MessageBox.Show("Rol necunoscut. Contactați administratorul.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                    }

                    if (newWindow != null)
                    {
                        newWindow.Show();
                        loginWindow.Close();
                    }
                }
            }
        }
        private void TogglePasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            
            if (PasswordBox.Visibility == Visibility.Visible)
            {
                PasswordBox.Visibility = Visibility.Collapsed;
                PasswordTextBox.Visibility = Visibility.Visible;
                PasswordTextBox.Text = PasswordBox.Password; 
            }
            else
            {
                PasswordBox.Visibility = Visibility.Visible;
                PasswordTextBox.Visibility = Visibility.Collapsed;
            }
        }
        private void MainWindowBttn_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = Window.GetWindow(this);
            MainWindow mainWindow = new MainWindow();
            loginWindow.Close();
            mainWindow.Show();
          
        }
        private void LogareBttn_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (LoginViewModel)DataContext;
            if (viewModel.LoginCommand.CanExecute(null))
            {
                viewModel.LoginCommand.Execute(null);
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            var viewModel = DataContext as LoginViewModel;

            if (viewModel != null)
            {
                viewModel.Parola = passwordBox.Password;  
            }
        }
       
    }
}
