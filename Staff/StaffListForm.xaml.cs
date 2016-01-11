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
    /// Interaction logic for StaffListForm.xaml
    /// </summary>
    public partial class StaffListForm : Window
    {
        Window _parnet;
        DatabaseManager db_manager;
        Staff currentStaff;

        public StaffListForm()
        {
            InitializeComponent();
        }

        internal StaffListForm(Window form, DatabaseManager manager)
            : this()
        {
            _parnet = form;
            db_manager = manager;
        }

        private void Windows_Closed(object sender, EventArgs e)
        {
            _parnet.Show();
            _parnet.ShowActivated = true;
            this.Close();
        }

        // load staff list here
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitEmployeeTable();
        }

        private void InitEmployeeTable()
        {
            DataTable staffTable = db_manager.GetDataTable(SQLStatement.GetStaffTableQuery());

            List<Staff> staff = new List<Staff>();

            foreach (DataRow r in staffTable.Rows)
            {
                Staff c = new Staff(Convert.ToInt32(r["Staff_ID"]), r["Name"].ToString(), r["EmployeeCode"].ToString(), r["TaxCode"].ToString(), Convert.ToDecimal(r["Rate"]), Convert.ToDecimal(r["Hours"]), r["BankCode"].ToString());
                staff.Add(c);
            }

            EmployeeListView.ItemsSource = staff;

            // default to the first one
            currentStaff = staff[0];
        }

        private void BTN_UpdateStaff_Clicked(object sender, RoutedEventArgs e)
        {
            Button cmd = sender as Button;

            if (cmd.DataContext is Staff)
            {
                Staff s = cmd.DataContext as Staff;

                currentStaff = s;

                StaffForm sForm = new StaffForm(this, db_manager, currentStaff);

                sForm.StaffChanged += new EventHandler<ObjectPassedEventArgs>(StaffChangedNotice);

                sForm.Owner = this;

                sForm.ShowActivated = true;

                sForm.Show();

               // this.Hide();
            }
        }

        private void StaffChangedNotice(object sender, ObjectPassedEventArgs e)
        {
            InitEmployeeTable();
        }

        private void BTN_DeleteStaff_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void BTN_Back_Clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
            _parnet.Show();
        }

        private void BTN_Add_Clicked(object sender, RoutedEventArgs e)
        {
            StaffForm sForm = new StaffForm(this, db_manager);

            sForm.StaffChanged += new EventHandler<ObjectPassedEventArgs>(StaffChangedNotice);

            sForm.Owner = this;

            sForm.ShowActivated = true;

            sForm.Show();
        }

        private void Employee_SelectedChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
