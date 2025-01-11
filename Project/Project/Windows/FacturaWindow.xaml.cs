using Project.Model;
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
using System.Windows.Shapes;

namespace Project.Windows
{
    /// <summary>
    /// Interaction logic for FacturaWindow.xaml
    /// </summary>
    public partial class FacturaWindow : Window
    {
        public FacturaWindow(object obj)
        {
            InitializeComponent();
            if(obj is ConsultatieModel consultatie)
                DataContext = new FacturaWindowViewModel<ConsultatieModel>(consultatie);
            if(obj is BuletinAnalizeModel buletin)
                DataContext = new FacturaWindowViewModel<BuletinAnalizeModel>(buletin);
        }
    }
}
