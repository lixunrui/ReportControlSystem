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
        Period period;
        PeriodType newPeriodType;

        internal EventHandler<ObjectPassedEventArgs> NewPeriodAddedEvent;

        public AddPeriodForm()
        {
            InitializeComponent();
        }

        internal AddPeriodForm(Window form, DatabaseManager _db_manager, Dictionary<int, string> _periodTypes)
            :this()
        {
            _parent = form;
            this.Owner = form;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            period = new Period(DateTime.Now, DateTime.Now);
            
            db_manager = _db_manager;
            
            dateStart.SelectedDate = DateTime.Now;
            dateEnd.SelectedDate = DateTime.Now;

            InitComBoxPeriodType(_periodTypes);

            periodTypeID = 0;
        }

        void InitComBoxPeriodType(Dictionary<int, string> _periodTypes)
        {
            _periodTypes.Add(0, "Custom");

            periodTypes = _periodTypes;

            comboxPeriodType.ItemsSource = _periodTypes.Values;

            comboxPeriodType.SelectedIndex = 0;

            UpdateDatePickerStatus(false);
        }

        private void UpdateDatePickerStatus(bool show)
        {
            //dateStart.IsEnabled = show;
            dateEnd.IsEnabled = show;
            //txtTypeName.IsReadOnly = !show;
        }

        private void Windows_Closed(object sender, EventArgs e)
        {
            CloseWindow();
        }

        private void StartDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker picker = sender as DatePicker;

            startDT = picker.SelectedDate;

            if (startDT == null)
            {
                startDT = DateTime.Now;
                period.Start_Date = startDT.Value.Date;
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
                period.End_Date = endDT.Value.Date;
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

            period.End_Date = dateEnd.SelectedDate.Value.Date;
        }

        private void ComboxSelectionChangeed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;

            var item = box.SelectedValue;

            // find the key from
            int key = periodTypes.FirstOrDefault(x => x.Value.Equals(item.ToString())).Key;

            periodTypeID = key;

            if (periodTypeID > 0)
            {
                period.Period_Type_ID = periodTypeID;
                UpdateEndDate();
                UpdateDatePickerStatus(false);
                txtSelectedTypeName.Text = "Selected Type Name:";
                txtTypeName.Text = item.ToString();
            }
            else
            {
                newPeriodType = new PeriodType("Custom Period Type");
                UpdateDatePickerStatus(true);
                txtSelectedTypeName.Text = "Insert Type Name:";
                txtTypeName.IsReadOnly = false;
            }
            
        }

        private void BTN_Cancel_Clicked(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void BTN_Create_Clicked(object sender, RoutedEventArgs e)
        {
            if (periodTypeID == 0)
            {

                newPeriodType.PeriodDateRange = (period.End_Date.Date - period.Start_Date.Date).Days;

                if (txtTypeName.Text.Length > 0)
                {
                    newPeriodType.Period_Type = txtTypeName.Text;
                }

                db_manager.LoadSQLTextFile(SQLStatement.GetInsertPeriodTypeTableQuery(newPeriodType));
                SQLiteDataReader reader = db_manager.ExecuteSQLTextFile(SQLStatement.GetMaxPeriodTypeIDQuery());
                while (reader.Read())
                {
                    period.Period_Type_ID = Convert.ToInt32(reader[0]);
                }
            }

            db_manager.LoadSQLTextFile(SQLStatement.GetInsertPeriodTableQuery(period));

            if (NewPeriodAddedEvent != null)
            {
                NewPeriodAddedEvent(this, null);
            }
            CloseWindow();
        }

        void CloseWindow()
        {
            _parent.Show();
            this.Close();
        }

    }
}
