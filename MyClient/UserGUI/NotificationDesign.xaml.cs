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
using UserGUI.BrokerReference;

namespace UserGUI
{
    /// <summary>
    /// Interaction logic for NotificationDesign.xaml
    /// </summary>
    public partial class NotificationDesign : UserControl
    {
        private string Sender;
        private string Reciever;
        private string Data;
        Notification myNotification;
        public NotificationDesign(Notification myNotification)
        {
            InitializeComponent();
            this.DataContext = myNotification;
            this.Sender = myNotification.Sender;
            this.Reciever = myNotification.Reciever;
            this.Data = myNotification.Data;
            this.myNotification = myNotification;
        }

        public Notification getNotification()
        {
            return this.myNotification;
        }

        public string getSender()
        {
            return this.Sender;
        }

        public string getReciever()
        {
            return this.Reciever;
        }

        public string getData()
        {
            return this.Data;
        }

        
    }
}
