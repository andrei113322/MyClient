using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User user;
        private Dictionary<Coin, double> coinList;
        private ServiceBrokerClient brokerDB = new ServiceBrokerClient();

        public MainWindow(User user)
        {
            this.user = user;
            coinList = brokerDB.SelectCoinByUser(user);

            foreach (var item in coinList)
            {
                switch(item.Key.Symbol)
                {
                    case "BTC":
                        BTCValue.Text = item.Value.ToString();
                        break;
                    case "ETH":
                        ETHValue.Text = item.Value.ToString();
                        break;
                    default:
                        break;
                }
            }
            InitializeComponent();
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
