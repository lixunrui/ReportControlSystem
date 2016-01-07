using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        
        EventWaitHandle[] handler = new EventWaitHandle[1];
        public CustomMessageBox()
        {
            InitializeComponent();
        }

        public CustomMessageBox(AutoResetEvent eventFlag, string message)
            : this()
        {
            handler[0] = eventFlag;
            txtMsg.Text = message;
        }

        //TODO: cannot display the message
        internal void Show(String message)
        {
            this.Show();
            txtMsg.Text = message;
            Thread monitor = new Thread(StartMonitor);
            monitor.Start();
            //monitor.Join();

        }

        void StartMonitor()
        {
            int result = EventWaitHandle.WaitAny(handler, 6000);

            if (result == 0)
            {
                this.Dispatcher.BeginInvoke((Action)(() =>
                {
                    this.Close();
                }));
            }
        }
    }
}
