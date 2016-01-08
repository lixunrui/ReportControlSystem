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
using System.Data;
using System.Threading;

namespace ReportControlSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DatabaseManager dbManager;
        DataTable periodTable;

        public MainWindow()
        {
            InitializeComponent();

            InitializeCustomerComponent();

            ShowLogonPanel();
        }

        private void InitializeCustomerComponent()
        {
            dbManager = new DatabaseManager(@"C:\");
            Reload();
            UpdateGroupsStatus();
        }

        internal void Reload()
        {
            CreateReportGroup();
        }

        private void CreateReportGroup()
        {
            int periodNum;

            // remove all rows
            if (PeriodGrid.RowDefinitions.Count > 0)
            {
                PeriodGrid.RowDefinitions.RemoveRange(0, PeriodGrid.RowDefinitions.Count);
                PeriodGrid.Children.Clear();
            }

            periodTable = dbManager.GetDataTable(SQLStatement.GetPeriodTypeTableQuery());

            if (periodTable == null || periodTable.Rows.Count == 0)
            {
                // create a message label to indicate there is no period type available
                // and offer a button to create default period type table
                RowDefinition periodRow1 = new RowDefinition();

                RowDefinition periodRow2 = new RowDefinition();    

                PeriodGrid.RowDefinitions.Add(periodRow1);
                PeriodGrid.RowDefinitions.Add(periodRow2);

                // add a label to the first row
                TextBlock txtMsg = new TextBlock();
                txtMsg.Text = @"Unable to detect any period type in the database, please either add a new period type from the Period button, or use the button below to create default period types: Daily, Weekly, Fortnightly, Monthly, Quarterly, Annually.";
                txtMsg.TextWrapping = TextWrapping.Wrap;
                txtMsg.FontSize = 15;
                txtMsg.FontWeight = FontWeights.Bold;
                txtMsg.Margin = new Thickness(12);

                btnPeriod.BorderBrush = Brushes.Red;
                btnPeriod.BorderThickness = new Thickness(6); 

                Grid.SetRow(txtMsg, 0);

                PeriodGrid.Children.Add(txtMsg);

                // add a button to the second row
                Button btnAddDefault = new Button();

                btnAddDefault.Content = @"Restore Default Period Types";
                btnAddDefault.FontSize = 15;

                btnAddDefault.Foreground = Brushes.Red;
                btnAddDefault.Margin = new Thickness(12);

                btnAddDefault.VerticalAlignment = VerticalAlignment.Top;

                btnAddDefault.Click += btnAddDefault_Click;

                Grid.SetRow(btnAddDefault, 1);

                PeriodGrid.Children.Add(btnAddDefault);

            }
            else
            {
                periodNum = periodTable.Rows.Count;

                for (int i = 1; i <= periodNum; i++ )
                {
                    RowDefinition periodRow = new RowDefinition();
                    PeriodGrid.RowDefinitions.Add(periodRow);
                }

                // add buttons
                for (int i = 0; i < periodNum; i++ )
                {
                    Button btn = new Button();

                    string btnContent = string.Format("{0} Report", periodTable.Rows[i]["Period_Type"].ToString());

                    btn.Content = btnContent;
                    btn.FontSize = 15;

                  //  btn.Foreground = Brushes.Red;
                    btn.Margin = new Thickness(12);

                    // set tag
                    btn.Tag = periodTable.Rows[i]["Period_Type_ID"];

                    btn.Click += BTN_Period_Type_Clicked;

                    Grid.SetRow(btn, i);

                    PeriodGrid.Children.Add(btn);
                }
                
            }
        }

        // Pop up the report form
        void BTN_Period_Type_Clicked(object sender, RoutedEventArgs e)
        {
            if (periodTable != null)
            {
                int periodID = Convert.ToInt32((sender as Button).Tag);

                string filter = string.Format("Period_Type_ID={0}", periodID);

                DataRow[] rows = periodTable.Select(filter);

                if (rows[0] != null)
                {
                    String periodTitle = rows[0]["Period_Type"].ToString();

                    // pop up the form
                    ReportManager rManager = new ReportManager(this, dbManager);

                    rManager.Owner = this;

                    rManager.Title = periodTitle;

                    rManager.txtReport.Text = periodTitle + "Report";

                    this.Hide();

                    rManager.Show();
                }
            }
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Logon_StoryBoard_Completed(object sender, EventArgs e)
        {
            // check database
            
        }


#region Button Events
        private void BTN_Administratorw_Clicked(object sender, RoutedEventArgs e)
        {
            this.Hide();

            Administrator adminControl = new Administrator(this);

            adminControl.Owner = this;

            adminControl.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            adminControl.Show();
        }

        private void BTN_Period_Clicked(object sender, RoutedEventArgs e)
        {
            this.Hide();

            Period period = new Period(this);

            period.Owner = this;

            period.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            period.Show();
        }

        private void BTN_Category_Clicked(object sender, RoutedEventArgs e)
        {
            this.Hide();

            CategoryManager category = new CategoryManager(this, dbManager);

            category.Owner = this;

            category.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            category.Show();
        }

        private void BTN_Staff_Clicked(object sender, RoutedEventArgs e)
        {

        }


        // Button event to add default period data into table
        void btnAddDefault_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;

            AutoResetEvent m = new AutoResetEvent(true);

            CustomMessageBox auto = new CustomMessageBox(m, "asdasdasd");

            auto.Owner = this;

            auto.Show("asdasdasd");

            result = dbManager.RestoreTable(DataBaseName.Period_Type);

            if (result)
            {
                m.Set();
                Reload();
            }

        }
#endregion

        private void BTN_Payment_Clicked(object sender, RoutedEventArgs e)
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
            txt_username.Focus();
        }

        private void BTN_Logon_Clicked(object sender, RoutedEventArgs e)
        {
            string username = txt_username.Text;
            string password = txt_password.Password;
            if (dbManager.CheckUser(username, password))
            {
                LogonPanel.Visibility = Visibility.Collapsed;
                this.Title = string.Format("Welcome {0} to RCS", username);
                UpdateGroupsStatus(true);
            }
            else
            {
                lbl_logon_status.Content = "Status: Incorrect Logon Details.";
            }
        }

        private void UpdateGroupsStatus(bool show = false)
        {
            Visibility v;
            if (show)
            {
                v = Visibility.Visible;
            }
            else
                v = Visibility.Collapsed;


            MaintenanceGroup.Visibility = v;
            ReportGroup.Visibility = v;
            PaymentGroup.Visibility = v;
        }
    }
}
