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
    /// Interaction logic for PaginaPrincipalaView.xaml
    /// </summary>
    public partial class PaginaPrincipalaView : UserControl
    {
        public PaginaPrincipalaView()
        {
            InitializeComponent();
            this.DataContext = this;

            AnalizeView = new AnalizeView();
            CliniciView = new CliniciView();
            MediciView = new MediciView();
        }

        public AnalizeView AnalizeView { get; set; }
        public CliniciView CliniciView { get; set; }
        public MediciView MediciView { get; set; }
    }
}
