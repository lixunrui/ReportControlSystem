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
    /// Interaction logic for AdminConfiguration.xaml
    /// </summary>
    public partial class AdminConfiguration : Window
    {
        Window _parent;
        DatabaseManager db_manager;
        int currentID;
        User currentUser;

        internal EventHandler<ObjectPassedEventArgs> UserUpdatedEvent;

        public AdminConfiguration()
        {
            InitializeComponent();
        }

        internal AdminConfiguration(Window main, DatabaseManager manager, int userID)
            : this()
        {
            _parent = main;
            db_manager = manager;
            currentID = userID;
            LoadUser();
        }

        void LoadUser()
        {
            DataTable userTable = db_manager.GetDataTable(SQLStatement.GetUserFromIDQuery(currentID));

            if (userTable != null)
            {
                foreach (DataRow row in userTable.Rows)
                {
                    currentUser = new User(Convert.ToInt32(row[Constants.UserElements.UserID]),
                    row[Constants.UserElements.LoginName].ToString(),
                    row[Constants.UserElements.Password].ToString());
                }

                txtLoginName.Text = currentUser.Login_Name;
                //txtLoginName.IsReadOnly = true;
                btnUpdate.IsEnabled = false;
            }
        }

        private void BTN_Cancel_Clicked(object sender, RoutedEventArgs e)
        {
            ExitMethod();
        }

        private void ExitMethod()
        {
            this.Close();
            _parent.Show();
        }

        private void BTN_Update_Clicked(object sender, RoutedEventArgs e)
        {
            if (txtNewPWD.Password.Length > 0)
            {
                string newPassword = MD5PWD.MD5HashPassword(txtNewPWD.Password);
                db_manager.LoadSQLTextFile(SQLStatement.GetUserUpdateFromID(currentID, txtLoginName.Text, newPassword));
                if (UserUpdatedEvent!=null)
                {
                    UserUpdatedEvent(this, null);
                    ExitMethod();
                   // this.DialogResult = true;
                }
            }
        }

        private void Event_LostFocus_CheckPWD(object sender, RoutedEventArgs e)
        {
            if (txtOldPWD.Password.Length > 0)
            {
                if (currentUser.Password.Equals(MD5PWD.MD5HashPassword(txtOldPWD.Password)))
                {
                    // password ok,
                    btnUpdate.IsEnabled = true;
                }
                else
                {
                    txtOldPWD.Foreground = Brushes.Red;
                }
            }
        }

        private void Event_GotFocus_OldPWD(object sender, RoutedEventArgs e)
        {
            txtOldPWD.Password = string.Empty;
            txtOldPWD.Foreground = Brushes.Black;
        }
    }
}
