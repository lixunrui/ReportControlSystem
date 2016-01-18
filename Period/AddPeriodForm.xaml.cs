using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SQLite;
using Microsoft.Windows.Controls;



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
        List<PeriodType> periodTypes;
        Period period;
        PeriodType newPeriodType;
        DatePicker dateStart;
        DatePicker dateEnd;

        internal EventHandler<ObjectPassedEventArgs> NewPeriodAddedEvent;

        public AddPeriodForm()
        {
            InitializeComponent();
        }

        //internal AddPeriodForm(Window form, DatabaseManager _db_manager, Dictionary<int, string> _periodTypes)
        internal AddPeriodForm(Window form, DatabaseManager _db_manager, List<PeriodType> _periodTypes)
            :this()
        {
            _parent = form;
            this.Owner = form;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            period = new Period(DateTime.Now, DateTime.Now);
            
            db_manager = _db_manager;

            InitDatePickers();
            
            dateStart.SelectedDate = DateTime.Now;
            dateEnd.SelectedDate = DateTime.Now;

            InitComBoxPeriodType(_periodTypes);

            periodTypeID = 0;
        }

        void InitComBoxPeriodType(List<PeriodType> _periodTypes)
        {
            periodTypes = _periodTypes;

            comboxPeriodType.ItemsSource = _periodTypes; //_periodTypes.Values;
            //comboxPeriodType.DataContext = _periodTypes;

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
            //int key = periodTypes.FirstOrDefault(x => x.Value.Equals(item.ToString())).Key;

            periodTypeID = (box.SelectedItem as PeriodType).Period_Type_ID;

            newPeriodType = new PeriodType();

            if (periodTypeID > 0)
            {
                UpdateEndDate();
                UpdateDatePickerStatus(false);
                txtSelectedTypeName.Text = "Selected Type Name:";
                txtTypeName.Text = (box.SelectedItem as PeriodType).Period_Type;
            }
            else
            {
                UpdateDatePickerStatus(true);
                txtSelectedTypeName.Text = "Insert Type Name:";
            }
            
        }

        private void BTN_Cancel_Clicked(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void BTN_Create_Clicked(object sender, RoutedEventArgs e)
        {
           // if (periodTypeID == 0)
           // {
                newPeriodType.PeriodDateRange = (period.End_Date.Date - period.Start_Date.Date).Days;

                if (txtTypeName.Text.Length > 0)
                {
                    newPeriodType.Period_Type = txtTypeName.Text;
                }
                else
                {
                    newPeriodType.Period_Type = "New Period Type";
                }

                db_manager.LoadSQLTextFile(SQLStatement.GetInsertPeriodTypeTableQuery(newPeriodType));

                SQLiteDataReader reader = db_manager.ExecuteSQLTextFile(SQLStatement.GetMaxPeriodTypeIDQuery());
                while (reader.Read())
                {
                    period.Period_Type_ID = Convert.ToInt32(reader[0]);
                }
          //  }

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


        private void InitDatePickers()
        {
            dateStart = new DatePicker();
            dateStart.Height = 45;
            dateStart.FontSize = 18;
            dateStart.Focusable = false;
            dateStart.DisplayDate = DateTime.Now;
            dateStart.BorderBrush = Brushes.Black;
            dateStart.BorderThickness = new Thickness(1);

            dateStart.SelectedDateChanged += StartDateChanged;
            dateStart.SelectedDateFormat = DatePickerFormat.Short;

            Grid.SetRow(dateStart, 2);
            Grid.SetColumn(dateStart, 1);

            dateEnd = new DatePicker();
            dateEnd.Height = 45;
            dateEnd.FontSize = 18;
            dateEnd.Focusable = false;
            dateEnd.DisplayDate = DateTime.Now;

            dateEnd.BorderBrush = Brushes.Black;
            dateEnd.BorderThickness = new Thickness(1);

            dateEnd.SelectedDateChanged += EndDateChanged;
            dateStart.SelectedDateFormat = DatePickerFormat.Short;
            Grid.SetRow(dateEnd, 3);
            Grid.SetColumn(dateEnd, 1);

            GridLayout.Children.Add(dateStart);
            GridLayout.Children.Add(dateEnd);
        }

    }
}
