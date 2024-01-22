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
using System.Windows.Threading;
using System.Reflection;

namespace UserGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User user;
        private MyCoinList coinList;
        private ServiceBrokerClient brokerService = new ServiceBrokerClient();
        private Dictionary<string, decimal> coinsValues;
        private double totValue = 0;
        private List<CoinDesign> coinDesigns;
        private DispatcherTimer dispatcherTimer;

        public MainWindow(User user)
        {
            InitializeComponent();
            this.user = user;

            coinList = brokerService.GetCoinsByUser(user);
            getCoinsValueSimple();
            Console.WriteLine("hello");

            Thread getCoinValueThread = new Thread(getCoinsValue);
            getCoinValueThread.Start();

            coinDesigns = new List<CoinDesign>();
            foreach (var item in coinList)
            {
                double usd = (double)coinsValues.ToList().Find(c => c.Key.ToString().Contains(item.Coin.Symbol)).Value;
                totValue += item.Value * usd;

                CoinDesign cd = new CoinDesign(item, usd);
                coinsPanel.Children.Add(cd);
                coinDesigns.Add(cd);

                PurpuleCoinDesign cp = new PurpuleCoinDesign(item, usd);
                purpuleCoinsPanel.Items.Add(cp);

                PurpuleCoinDesign cpp = new PurpuleCoinDesign(item, usd);
                purpuleCoinsPanelSecond.Items.Add(cpp);

            }



            TOTBalance.Text = totValue.ToString("F2");

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(2);
            dispatcherTimer.Tick += DispatcherTimer_Tick;

            dispatcherTimer.Start();


        }


        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
           updateConvertData();
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void updateConvertData()
        {
            totValue = 0;
            try
            {
                foreach (var item in coinDesigns)
                {
                    double usd = (double)coinsValues.ToList().Find(c => c.Key.ToString().Contains(item.myCoin.Coin.Symbol)).Value;
                    totValue += item.myCoin.Value * usd;
                    item.updateCoinValue(item.myCoin, usd);
                }
                TOTBalance.Text = totValue.ToString("F2");
                Console.WriteLine("sec run");
            }
            catch
            {
                Console.WriteLine("An thread error occured\n");
            }
        }

        private void getCoinsValue()
        {
            try
            {
                while (true)
                {
                    CoinList coins = brokerService.SelectAllCoins();
                    List<String> myCoins = new List<string>();

                    foreach (var item in coins)
                    {
                        myCoins.Add(item.Symbol + "USDT");
                    }
                    this.coinsValues = brokerService.GiveCoinValue(myCoins.ToArray());
                    Console.WriteLine("one run\n");
                    Thread.Sleep(5000);
                }
            }
            catch
            {
                Console.WriteLine("An thread error occured\n");
            }

        }

        private void getCoinsValueSimple()
        {
            CoinList coins = brokerService.SelectAllCoins();
            List<String> myCoins = new List<string>();

            foreach (var item in coins)
            {
                myCoins.Add(item.Symbol + "USDT");
            }
            this.coinsValues = brokerService.GiveCoinValue(myCoins.ToArray());
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ConvertValue.Text = "0";
            ConvertToValue.Text = "0";

            string convertsss = ConvertSymbol.Text;
            ConvertSymbol.Text = ConvertToSymbol.Text;
            ConvertToSymbol.Text = convertsss;

            int index = purpuleCoinsPanelSecond.SelectedIndex;
            purpuleCoinsPanelSecond.SelectedIndex = purpuleCoinsPanel.SelectedIndex;
            purpuleCoinsPanel.SelectedIndex = index;

            ConvertToUSDValue.Text = "0";
            ConvertUSDValue.Text = "0";
        }

        private void ConvertValue_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void convertionClick(object sender, RoutedEventArgs e)
        {
            double value = coinList.FirstOrDefault(item => item.Coin.Symbol == ConvertSymbol.Text)?.Value ?? 0;

            if (double.TryParse(ConvertValue.Text, out double inputValue) && inputValue <= value)
            {
                AfterConvertingSymbol.Text = ConvertSymbol.Text;
                AfterToConvertingSymbol.Text = ConvertToSymbol.Text;
                AfterConvertingValue.Text = ConvertValue.Text;
                AfterToConvertingValue.Text = ConvertToValue.Text;
                ConfirmationButton.Content = "Confirm";
                stepTooSeparator.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1D212C"));
                stepTooNote.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1D212C"));
                stepTooConfirm.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1D212C"));


                StepToo.Visibility = Visibility.Visible;
                ConvertScreen.Visibility = Visibility.Collapsed;
            }
            else
            {

            }
        }

        private void PurpleCoinsPanel_SelectionChanged(object sender, RoutedEventArgs e)
        {
            PurpuleCoinDesign selectedControl = purpuleCoinsPanel.SelectedItem as PurpuleCoinDesign;

            // Check if an item is selected
            if (selectedControl != null)
            {
                // Call the getSymbol function on the selected user control
                ConvertSymbol.Text = selectedControl.getSymbol();

                // Now, 'symbol' holds the result of getSymbol
                // Do something with 'symbol'
            }
        }

        private void PurpleCoinsPanel_SelectionChangedSecond(object sender, RoutedEventArgs e)
        {
            PurpuleCoinDesign selectedControl = purpuleCoinsPanelSecond.SelectedItem as PurpuleCoinDesign;

            // Check if an item is selected
            if (selectedControl != null)
            {
                // Call the getSymbol function on the selected user control
                ConvertToSymbol.Text = selectedControl.getSymbol();

                // Now, 'symbol' holds the result of getSymbol
                // Do something with 'symbol'
            }
        }


        private void ConvertValue_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ConvertValue.Focus();
        }

        private void ConvertValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (coinsValues != null && !string.IsNullOrEmpty(ConvertSymbol.Text))
            {
                if (!string.IsNullOrEmpty(ConvertValue.Text))
                {
                    ConvertUSDValue.Text = ((coinsValues.ToList().Find(c => c.Key.ToString().Contains(ConvertSymbol.Text)).Value) * decimal.Parse(ConvertValue.Text)).ToString("F3");
                    if (!string.IsNullOrEmpty(ConvertToSymbol.Text))
                    {
                        ConvertToValue.Text = ((coinsValues.ToList().Find(c => c.Key.ToString().Contains(ConvertSymbol.Text)).Value * decimal.Parse(ConvertValue.Text)) / (coinsValues.ToList().Find(c => c.Key.ToString().Contains(ConvertToSymbol.Text)).Value)).ToString("F3");
                        if (!string.IsNullOrEmpty(ConvertToValue.Text))
                        {
                            ConvertToUSDValue.Text = ((coinsValues.ToList().Find(c => c.Key.ToString().Contains(ConvertToSymbol.Text)).Value) * decimal.Parse(ConvertToValue.Text)).ToString("F3");
                        }
                    }
                }
                else
                {
                    ConvertUSDValue.Text = "0";
                    ConvertToValue.Text = "0";
                    ConvertToValue.Text = "0";
                }
            }
        }
    }
}
