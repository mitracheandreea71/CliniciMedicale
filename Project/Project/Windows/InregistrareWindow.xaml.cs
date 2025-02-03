using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Project.ViewModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Project.Commands;
using Project.Model;

namespace Project.Windows
{
    /// <summary>
    /// Interaction logic for InregistrareWindow.xaml
    /// </summary>
    public partial class InregistrareWindow : Window
    {

        public InregistrareWindow()
        {
            DataContext = new InregistrareWindowViewModel();
            InitializeComponent();
            EventAggregator.Instance.Subscribe<ViewChangeMessage<PacientModel>>(message =>
            {
                switch (message.NewView)
                {
                    case "HomePageWindow":
                        new HomePageWindow(message.Model).Show();
                        Window.GetWindow(this).Close();
                        break;
                }
            });
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            var viewModel = DataContext as InregistrareWindowViewModel;

            if (viewModel != null)
            {
                viewModel.Parola = passwordBox.Password;
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
    }
}
