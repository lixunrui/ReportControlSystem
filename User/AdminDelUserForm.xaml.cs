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
using System.Data.SQLite;

namespace ReportControlSystem
{
    /// <summary>
    /// Interaction logic for AminAddForm.xaml
    /// </summary>
    public partial class AdminDeleteUserForm : Window
    {
        Window _parent;
        DatabaseManager db_manager;
        Int32 currentID;

        internal EventHandler<ObjectPassedEventArgs> deleteUserEvent;

        public AdminDeleteUserForm()
        {
            InitializeComponent();
        }

        internal AdminDeleteUserForm(Window parent, DatabaseManager manager, int userID)
        :   this()
        {
            _parent = parent;
            this.Owner = parent;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            currentID = userID;
        }

        private void BTN_Cancel_Clicked(object sender, RoutedEventArgs e)
        {
            ExitMethod();
        }

        private void BTN_Delete_Clicked(object sender, RoutedEventArgs e)
        {
            if (currentID > 0)
            {
                //TODO: Check remaining admin number
                if (currentID == 1)
                {
                    MessageBox.Show("Cannot delete the default user for now, this function will be added later.");
                    ExitMethod();
                }
                else
                {
                    // get the password
                    DataTable users = db_manager.GetDataTable(SQLStatement.GetUserFromIDQuery(currentID));

                    SQLiteDataReader reader = db_manager.ExecuteSQLTextFile(SQLStatement.GetUserFromIDQuery(currentID));
                    while(reader.Read())
                    {
                        if (reader[Constants.UserElements.Password].ToString().Equals(MD5PWD.MD5HashPassword(txtPWD.Password)))
                        {
                            db_manager.LoadSQLTextFile(SQLStatement.GetDeleteFromUser(currentID));
                            if (deleteUserEvent != null)
                            {
                                deleteUserEvent(this, null);
                                ExitMethod();
                            }

                        }
                    }



                }

            }
        }

        private void ExitMethod()
        {
            this.Close();
            _parent.Show();
        }
    }
}
