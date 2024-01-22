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
    /// Interaction logic for PurpuleCoinDesign.xaml
    /// </summary>
    public partial class PurpuleCoinDesign : UserControl
    {
        public MyCoin myCoin { get; }
        public PurpuleCoinDesign(MyCoin coin, double val)
        {
            InitializeComponent();
            this.DataContext = coin;
            myCoin = coin;
            changeVal.Text = $"1 {coin.Coin.Symbol} (${val.ToString()})";

            try
            {
                imgCoin.Source = new BitmapImage(new Uri($"pack://application:,,,/images/purpuleCoins/{coin.Coin.Symbol}.png"));
            }
            catch
            {
                imgCoin.Source = null;//new BitmapImage(new Uri($"pack://application:,,,/images/Coins/{logo}.png"));
            }
        }

        public string getSymbol()
        {
            return this.myCoin.Coin.Symbol;
        }
    }
}
