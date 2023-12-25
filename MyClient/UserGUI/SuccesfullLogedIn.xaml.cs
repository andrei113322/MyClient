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
using System.Windows.Threading;
using UserGUI.BrokerReference;

namespace UserGUI
{
    /// <summary>
    /// Interaction logic for SuccesfullLogedIn.xaml
    /// </summary>
    public partial class SuccesfullLogedIn : Window
    {
        private DispatcherTimer timer;
        private ServiceBrokerClient brokerDB = new ServiceBrokerClient();
        private User myUser;

        public SuccesfullLogedIn(User myUser)
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += Timer_Tick;
            this.myUser = myUser;

            // Start the timer
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            MainWindow man = new MainWindow(myUser);
            this.Close();
            man.ShowDialog();
        }
    }
}
