﻿using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for PaymentForm.xaml
    /// </summary>
    public partial class PaymentForm : Window
    {
        Window _parent;
        DatabaseManager db_manager;
        Dictionary<Int32, String> PeriodTypes;
        Dictionary<Int32, String> Employees;
        Staff currentSelectedStaff;

        public PaymentForm()
        {
            InitializeComponent();
        }

        internal PaymentForm(Window form, DatabaseManager manager)
            : this()
        {
            _parent = form;
            db_manager = manager;
            
            InitEmployeeCombox();
            InitPeriodCombox();
        }

        private void Windows_Loaded(object sender, RoutedEventArgs e)
        {
            //InitEmployeeCombox();
            //InitPeriodCombox();
        }

        private void InitPeriodCombox()
        {
            DataTable periodTypeTable = db_manager.GetDataTable(SQLStatement.GetPeriodTypeTableQuery());

            PeriodTypes = new Dictionary<Int32, String>();

            foreach (DataRow r in periodTypeTable.Rows)
            {
                PeriodTypes.Add(Convert.ToInt32(r[Constants.PeriodTypesElements.Period_Type_ID]), r[Constants.PeriodTypesElements.Period_Type].ToString());
            }

            comPeriodType.ItemsSource = PeriodTypes.Values;
            comPeriodType.SelectedIndex = 0;
        }

        private void InitEmployeeCombox()
        {
            DataTable staffTable = db_manager.GetDataTable(SQLStatement.GetStaffTableQuery());

            Employees = new Dictionary<Int32, String>();

            foreach (DataRow r in staffTable.Rows)
            {
                Staff staff = new Staff(Convert.ToInt32(r["Staff_ID"]), r["Name"].ToString(), r["EmployeeCode"].ToString(), r["TaxCode"].ToString(), Convert.ToDecimal(r["Rate"]));

                Employees.Add(Convert.ToInt32(r["Staff_ID"]), r["Name"].ToString());
            }
            comEmployee.ItemsSource = Employees.Values;
            comEmployee.SelectedIndex = 0;

        }

        void ResetEmployeeTotalHours(int employeeID)
        {
            if (currentSelectedStaff != null)
            {
                db_manager.LoadSQLTextFile(SQLStatement.GetUpdateHoursForStaff(currentSelectedStaff.Staff_ID));
            }
        }

        private void Windows_Closed(object sender, EventArgs e)
        {
            ExitForm();
        }


        private void ComFrequencySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;

            var item = cmb.SelectedValue;

            int key = PeriodTypes.FirstOrDefault(x => x.Value.Equals(item.ToString())).Key;

            // find the period
            if (key > 0)
            {
                SQLiteDataReader reader = db_manager.ExecuteSQLTextFile(SQLStatement.GetPeriodFromTypeID(key));
                while (reader.Read())
                {
                    LoadDate(Convert.ToDateTime(reader[Constants.PeriodElements.Start_Date]), Convert.ToDateTime(reader[Constants.PeriodElements.End_Date]));
                }
            }
            
        }

        private void LoadDate(DateTime startDT, DateTime endDT)
        {
            txtStartDate.Text = startDT.ToString("yyyy-MM-dd");
            txtEndDate.Text = endDT.ToString("yyyy-MM-dd");
        }

        private void ComEmployeeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;

            var item = cmb.SelectedValue;

            int key = Employees.FirstOrDefault(x => x.Value.Equals(item.ToString())).Key;

            if (key > 0)
            {
                List<Int32> categoryID = new List<Int32>();
                SQLiteDataReader reader = db_manager.ExecuteSQLTextFile(SQLStatement.GetStaffFromIDQuery(key));
                while (reader.Read())
                {
                    currentSelectedStaff = new Staff(
                        Convert.ToInt32(reader[Constants.EmployeeElements.Employee_ID]), 
                        reader[Constants.EmployeeElements.Employee_Name].ToString(),
                        reader[Constants.EmployeeElements.Employee_Code].ToString(),
                        reader[Constants.EmployeeElements.Employee_TaxCode].ToString(),
                        Convert.ToDecimal(reader[Constants.EmployeeElements.Employee_Rate]),
                        Convert.ToDecimal(reader[Constants.EmployeeElements.Employee_Hours]),
                        reader[Constants.EmployeeElements.Employee_BankCode].ToString());
                }
                reader.Close();

                UpdateEmployeeInfo();


                reader = db_manager.ExecuteSQLTextFile(SQLStatement.GetStaffCategoryFor(key));
                while (reader.Read())
                {
                    categoryID.Add(Convert.ToInt32(reader[Constants.CategoryElements.Category_ID]));
                }
                LoadCategories(categoryID);
            }      
        }

        private void UpdateEmployeeInfo()
        {
            txtEmployeeID.Text = currentSelectedStaff.EmployeeCode;
            txtBank.Text = currentSelectedStaff.BankCode;
            txtRate.Text = currentSelectedStaff.Rate.ToString();
            txtTotalHours.Text = currentSelectedStaff.Hours.ToString();
        }

        private void LoadCategories(List<Int32> categoryID)
        {
            PaymentPanel.Children.Clear();

            foreach (int id in categoryID)
            {
                SQLiteDataReader reader = db_manager.ExecuteSQLTextFile(SQLStatement.GetCategoryFor(id));
                while (reader.Read())
                {
                    Category c = new Category(Convert.ToInt32(reader[Constants.CategoryElements.Category_ID]), reader[Constants.CategoryElements.Category_Name].ToString(), Convert.ToBoolean(reader[Constants.CategoryElements.Category_Type]), reader[Constants.CategoryElements.Category_Description].ToString());

                   TextBlock txtCName = new TextBlock();
                   txtCName.Text = c.Category_Name;
                   txtCName.FontSize = 18;
                   txtCName.Margin = new Thickness(3, 0, 0, 0);

                   TextBox txtContent = new TextBox();
                   txtContent.Margin = new Thickness(9,0,0,0);
                   Border border = new Border();
                   border.BorderBrush = Brushes.Black;
                   border.BorderThickness = new Thickness(0, 0, 0, 1);
                   border.Margin = new Thickness(9,0,0,9);

                   PaymentPanel.Children.Add(txtCName);
                   PaymentPanel.Children.Add(txtContent);
                   PaymentPanel.Children.Add(border);
                }
            }
        }

        private void BTN_Cancel_Clicked(object sender, RoutedEventArgs e)
        {
            ExitForm();
        }

        private void ExitForm()
        {
            _parent.Activate();
            _parent.Show();
            this.Close();
        }

        
    }
}
