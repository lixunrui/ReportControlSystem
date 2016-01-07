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

namespace ReportControlSystem
{
    /// <summary>
    /// Interaction logic for ReportManager.xaml
    /// </summary>
    public partial class ReportManager : Window
    {
        MainWindow _parent;

        public ReportManager()
        {
            InitializeComponent();
        }

        public ReportManager(MainWindow main)
            : this()
        {
            _parent = main;
        }

        private void Windows_Closed(object sender, EventArgs e)
        {
            _parent.Show();
            this.Close();
        }

        private void BTN_Add_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void BTN_Remove_Clicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
