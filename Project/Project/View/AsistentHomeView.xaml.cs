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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project.View
{
    /// <summary>
    /// Interaction logic for AsistentHomeView.xaml
    /// </summary>
    public partial class AsistentHomeView : UserControl
    {
        public AsistentHomeView(AsistentiModel asistent)
        {
            InitializeComponent();
            DataContext = new AsistentHomeViewModel(asistent);
        }
    }
}
