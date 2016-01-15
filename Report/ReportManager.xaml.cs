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
using System.Collections.ObjectModel;
using System.Data;
using System.Threading;
using System.Data.SQLite;

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
        DatabaseManager db_Manager;

        ManualResetEvent FromListLocker;
        ManualResetEvent ToListLocker;

        int currentPeriod;

        public ReportManager()
        {
            InitializeComponent();
        }

        internal ReportManager(MainWindow main, DatabaseManager _dbManager, int periodID)
            : this()
        {
            _parent = main;
            this.Owner = main;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            db_Manager = _dbManager;
            currentPeriod = periodID;
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            toStaffs = new ObservableCollection<Staff>();
            fromStaffs = new ObservableCollection<Staff>();
            staffAddTo = new List<Staff>();
            staffRemoveFrom = new List<Staff>();

            FromListLocker = new ManualResetEvent(true);
            ToListLocker = new ManualResetEvent(true);

            UpdateButtonsStatus(ButtonStatus.DisableAll);

            // Get staff info from database
            BuildStaffFromTable();
        }

        private void BuildStaffFromTable()
        {
            staffTable = db_Manager.GetDataTable(SQLStatement.GetStaffTableQuery());

            if (staffTable != null)
            {
                foreach (DataRow r in staffTable.Rows)
                {
                    Staff staff = new Staff(Convert.ToInt32(r["Staff_ID"]), r["Name"].ToString(), r["EmployeeCode"].ToString(), r["TaxCode"].ToString(), Convert.ToDecimal(r["Rate"]));
                    fromStaffs.Add(staff);
                }
            }
            if (fromStaffs.Count > 0)
            {
                UpdateButtonsStatus(ButtonStatus.CanAdd);
            }
            else
            {
                UpdateButtonsStatus(ButtonStatus.DisableAll);
            }
            

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

        private void BTN_Back_Clicked(object sender, RoutedEventArgs e)
        {
            _parent.Show();
            this.Close();
        }
        

        private void BTN_Generate_Reports_Clicked(object sender, RoutedEventArgs e)
        {
            List<DataTable> tables = new List<DataTable>();
            foreach (Staff s in toStaffs)
            {
                DataTable table = db_Manager.GetDataTable(SQLStatement.GetPaymentDetailsFromStaffIDAndPeriodIDTableQuery(s.Staff_ID, currentPeriod));
                if (table != null && table.Rows.Count > 0)
                {
                    tables.Add(table);
                }
            }

            CustomMessageBox cDialog = new CustomMessageBox(this, "Select Report Type:", "Detail Report", "Rough Report");

            cDialog.ShowDialog();

            int index = cDialog.DialogIndex;

            //ManualResetEvent eventTrigger = new ManualResetEvent(false);
            //cDialog = new CustomMessageBox(this, "Loading...", eventTrigger);

            //Thread reportThread = new Thread(() => ShowWaitDialog(cDialog));

            //reportThread.Start();

            ReportGenerator manager = new ReportGenerator(this.txtReport.Text);

            if (index == 0) // detail report
            {
                manager.GenerateDetailedReport(tables);
            }
            else
            {
                manager.GenerateRoughReport(tables);
            }

            //Thread.Sleep(3000);

            //cDialog.Close();
        }

        //void ShowWaitDialog(CustomMessageBox cDialog)
        //{
        //    this.Dispatcher.BeginInvoke((Action)(() =>
        //    {
        //        cDialog.ShowDialog();

        //        cDialog.WaitUntiReceiveSign();
        //    }));

        //}
    }
}
