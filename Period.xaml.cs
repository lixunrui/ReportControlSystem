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
using System.Data;

namespace ReportControlSystem
{
    /// <summary>
    /// Interaction logic for Period.xaml
    /// </summary>
    public partial class Period : Window
    {
        MainWindow _parent;
        DatabaseManager db_manager;

        public Period()
        {
            InitializeComponent();
        }

        internal Period(MainWindow main, DatabaseManager db_Manager)
            :this()
        {
            _parent = main;
            db_manager = db_Manager;
        }

        private void Windows_Closed(object sender, EventArgs e)
        {
            _parent.Reload();
            _parent.Show();
            this.Close();
        }

        private void Windows_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPeriodType();
        }

        void LoadPeriodType()
        {
            DataTable periodTypeTable = db_manager.GetDataTable(SQLStatement.GetPeriodTypeTableQuery());

            Dictionary<Int32, String> PeriodTypes = new Dictionary<Int32, String>();

            foreach (DataRow r in periodTypeTable.Rows)
            {
                PeriodTypes.Add(Convert.ToInt32(r[Constants.PeriodTypesElements.Period_Type_ID]),r[Constants.PeriodTypesElements.Period_Type].ToString());   
            }

            comboxPeriodType.ItemsSource = PeriodTypes.Values;
        }

        private void PeriodSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
