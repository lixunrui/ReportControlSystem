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
        internal int DialogIndex;
        //EventWaitHandle[] handler = new EventWaitHandle[1];

        ManualResetEvent resetEvent;

        internal CustomMessageBox()
        {
            InitializeComponent();
        }

        internal CustomMessageBox(Window parent, string message=null)
            : this()
        {
            this.Owner = parent;
            txtMsg.Text = message;
        }

        internal CustomMessageBox(Window parent, string message, ManualResetEvent _resetEvent)
            :this(parent,message)
        {
            resetEvent = _resetEvent;
        }

        internal CustomMessageBox(Window parent, string message, params string[] buttonText) 
            : this(parent, message )
        {
            DrawTheDialog(buttonText.Length, buttonText);
        }

        void DrawTheDialog(int buttonNum, params string[] buttonTexts)
        {
            for (int i = 0; i < buttonNum; i++ )
            {
                ColumnDefinition colDef = new ColumnDefinition();

                GridLayout.ColumnDefinitions.Add(colDef);

                Button btn = new Button();

                btn.Content=buttonTexts[i];

                btn.Tag = i;

                btn.Margin = new Thickness(12,3,12,3);

                btn.Click += BTN_Clicked;

                Grid.SetColumn(btn, i);
                Grid.SetRow(btn, 2);

                GridLayout.Children.Add(btn);
            }
        }

        private void BTN_Clicked(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (btn != null)
            {
                DialogIndex = Convert.ToInt32(btn.Tag);
                this.Close();
            }
        }

        internal void WaitUntiReceiveSign()
        {
            resetEvent.WaitOne();
            this.Close();
        }
    }
}
