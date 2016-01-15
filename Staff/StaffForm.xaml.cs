using System;
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

namespace ReportControlSystem
{
    /// <summary>
    /// Interaction logic for StaffForm.xaml
    /// </summary>
    public partial class StaffForm : Window
    {
        enum FieldStatus
        {
            ReadOnly,
            Editable,
        }
        Window _parnet;
        DatabaseManager db_manager;
        bool formHasChanged;
        Staff currentStaff;
        List<Category> selectedCategories;

        internal EventHandler<ObjectPassedEventArgs> StaffChanged;

        public StaffForm()
        {
            InitializeComponent();
        }

        internal StaffForm(Window form, DatabaseManager manager, Staff staff = null)
            :this()
        {
            _parnet = form;
            this.Owner = form;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            db_manager = manager;
            formHasChanged = false;
            currentStaff = staff;
        }

        private void Windows_Loaded(object sender, RoutedEventArgs e)
        {
            InitEmployee();
            InitializeCategoryList();
            if (currentStaff == null)
            {
                UpdateFieldsStatus(FieldStatus.Editable);
                btnDelete.IsEnabled = false;
            }
            CategoryList.Visibility = Visibility.Collapsed;
        }

        private void Windows_Closed(object sender, EventArgs e)
        {
            _parnet.Show();
            _parnet.ShowActivated = true;
            this.Close();
        }

#region Startup

        private void InitEmployee()
        {
            if (currentStaff != null)
            {
                txtName.Text = currentStaff.Name;
                txtEmployeeCode.Text = currentStaff.EmployeeCode;
                txtTaxCode.Text = currentStaff.TaxCode;
                txtRate.Text = currentStaff.Rate.ToString();
                txtHours.Text = currentStaff.Hours.ToString();
                txtBankCode.Text = currentStaff.BankCode;

                LoadEmployeeCategories();
            }
        }

        private void LoadEmployeeCategories()
        {
            DataTable categoryTableForStaff = db_manager.GetDataTable(SQLStatement.GetStaffCategoryFor(currentStaff.Staff_ID));

            List<Category> cats = new List<Category>();

            foreach (DataRow r in categoryTableForStaff.Rows)
            {
                DataTable categoriesTable = db_manager.GetDataTable(SQLStatement.GetCategoryFor(Convert.ToInt32(r["Category_ID"])));
                foreach (DataRow c in categoriesTable.Rows)
                {
                    Category cat = new Category(Convert.ToInt32(c["Category_ID"]), c["Category_Name"].ToString(), Convert.ToBoolean(c["Category_Type"]), c["Category_Description"].ToString());
                    cats.Add((Category)cat);
                }
            }

            Staff_Category_ListView.ItemsSource = cats;

            // in case we click back after adding categories into a new staff, but without click saving
            if (StaffChanged != null)
            {
                StaffChanged(this, null);
            }
        }

#endregion

        #region Main Panel Events
        // click back, back to the main page
        private void BTN_Back_Clicked(object sender, RoutedEventArgs e)
        {
            _parnet.Show();
            _parnet.ShowActivated = true;
            this.Close();
        }

        // delete the current staff
        private void BTN_Delete_Clicked(object sender, RoutedEventArgs e)
        {
            db_manager.LoadSQLTextFile(SQLStatement.GetDeleteFromStaff(currentStaff.Staff_ID));

            if (StaffChanged != null)
            {
                StaffChanged(this, null);
            }

            _parnet.Show();
            this.Close();
        }

        // update the current staff
        private void BTN_UpdateStaff_Clicked(object sender, RoutedEventArgs e)
        {
            formHasChanged = true;
            UpdateFieldsStatus(FieldStatus.Editable);
        }

        // save all changes
        private void BTN_Save_Clicked(object sender, RoutedEventArgs e)
        {
            UpdateCurrentEmployee();

            InitEmployee();

            UpdateFieldsStatus(FieldStatus.ReadOnly);

            if (StaffChanged != null)
            {
                StaffChanged(this, null);
            }

        }

        // add a new category
        private void BTN_Add_Clicked(object sender, RoutedEventArgs e)
        {
            InitializeCategoryList();
            CategoryGroup.Visibility = Visibility.Collapsed;
            CategoryList.Visibility = Visibility.Visible;
            selectedCategories = new List<Category>();
        }

        // remove an/some category from the list
        private void BTN_Remove_Clicked(object sender, RoutedEventArgs e)
        {
            foreach (Category cat in selectedCategories)
            {
                db_manager.LoadSQLTextFile(SQLStatement.GetDeleteFromStaffCategory(currentStaff.Staff_ID, cat));
            }

            LoadEmployeeCategories();
        }

