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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace ReportControlSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DatabaseManager manager;

        public MainWindow()
        {
            InitializeComponent();

            InitializeCustomerComponent();

            ShowLogonPanel();
        }

        private void InitializeCustomerComponent()
        {
            manager = new DatabaseManager();
        }

        

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void Logon_StoryBoard_Completed(object sender, EventArgs e)
        {

        }
    }

    /// <summary>
    /// Logon Panel Button Events
    /// </summary>
    public partial class MainWindow : Window
    {
        private void ShowLogonPanel()
        {
            LogonPanel.Visibility = Visibility.Visible;

            StartAnimation("open_logon");
        }

        void StartAnimation(String StoryBoardName)
        {
            Storyboard currentStoryBoard;
            try
            {
                currentStoryBoard = this.FindResource(StoryBoardName) as Storyboard;
                currentStoryBoard.Begin(this, false);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
        }
        
        private void BTN_Clear_Clicked(object sender, RoutedEventArgs e)
        {
            txt_username.Text = String.Empty;
            txt_password.Password = String.Empty;
            lbl_logon_status.Content = "Status:";
        }

        private void BTN_Logon_Clicked(object sender, RoutedEventArgs e)
        {
            string username = txt_username.Text;
            string password = txt_password.Password;
            if (manager.CheckUser(username, password))
            {
                LogonPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                lbl_logon_status.Content = "Status: Incorrect Logon Details.";
            }
        }
    }
}
