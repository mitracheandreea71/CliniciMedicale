﻿using System;
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
using Project.ViewModel;

namespace Project.Windows
{
    /// <summary>
    /// Interaction logic for RezultatAnalize.xaml
    /// </summary>
    public partial class RezultatAnalizeWindow : Window
    {
        public RezultatAnalizeWindow(int idBuletin, int idPacient)
        {
            InitializeComponent();
            DataContext = new RezultateWindowViewModel(idBuletin, idPacient);
        }
    }
}
