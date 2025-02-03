﻿using Project.Model;
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
using Project.Model;

namespace Project.View
{
    /// <summary>
    /// Interaction logic for IstoricMoreView.xaml
    /// </summary>
    public partial class IstoricMoreView : UserControl
    {
        public IstoricMoreView(PacientModel pacient)
        {
            InitializeComponent();
            DataContext = new IstoricMoreViewModel(pacient);
        }

        
    }
}
