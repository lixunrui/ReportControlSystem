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
using System.Data.SQLite;

namespace ReportControlSystem
{
    /// <summary>
    /// Interaction logic for AddPeriodForm.xaml
    /// </summary>
    public partial class AddPeriodForm : Window
    {
        Window _parent;
        DatabaseManager db_manager;
        Int32 periodTypeID;
        DateTime? startDT;
        DateTime? endDT;
        Dictionary<int, string> periodTypes;

        public AddPeriodForm()
        {
            InitializeComponent();
        }

        internal AddPeriodForm(Window form, DatabaseManager _db_manager, Dictionary<int, string> _periodTypes)
            :this()
        {
            _parent = form;
            periodTypes = _periodTypes;
            db_manager = _db_manager;
            comboxPeriodType.ItemsSource = periodTypes.Values;
            dateStart.SelectedDate = DateTime.Now;
            dateEnd.SelectedDate = DateTime.Now;
            periodTypeID = 0;
        }


        private void Windows_Closed(object sender, EventArgs e)
        {
            _parent.Show();
            this.Close();
        }

        private void StartDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker picker = sender as DatePicker;

            startDT = picker.SelectedDate;

            if (startDT == null)
            {
                startDT = DateTime.Now;
            }
            UpdateEndDate();
        }

        private void EndDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker picker = sender as DatePicker;

            endDT = picker.SelectedDate;

            if (endDT == null)
            {
                endDT = DateTime.Now;
            }
        }

        // update based on the period type
        private void UpdateEndDate()
        {
            // get date range
            SQLiteDataReader reader = db_manager.ExecuteSQLTextFile(SQLStatement.GetDateRangeFromPeriodTypeTableWhereIDQuery(periodTypeID));

            int range = 0;
            while (reader.Read())
            {
                range = Convert.ToInt32(reader[Constants.PeriodElements.PeriodDateRange]);
            }

            reader.Close();

            dateEnd.SelectedDate = startDT.Value.AddDays(range);
        }

        private void ComboxSelectionChangeed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;

            var item = box.SelectedValue;

            // find the key from
            int key = periodTypes.FirstOrDefault(x => x.Value.Equals(item.ToString())).Key;

            if (key > 0)
            {
                periodTypeID = key;
            }
            
        }

        

        private void BTN_Cancel_Clicked(object sender, RoutedEventArgs e)
        {
            _parent.Show();
            this.Close();
        }

        private void BTN_Create_Clicked(object sender, RoutedEventArgs e)
        {
            if (periodTypeID == 0)
            {
                MessageBox.Show("Please select a valid period type.");
                return;
            }
        }

       

    }
}
