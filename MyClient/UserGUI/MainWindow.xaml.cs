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
using System.Threading;

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
        private Dictionary<string, decimal> coinsValues;
        private double totValue = 0;


        public MainWindow(User user)
        {
            InitializeComponent();
            this.user = user;
            coinList = brokerDB.SelectCoinByUser(user);
            getCoinsValue();


            foreach (var item in coinList)
            {
                switch(item.Key.Symbol)
                {
                    case "BTC":
                        BTCValue.Text = item.Value.ToString();
                        TOTBTCVALUE.Text = ((double)coinsValues["BTCUSDT"] * item.Value).ToString("F2");
                        totValue += ((double)coinsValues["BTCUSDT"] * item.Value);
                        break;
                    case "ETH":
                        ETHValue.Text = item.Value.ToString();
                        TOTETHVALUE.Text = ((double)coinsValues["ETHUSDT"]*item.Value).ToString("F2");
                        totValue += ((double)coinsValues["ETHUSDT"] * item.Value);
                        break;
                    default:
                        break;
                }
            }

            TOTBalance.Text = totValue.ToString("F2");
            //Thread thread = new Thread(new ThreadStart(getCoinsValue));
            //thread.Start();
            //thread.Join();
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void getCoinsValue()
        {
            CoinList coins = brokerDB.SelectAllCoins();
            List<String> myCoins = new List<string>();

            foreach (var item in coins)
            {
                myCoins.Add(item.Symbol + "USDT");
            }

            this.coinsValues = brokerDB.GiveCoinValue(myCoins.ToArray());
        }
    }
}
