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

namespace Project.View
{
    /// <summary>
    /// Interaction logic for ViewAsigurariAsistView.xaml
    /// </summary>
    public partial class ViewAsigurariAsistView : UserControl
    {
        public ViewAsigurariAsistView(AsistentiModel asistent)
        {
            InitializeComponent();
            DataContext = new ViewAsigurariAsistViewModel(asistent);
        }
    }
}
