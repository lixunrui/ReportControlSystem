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
            db_manager = manager;
            formHasChanged = false;
            currentStaff = staff;
        }

        private void Windows_Closed(object sender, EventArgs e)
        {
            _parnet.Show();
            _parnet.ShowActivated = true;
            this.Close();
        }

        private void BTN_Back_Clicked(object sender, RoutedEventArgs e)
        {
            _parnet.Show();
            _parnet.ShowActivated = true;
            this.Close();
        }

        private void BTN_Delete_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void BTN_Save_Clicked(object sender, RoutedEventArgs e)
        {
            if (currentStaff == null)
            {
                currentStaff = new Staff(txtName.Text, txtEmployeeCode.Text, txtTaxCode.Text, Convert.ToDecimal(txtRate.Text), Convert.ToDecimal(txtHours.Text), txtBankCode.Text);
                db_manager.ExecuteSQLTextFile(SQLStatement.GetInsertStaffTableQuery(currentStaff));
            }
            else
            {
                currentStaff.Name = txtName.Text;
                currentStaff.EmployeeCode = txtEmployeeCode.Text;
                currentStaff.TaxCode = txtTaxCode.Text;
                currentStaff.Rate = Convert.ToDecimal(txtRate.Text);
                currentStaff.Hours = Convert.ToDecimal(txtHours.Text);
                currentStaff.BankCode = txtBankCode.Text;
                db_manager.ExecuteSQLTextFile(SQLStatement.GetUpdateForStaff(currentStaff));
            }

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
            CategoryGroup.Visibility = Visibility.Collapsed;
            CategoryList.Visibility = Visibility.Visible;
            selectedCategories = new List<Category>();

        }

        private void BTN_Remove_Clicked(object sender, RoutedEventArgs e)
        {
            foreach (Category cat in selectedCategories)
            {
                db_manager.ExecuteSQLTextFile(SQLStatement.GetDeleteFromStaffCategory(currentStaff.Staff_ID, cat));
            }

            LoadEmployeeCategories();
        }

        private void BTN_UpdateStaff_Clicked(object sender, RoutedEventArgs e)
        {
            formHasChanged = true;
            UpdateFieldsStatus(FieldStatus.Editable);
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

            btnUpdate.IsEnabled = readOnly;

            
        }

        private void Windows_Loaded(object sender, RoutedEventArgs e)
        {
            InitEmployee();
            InitializeCategoryList();
            if (currentStaff == null)
            {
                UpdateFieldsStatus(FieldStatus.Editable);
            }
            CategoryList.Visibility = Visibility.Collapsed;
        }

        private void InitEmployee()
        {
            if (currentStaff!=null)
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

        private void InitializeCategoryList()
        {
            DataTable categoryTable = db_manager.GetDataTable(SQLStatement.GetCategoryTableQuery());

            if (categoryTable != null)
            {
                List<Category> category = new List<Category>();

                foreach (DataRow r in categoryTable.Rows)
                {
                    Category c = new Category(Convert.ToInt32(r["Category_ID"]), r["Category_Name"].ToString(), Convert.ToBoolean(r["Category_Type"]), r["Category_Description"].ToString());
                    category.Add(c);
                }

                CategoryList.DataContext = category;
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
        }

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

        private void BTN_CancelAdding_Clicked(object sender, RoutedEventArgs e)
        {
            ResetPanel();
        }

        private void BTN_ConfirmAdding_Clicked(object sender, RoutedEventArgs e)
        {
            foreach (Category a in selectedCategories)
            {
                db_manager.ExecuteSQLTextFile(SQLStatement.GetInsertStaffCategoryTableQuery(currentStaff.Staff_ID, a));
            }

            

            ResetPanel();
            
        }

        void ResetPanel()
        {
            LoadEmployeeCategories();
            CategoryList.Visibility = Visibility.Collapsed;
            CategoryGroup.Visibility = Visibility.Visible;
            selectedCategories.Clear();
        }
    }
}
