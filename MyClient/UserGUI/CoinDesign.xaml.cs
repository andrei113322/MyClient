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
    /// Interaction logic for CoinDesign.xaml
    /// </summary>
    public partial class CoinDesign : UserControl
    {
        public MyCoin myCoin { get; }
        public CoinDesign(MyCoin coin, double usd)
        {
            InitializeComponent();
            this.DataContext = coin.Coin;
            myCoin = coin;
            TOTBTCVALUE.Text = $"${coin.Value*usd}";
            try
            {
                imgCoin.Source = new BitmapImage(new Uri($"pack://application:,,,/images/Coins/{coin.Coin.Symbol}.png"));
            }
            catch
            {
                imgCoin.Source = null;//new BitmapImage(new Uri($"pack://application:,,,/images/Coins/{logo}.png"));
            }
        }

        public void updateCoinValue( MyCoin coin ,double input)
        {
            TOTBTCVALUE.Text = $"${coin.Value * input}";
        }
    }
}
