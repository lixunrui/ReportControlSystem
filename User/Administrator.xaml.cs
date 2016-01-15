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
    /// Interaction logic for Administrator.xaml
    /// </summary>
    public partial class Administrator : Window
    {
        MainWindow _parent;
        DatabaseManager db_manager;
        DataTable userTable;
        User currentSelectedUser;


        public Administrator()
        {
            InitializeComponent();
        }

        internal Administrator(MainWindow main, DatabaseManager manager)
            : this()
        {
            _parent = main;
            db_manager = manager;
            this.Owner = main;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            LoadUserTable();
        }

        void LoadUserTable()
        {
            userTable = db_manager.GetDataTable(SQLStatement.GetUserTableQuery());

            List<User> users = new List<User>();

            foreach (DataRow row in userTable.Rows)
            {
                User user = new User(Convert.ToInt32(row[Constants.UserElements.UserID]),
                    row[Constants.UserElements.LoginName].ToString(),
                    row[Constants.UserElements.Password].ToString());

                users.Add(user);
            }

            UserListView.ItemsSource = users;
        }

        private void Windows_Closed(object sender, EventArgs e)
        {
            _parent.Show();
            this.Close();
        }

        private void BTN_Update_Clicked(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                AdminConfiguration config = new AdminConfiguration(this, db_manager, Convert.ToInt32(btn.Tag));
                config.UserUpdatedEvent += new EventHandler<ObjectPassedEventArgs>(UpdatedUser);
                config.ShowDialog();
            }
            
        }

        private void UpdatedUser(object sender, ObjectPassedEventArgs e)
        {
            LoadUserTable();
        }

        private void BTN_Delete_Clicked(object sender, RoutedEventArgs e)
        {
            //Button btn = sender as Button;

            //if (btn != null)
            //{
            //    if (Convert.ToInt32(btn.Tag) == 1) //default user
            //    {
            //        MessageBox.Show("Should not delete the default user");
            //    }
            //    else
            //    {
            //        db_manager.LoadSQLTextFile(SQLStatement.GetDeleteFromUser(Convert.ToInt32(btn.Tag));
            //        LoadUserTable();
            //    }
          
            //}
        }
    }
}
