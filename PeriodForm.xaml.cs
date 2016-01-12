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
    public partial class PeriodForm : Window
    {
        MainWindow _parent;
        DatabaseManager db_manager;
        Dictionary<Int32, String> PeriodTypes;
        DateTime selectedDate;

        public PeriodForm()
        {
            InitializeComponent();
        }

        internal PeriodForm(MainWindow main, DatabaseManager db_Manager)
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
            LoadPeriodTable();
            //ShowCalendarPanel(false);
        }

        void LoadPeriodType()
        {
            DataTable periodTypeTable = db_manager.GetDataTable(SQLStatement.GetPeriodTypeTableQuery());

            PeriodTypes = new Dictionary<Int32, String>();

            PeriodTypes.Add(-1, "All Types");

            foreach (DataRow r in periodTypeTable.Rows)
            {
                PeriodTypes.Add(Convert.ToInt32(r[Constants.PeriodTypesElements.Period_Type_ID]),r[Constants.PeriodTypesElements.Period_Type].ToString());   
            }

            comboxPeriodType.ItemsSource = PeriodTypes.Values;
            comboxPeriodType.SelectedIndex = 0;
        }


        private void LoadPeriodTable()
        {
            DataTable periodTable;

            if (comboxPeriodType.SelectedIndex == 0)
            {
                periodTable = db_manager.GetDataTable(SQLStatement.GetPeriodTableQuery());
            }
            else
            {
                int key = PeriodTypes.FirstOrDefault(x => x.Value.Equals(comboxPeriodType.SelectedValue.ToString())).Key;

                periodTable = db_manager.GetDataTable(SQLStatement.GetPeriodFromTypeID(key));
            }

            List<Period> periods = new List<Period>();
            foreach (DataRow r in periodTable.Rows)
            {
                Period period = new Period(Convert.ToInt32(r[Constants.PeriodElements.Period_ID]), Convert.ToDateTime(r[Constants.PeriodElements.Start_Date]), Convert.ToDateTime(r[Constants.PeriodElements.End_Date]), Convert.ToInt32(r[Constants.PeriodElements.Period_Type_ID]), r[Constants.PeriodElements.Period_Type].ToString(), Convert.ToBoolean(r[Constants.PeriodElements.Period_Status]), Convert.ToInt32(r[Constants.PeriodElements.PeriodDateRange]));

                periods.Add(period);
            }

            PeriodList.ItemsSource = periods;
        }

        private void PeriodSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBoxSelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadPeriodTable();

        }

        private void BTN_ClosePeriod_Clicked(object sender, RoutedEventArgs e)
        {
            // close the current period
            Button cmd = sender as Button;

            if (cmd.DataContext is Period)
            {
                Period period = (Period)cmd.DataContext;

                // close the period
                db_manager.LoadSQLTextFile(SQLStatement.GetUpdatePeriodCloseFromID(period.Period_ID));

                // show the calendar
               //// ShowCalendarPanel(true);

               // // create new period from the next Monday of the closed period
               // DateTime newStartDate = period.End_Date;

               // do 
               // {
               //     newStartDate.AddDays(1);
               // } while (newStartDate.DayOfWeek != DayOfWeek.Monday);

               // DateTime newEndData = newStartDate;

                LoadPeriodTable();
            }
            
            // show calendar panel to start the next one
        }

        private void BTN_Add_New_Period(object sender, RoutedEventArgs e)
        {
            AddPeriodForm newForm = new AddPeriodForm(this, db_manager, PeriodTypes);

            newForm.Show();

            this.Hide();
        }

        //void ShowCalendarPanel(bool showPanel)
        //{
        //    Visibility visiblity = Visibility.Visible;

        //    if (showPanel)
        //    {
        //        visiblity = Visibility.Visible;
        //        PeriodList.IsEnabled = false;
        //    }
        //    else
        //    {
        //        visiblity = Visibility.Collapsed;
        //        PeriodList.IsEnabled = true;
        //    }

        //    Calendar_Panel.Visibility = visiblity;
        //}

        //private void CalendarSelectionDatesChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}

        //private void BTN_Create_Period_Clicked(object sender, RoutedEventArgs e)
        //{

        //}
    }
}
