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

namespace ReportControlSystem
{
    /// <summary>
    /// Interaction logic for Period.xaml
    /// </summary>
    public partial class Period : Window
    {
        MainWindow _parent;
        public Period()
        {
            InitializeComponent();
        }

        public Period(MainWindow main)
            :this()
        {
            _parent = main;
        }

        private void Windows_Closed(object sender, EventArgs e)
        {
            _parent.Reload();
            _parent.Show();
            this.Close();
        }
    }
}
