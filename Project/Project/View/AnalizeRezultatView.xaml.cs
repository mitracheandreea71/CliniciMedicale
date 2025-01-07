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
using Project.ViewModel;

namespace Project.View
{
    /// <summary>
    /// Interaction logic for AnalizeRezultatView.xaml
    /// </summary>
    public partial class AnalizeRezultatView : UserControl
    {
        public AnalizeRezultatView(int idBuletinAnalize, int idPacient)
        {
            InitializeComponent();
            DataContext = new AnalizeRezultatViewModel(idBuletinAnalize, idPacient);
        }
    }
}
