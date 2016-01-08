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
using System.Collections.ObjectModel;
using System.Data;
using System.Threading;

namespace ReportControlSystem
{
    /// <summary>
    /// Interaction logic for ReportManager.xaml
    /// </summary>
    public partial class ReportManager : Window
    {
        enum ButtonStatus
        {
            CanAdd,
            CanRemove,
            DisableAdd,
            DisableRemove,
            DisableAll,
        }
        MainWindow _parent;

        // staff list that is going to generate the reports
        ObservableCollection<Staff> toStaffs;
        // staff list from database originally 
        ObservableCollection<Staff> fromStaffs;

        List<Staff> staffAddTo;
        List<Staff> staffRemoveFrom;

        DataTable staffTable;
        DatabaseManager dbManager;

        ManualResetEvent FromListLocker;
        ManualResetEvent ToListLocker;

        public ReportManager()
        {
            InitializeComponent();
        }

        internal ReportManager(MainWindow main, DatabaseManager _dbManager)
            : this()
        {
            _parent = main;
            dbManager = _dbManager;
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            toStaffs = new ObservableCollection<Staff>();
            fromStaffs = new ObservableCollection<Staff>();
            staffAddTo = new List<Staff>();
            staffRemoveFrom = new List<Staff>();

            FromListLocker = new ManualResetEvent(false);
            ToListLocker = new ManualResetEvent(true);



            UpdateButtonsStatus(ButtonStatus.DisableAll);

            // Get staff info from database
            BuildStaffFromTable();
        }

        private void BuildStaffFromTable()
        {
            staffTable = dbManager.GetDataTable(SQLStatement.GetStaffTableQuery());

            if (staffTable != null)
            {
                foreach (DataRow r in staffTable.Rows)
                {
                    Staff staff = new Staff(r["Name"].ToString(), r["Phone"].ToString());
                    fromStaffs.Add(staff);
                }
            }

            UpdateButtonsStatus(ButtonStatus.CanAdd);

            StaffListFrom.DataContext = fromStaffs;
            StaffListTo.DataContext = toStaffs;

        }

        void UpdateButtonsStatus(ButtonStatus status)
        {

            switch (status)
            {
                case ButtonStatus.CanAdd:
                    btnAdd.IsEnabled = true;
                    btnAddAll.IsEnabled = true;
                    break;
                case ButtonStatus.CanRemove:
                    btnRemove.IsEnabled = true;
                    btnRemoveAll.IsEnabled = true;
                    btnGenerate.IsEnabled = true;
                    break;
                case ButtonStatus.DisableAdd:
                    btnAdd.IsEnabled = false;
                    btnAddAll.IsEnabled = false;
                    btnGenerate.IsEnabled = true;
                    break;
                case ButtonStatus.DisableRemove:
                    btnRemove.IsEnabled = false;
                    btnRemoveAll.IsEnabled = false;
                    btnGenerate.IsEnabled = false;
                    break;
                case ButtonStatus.DisableAll:
                    btnAdd.IsEnabled = false;
                    btnAddAll.IsEnabled = false;
                    btnRemove.IsEnabled = false;
                    btnRemoveAll.IsEnabled = false;
                    btnGenerate.IsEnabled = false;
                    break;
            }
        }

        void UpdateButtons()
        {
            if (toStaffs.Count > 0)
            {
                UpdateButtonsStatus(ButtonStatus.CanRemove);
            }
            else
            {
                UpdateButtonsStatus(ButtonStatus.DisableRemove);
            }
            if (fromStaffs.Count > 0)
            {
                UpdateButtonsStatus(ButtonStatus.CanAdd);
            }
            else
            {
                UpdateButtonsStatus(ButtonStatus.DisableAdd);
            }
        }

        private void Windows_Closed(object sender, EventArgs e)
        {
            // make sure the parent window shows on top
            _parent.ShowActivated = true;
            _parent.Show();
            this.Close();
        }

        private void BTN_Add_Clicked(object sender, RoutedEventArgs e)
        {
            FromListLocker.WaitOne();
     
            ToListLocker.Reset();

            foreach (Staff s in staffAddTo)
            {
                toStaffs.Add(s);
                fromStaffs.Remove(s);
            }
            staffAddTo.Clear();

            ToListLocker.Set();

            UpdateButtons();
        }

        private void BTN_Remove_Clicked(object sender, RoutedEventArgs e)
        {
            FromListLocker.WaitOne();
            ToListLocker.Reset();
                foreach (Staff s in staffRemoveFrom)
                {
                    toStaffs.Remove(s);
                    fromStaffs.Add(s);
                }
                staffRemoveFrom.Clear();
                ToListLocker.Set();


            UpdateButtons();
        }

        private void BTN_Add_All_Clicked(object sender, RoutedEventArgs e)
        {
            FromListLocker.WaitOne();
            ToListLocker.Reset();
                foreach (Staff s in fromStaffs)
                {
                    toStaffs.Add(s);
                }
                fromStaffs.Clear();
                ToListLocker.Set();
            
            
            UpdateButtons();
        }

        private void BTN_Remove_All_Clicked(object sender, RoutedEventArgs e)
        {
            FromListLocker.WaitOne();
            ToListLocker.Reset();
                foreach (Staff s in toStaffs)
                {
                    fromStaffs.Add(s);
                }
                toStaffs.Clear();
                ToListLocker.Set();
            UpdateButtons();
        }

        private void StaffListFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // Thread from_SelectionThread = new Thread(new ParameterizedThreadStart(From_Thread));
           // from_SelectionThread.Start();

            Thread from_SelectionThread = new Thread(()=>From_Thread(e, staffAddTo));
            from_SelectionThread.Start();
        }

        private void StaffListTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Thread to_SelectionThread = new Thread(() => From_Thread(e, staffRemoveFrom));
            to_SelectionThread.Start();
        }

        private void From_Thread(SelectionChangedEventArgs e, List<Staff> list)
        {
            Console.WriteLine("from_selection with thread id {0}", Thread.CurrentThread.ManagedThreadId);
            ToListLocker.WaitOne();
            Console.WriteLine("Done waiting...");
            FromListLocker.Reset();
            foreach (Staff s in e.RemovedItems)
            {
                if (list.Contains(s))
                {
                    Console.WriteLine("Remove {0} from From List", s.Name);
                    list.Remove(s);
                }
            }

            foreach (Staff s in e.AddedItems)
            {
                if (!list.Contains(s))
                {
                    Console.WriteLine("Add {0} from From List", s.Name);
                    list.Add(s);
                }
            }
            FromListLocker.Set();
        }


        

        private void BTN_Generate_Reports_Clicked(object sender, RoutedEventArgs e)
        {

        }

    }
}
