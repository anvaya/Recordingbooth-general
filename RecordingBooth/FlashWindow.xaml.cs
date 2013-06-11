using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RecordingBooth
{
    /// <summary>
    /// Interaction logic for FlashWindow.xaml
    /// </summary>
    public partial class FlashWindow : Window
    {
        private System.Windows.Threading.DispatcherTimer dispatcherTimer;

        public FlashWindow()
        {
            InitializeComponent();

            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 3);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            HostWindow win = new HostWindow();
            win.Show();
            dispatcherTimer.Stop();
            dispatcherTimer.IsEnabled = false;
            this.Close();
        }

    }
}
