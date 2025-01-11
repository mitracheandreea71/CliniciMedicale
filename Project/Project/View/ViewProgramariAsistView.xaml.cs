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
    /// Interaction logic for ViewProgramariAsistView.xaml
    /// </summary>
    public partial class ViewProgramariAsistView : UserControl
    {
        public ViewProgramariAsistView(AsistentiModel asistent)
        {
            InitializeComponent();
            DataContext = new ViewProgramariAsistViewModel(asistent);
        }
    }
}
