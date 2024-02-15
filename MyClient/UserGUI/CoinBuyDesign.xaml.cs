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
    /// Interaction logic for CoinBuyDesign.xaml
    /// </summary>
    public partial class CoinBuyDesign : UserControl
    {
        public MyCoin myCoin { get; }
        public event EventHandler CoinBuyClicked;
        public event EventHandler CoinSellClicked;
        public CoinBuyDesign(MyCoin coin, double usd)
        {
            InitializeComponent();
            this.DataContext = coin;
            myCoin = coin;
            TOTBTCVALUE.Text = $"${coin.Value * usd}";
            try
            {
                imgCoin.Source = new BitmapImage(new Uri($"pack://application:,,,/images/Coins/{coin.Coin.Symbol}.png"));
            }
            catch
            {
                imgCoin.Source = null;//new BitmapImage(new Uri($"pack://application:,,,/images/Coins/{logo}.png"));
            }
        }

        public void updateCoinValue(MyCoin coin, double input)
        {
            TOTBTCVALUE.Text = $"${coin.Value * input}";
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CoinSellClicked?.Invoke(this, EventArgs.Empty);
        }

        private void TextBlock_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            CoinBuyClicked?.Invoke(this, EventArgs.Empty);
        }
    
    }
}