        #endregion

#region Main Panel Support functions
        private void UpdateCurrentEmployee()
        {
            if (currentStaff == null)
            {
                currentStaff = new Staff(txtName.Text, txtEmployeeCode.Text, txtTaxCode.Text, Convert.ToDecimal(txtRate.Text), Convert.ToDecimal(txtHours.Text), txtBankCode.Text);
                db_manager.LoadSQLTextFile(SQLStatement.GetInsertStaffTableQuery(currentStaff));
                // update current staff ID
                DataTable staffs = db_manager.GetDataTable(SQLStatement.GetMaxStaffIDTableQuery());
                if (staffs != null && staffs.Rows.Count == 1)
                {
                    currentStaff.Staff_ID = Convert.ToInt32(staffs.Rows[0][0]);
                }
                else
                {
                    MessageBox.Show("Error when Adding staff");
                }
            }
            else
            {
                if (formHasChanged)
                {
                    currentStaff.Name = txtName.Text;
                    currentStaff.EmployeeCode = txtEmployeeCode.Text;
                    currentStaff.TaxCode = txtTaxCode.Text;
                    currentStaff.Rate = Convert.ToDecimal(txtRate.Text);
                    currentStaff.Hours = Convert.ToDecimal(txtHours.Text);
                    currentStaff.BankCode = txtBankCode.Text;
                    db_manager.LoadSQLTextFile(SQLStatement.GetUpdateForStaff(currentStaff));
                }
            }

            btnDelete.IsEnabled = true;
        }

        void UpdateFieldsStatus(FieldStatus s)
        {
            bool readOnly = false;
            if (s == FieldStatus.ReadOnly)
            {
                readOnly = true;
                btnSaveChange.BorderBrush = Brushes.Black;
            }
            else
            {
                readOnly = false;
                btnSaveChange.BorderBrush = Brushes.Red;
            }

            btnSaveChange.IsEnabled = !readOnly;

            txtName.IsReadOnly = readOnly;

            txtEmployeeCode.IsReadOnly = readOnly;

            txtTaxCode.IsReadOnly = readOnly;

            txtRate.IsReadOnly = readOnly;

            txtHours.IsReadOnly = readOnly;

            txtBankCode.IsReadOnly = readOnly;

            btnUpdate.IsEnabled = readOnly;

        }

#endregion

        #region Sub Category Panel Events
        private void InitializeCategoryList()
        {
            DataTable categoryTable;

            if (currentStaff == null)
            {
                categoryTable = db_manager.GetDataTable(SQLStatement.GetCategoryTableQuery());
            }
            else
                categoryTable = db_manager.GetDataTable(SQLStatement.GetCategoryNotForStaffTableQuery(currentStaff.Staff_ID));

            if (categoryTable != null)
            {
                List<Category> category = new List<Category>();

                foreach (DataRow r in categoryTable.Rows)
                {
                    Category c = new Category(Convert.ToInt32(r["Category_ID"]), r["Category_Name"].ToString(), Convert.ToBoolean(r["Category_Type"]), r["Category_Description"].ToString());
                    category.Add(c);
                }

                StaffCategoryList.DataContext = category;
                StaffCategoryList.SelectedItems.Clear();
            }
        }

        private void BTN_CancelAdding_Clicked(object sender, RoutedEventArgs e)
        {
            ResetPanel();
        }

        private void BTN_ConfirmAdding_Clicked(object sender, RoutedEventArgs e)
        {
            UpdateCurrentEmployee();

            foreach (Category a in selectedCategories)
            {
                db_manager.LoadSQLTextFile(SQLStatement.GetInsertStaffCategoryTableQuery(currentStaff.Staff_ID, a));
            }

            ResetPanel();

        }

        void ResetPanel()
        {
            LoadEmployeeCategories();
            CategoryList.Visibility = Visibility.Collapsed;
            CategoryGroup.Visibility = Visibility.Visible;
            selectedCategories.Clear();
            StaffCategoryList.SelectedItems.Clear();
        }

        #endregion


#region Common selection event
        private void CategorySelectChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Category cat in e.RemovedItems)
            {
                if (selectedCategories.Contains(cat))
                {
                    Console.WriteLine("Remove {0} from From List", cat.Category_Name);
                    selectedCategories.Remove(cat);
                }
            }

            foreach (Category cat in e.AddedItems)
            {
                if (!selectedCategories.Contains(cat))
                {
                    Console.WriteLine("Add {0} from From List", cat.Category_Name);
                    selectedCategories.Add(cat);
                }
            }
        }
#endregion
         
    }
}
