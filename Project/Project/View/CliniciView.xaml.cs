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
using System.Collections.ObjectModel;
using Project.ViewModel;

namespace Project.View
{
    /// <summary>
    /// Interaction logic for CliniciView.xaml
    /// </summary>
public partial class CliniciView : UserControl
    {

        public CliniciView()
        {
            InitializeComponent();
            DataContext = new CliniciViewModel();
        }
    }
}
