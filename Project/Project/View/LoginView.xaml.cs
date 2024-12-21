using Project.ViewModel;
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

namespace Project.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
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

        private void Button_Click_X(object sender, RoutedEventArgs e)
        {
            MessageBorder.Visibility = Visibility.Collapsed;
        }

        private void ContNouBttn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
