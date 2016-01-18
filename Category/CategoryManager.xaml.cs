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
using System.Data;

namespace ReportControlSystem
{
    /// <summary>
    /// Interaction logic for CategoryManager.xaml
    /// </summary>
    public partial class CategoryManager : Window
    {
        MainWindow _parent;
        DatabaseManager db_Manager;
        DataTable categoryTable;

        public CategoryManager()
        {
            InitializeComponent();
        }

        internal CategoryManager(MainWindow form, DatabaseManager m)
            : this()
        {
            _parent = form;
            this.Owner = form;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            db_Manager = m;
            InitializeCustomComponent();
        }

        private void Windows_Closed(object sender, EventArgs e)
        {
            _parent.Show();

            this.Close();
        }

        private void InitializeCustomComponent()
        {
            categoryTable = db_Manager.GetDataTable(SQLStatement.GetCategoryTableQuery());

            if (categoryTable != null)
            {
                List<Category> category = new List<Category>();

                foreach (DataRow r in categoryTable.Rows)
                {
                    Category c = new Category(Convert.ToInt32(r["Category_ID"]),r["Category_Name"].ToString(), Convert.ToBoolean(r["Category_Type"]), r["Category_Description"].ToString());
                    category.Add(c);
                }

                CategoryListView.DataContext = category;
            }
        }

        private void BTN_Add_New_Clicked(object sender, RoutedEventArgs e)
        {
            CategoryAddingDialog catAddingDia = new CategoryAddingDialog(this);

            catAddingDia.CategoryPassed += NewCategoryPassed;

            catAddingDia.Owner = this;

            catAddingDia.ShowDialog();
        }

        void NewCategoryPassed(object sender, ObjectPassedEventArgs e)
        {
            CategoryAddingDialog form = (CategoryAddingDialog)sender;

            form.Close();
            
            this.Show();

            Category c = (Category)e.item;

            // update the source table

            db_Manager.LoadSQLTextFile(SQLStatement.GetInsertCategoryTableQuery(c));

            InitializeCustomComponent();
        }

        void CategoryUpdated(object sender, ObjectPassedEventArgs e)
        {
            CategoryAddingDialog form = (CategoryAddingDialog)sender;

            form.Close();

            this.Show();

            Category c = (Category)e.item;

            // update the source table

            db_Manager.LoadSQLTextFile(SQLStatement.GetUpdateFromCategory(c));

            InitializeCustomComponent();
        }

        private void BTN_Delete_Clicked(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;

            if (cmd.DataContext is Category)
            {
                Category c = (Category)cmd.DataContext;
                db_Manager.LoadSQLTextFile(SQLStatement.GetDeleteFromCategory(c));
                InitializeCustomComponent();
            }
        }

        private void BTN_Update_Clicked(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;

            if (cmd.DataContext is Category)
            {
                Category c = (Category)cmd.DataContext;

                CategoryAddingDialog catAddingDia = new CategoryAddingDialog(this, c);

                catAddingDia.CategoryPassed += CategoryUpdated;

                catAddingDia.Owner = this;

                catAddingDia.Focus();

                catAddingDia.Show();

            }
        }


        private void BTN_Back_Clicked(object sender, RoutedEventArgs e)
        {
            _parent.Show();

            this.Close();
        }

        
    }


 
}
