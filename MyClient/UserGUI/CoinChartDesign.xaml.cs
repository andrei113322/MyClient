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
    /// Interaction logic for CoinChartDesign.xaml
    /// </summary>
    public partial class CoinChartDesign : UserControl
    {
        public MyCoin myCoin { get; }
        public event EventHandler CoinClicked;

        public CoinChartDesign(MyCoin coin)
        {
            InitializeComponent();
            InitializeControl();
            this.DataContext = coin.Coin;
            myCoin = coin;
            try
            {
                imgCoin.Source = new BitmapImage(new Uri($"pack://application:,,,/images/Coins/{coin.Coin.Symbol}.png"));
            }
            catch
            {
                imgCoin.Source = null;//new BitmapImage(new Uri($"pack://application:,,,/images/Coins/{logo}.png"));
            }
        }

        private void InitializeControl()
        {
            // Wire up the click event handler
            this.MouseLeftButtonDown += CoinChartDesign_MouseLeftButtonDown;
        }

        private void CoinChartDesign_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Raise the CoinClicked event when the control is clicked
            CoinClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
