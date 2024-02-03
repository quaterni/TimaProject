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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimaProject.Desctop.Views
{
    /// <summary>
    /// Interaction logic for TimeFormView.xaml
    /// </summary>
    public partial class TimeFormView : UserControl
    {
        public TimeFormView()
        {
            InitializeComponent();
        }

        private void DatePicker_LostFocus(object sender, RoutedEventArgs e)
        {
            clickable.IsContentVisable = false;
        }
    }
}