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
                    Category c = new Category(r["Category_Name"].ToString(), Convert.ToBoolean(r["Category_Type"]), r["Category_Description"].ToString());
                    category.Add(c);
                }

                CategoryListView.DataContext = category;
            }
        }

        private void BTN_Add_New_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void BTN_Delete_Clicked(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;

            if (cmd.DataContext is Category)
            {
                Category c = (Category)cmd.DataContext;
            }
        }

        private void BTN_Update_Clicked(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;

            if (cmd.DataContext is Category)
            {
                Category c = (Category)cmd.DataContext;
            }
        }

        private void BTN_Back_Clicked(object sender, RoutedEventArgs e)
        {
            _parent.Show();

            this.Close();
        }

        
    }
}
