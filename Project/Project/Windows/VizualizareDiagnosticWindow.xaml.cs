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
    /// Interaction logic for VizualizareDiagnosticWindow.xaml
    /// </summary>
    public partial class VizualizareDiagnosticWindow : Window
    {
        public VizualizareDiagnosticWindow(ConsultatieModel consultatie, MediciModel medic)
        {
            InitializeComponent();
            var viewModel = new VizualizareDiagnosticViewModel(consultatie, medic);

            var view = new Project.View.VizualizareDiagnosticView();

            view.DataContext = viewModel;

            RezultateConsultatie.Content = view;
        }
    }
}
