using Project.Model;
using Project.View;
using Project.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace Project.Windows
{
    /// <summary>
    /// Interaction logic for HomePageWindow.xaml
    /// </summary>
    public partial class HomePageWindow : Window
    {
        public HomePageWindow(PacientModel pacient)
        {
            DataContext = new HomePageWindowViewModel(pacient);
            InitializeComponent();
        }

        private void ProgramareBttn_Click(object sender, RoutedEventArgs e)
        {
            SolicitaProgramareClinicaWindow wd = new SolicitaProgramareClinicaWindow();
            wd.Show();
        }
    }
}
