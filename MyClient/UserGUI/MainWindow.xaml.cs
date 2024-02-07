﻿using System;
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
using System.Text.RegularExpressions;

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
        private OrderHistoryList orderHistoryList;
        private int spaceBetween = 30;
        private User newUser = new User();
        private bool under18;

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
            if (StepToo.Visibility == Visibility.Collapsed)
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
            else
            {
                MyCoin changeFromCoin = null;
                MyCoin changeToCoin = null;
                foreach (var item in coinList)
                {
                    if (item.Coin.Symbol == AfterConvertingSymbol.Text)
                    {
                        changeFromCoin = item;
                    }
                    if (item.Coin.Symbol == AfterToConvertingSymbol.Text)
                    {
                        changeToCoin = item;
                    }
                }
                if (changeFromCoin != null && changeToCoin != null)
                {
                    changeFromCoin.Value = changeFromCoin.Value - double.Parse(AfterConvertingValue.Text);
                    brokerService.UpdateMyCoin(changeFromCoin);

                    changeToCoin.Value = changeToCoin.Value + double.Parse(AfterToConvertingValue.Text);
                    brokerService.UpdateMyCoin(changeToCoin);
                }

                StepToo.Visibility = Visibility.Collapsed;
                succesfullScreen.Visibility = Visibility.Visible;
                ConfirmationButton.Visibility = Visibility.Collapsed;

                stepThreeSeparator.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1D212C"));
                stepTreeNote.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1D212C"));
                stepThreeSuccess.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1D212C"));

                dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Interval = TimeSpan.FromSeconds(4);
                dispatcherTimer.Tick += succesfullTimer;

                dispatcherTimer.Start();

            }
        }

        private void CancelButtonPressed(object sender, RoutedEventArgs e)
        {
            ConfirmationButton.Content = "Convert Now";
            stepTooSeparator.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#92969F"));
            stepTooNote.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#92969F"));
            stepTooConfirm.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#92969F"));


            StepToo.Visibility = Visibility.Collapsed;
            ConvertScreen.Visibility = Visibility.Visible;
        }

        private void PurpleCoinsPanel_SelectionChanged(object sender, RoutedEventArgs e)
        {
            PurpuleCoinDesign selectedControl = purpuleCoinsPanel.SelectedItem as PurpuleCoinDesign;

            // Check if an item is selected
            if (selectedControl != null)
            {
                // Call the getSymbol function on the selected user control
                ConvertSymbol.Text = selectedControl.getSymbol();
                changeMyConvertionValues();

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
                changeMyConvertionValues();


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
            changeMyConvertionValues();
        }

        private void succesfullTimer(object sender, EventArgs e)
        {
            succesfullScreen.Visibility = Visibility.Collapsed;
            ConvertScreen.Visibility = Visibility.Visible;
            ConfirmationButton.Visibility = Visibility.Visible;

            stepTooSeparator.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#92969F"));
            stepTooNote.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#92969F"));
            stepTooConfirm.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#92969F"));

            stepThreeSeparator.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#92969F"));
            stepTreeNote.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#92969F"));
            stepThreeSuccess.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#92969F"));
            dispatcherTimer.Stop();
        }


        private void changeMyConvertionValues()
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





        private void tradeSelectionClick(object sender, RoutedEventArgs e)
        {
            collapseAllElipses();
            tradeSelectionEllipse.Visibility = Visibility.Visible;
        }

        private void convertSelectionClick(object sender, RoutedEventArgs e)
        {
            collapseAllElipses();
            convertSelectionEllipse.Visibility = Visibility.Visible;
            convertSecondColumn.Visibility = Visibility.Visible;
            convertThirdColums.Visibility = Visibility.Visible;
        }

        private void walletSelectionClick(object sender, RoutedEventArgs e)
        {
            collapseAllElipses();
            walletSelectionEllipse.Visibility = Visibility.Visible;
        }

        private void prizeSelectionClick(object sender, RoutedEventArgs e)
        {
            collapseAllElipses();
            prizeSelectionEllipse.Visibility = Visibility.Visible;
        }

        private void profileSelectionClick(object sender, RoutedEventArgs e)
        {
            newUser.UserName = user.UserName;
            newUser.Password = user.Password;
            newUser.BirthDate = user.BirthDate;
            newUser.Email = user.Email;
            newUser.FirstName = user.FirstName;
            newUser.SecondName = user.SecondName;
            newUser.IsAdmin = user.IsAdmin;
            newUser.ID = user.ID;
            


            collapseAllElipses();
            
            this.DataContext = newUser;
            profileSelectionEllipse.Visibility = Visibility.Visible;
            profileSecondColumn.Visibility = Visibility.Visible;
            profileFirstColumn.Visibility = Visibility.Visible;
        }

        private void collapseAllElipses()
        {
            tradeSelectionEllipse.Visibility = Visibility.Collapsed;
            convertSelectionEllipse.Visibility = Visibility.Collapsed;
            walletSelectionEllipse.Visibility = Visibility.Collapsed;
            prizeSelectionEllipse.Visibility = Visibility.Collapsed;
            profileSelectionEllipse.Visibility = Visibility.Collapsed;

            convertSecondColumn.Visibility = Visibility.Collapsed;
            convertThirdColums.Visibility = Visibility.Collapsed;

            profileSecondColumn.Visibility = Visibility.Collapsed;
            profileFirstColumn.Visibility = Visibility.Collapsed;

        }



        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            this.Close();
            Login loginWindow = new Login();
            loginWindow.ShowDialog();

        }

        private void OrderHistoryButtonClick(object sender, RoutedEventArgs e)
        {
            hideAllProfile();
            this.spaceBetween = 30;

            var textBlocksToRemove = new List<TextBlock>();

            foreach (var child in OrderHistoryPanel.Children)
            {
                if (child is TextBlock textBlock && textBlock.Tag != null && textBlock.Tag.ToString() == "data1")
                {
                    textBlocksToRemove.Add(textBlock);
                }
            }

            foreach (var textBlockToRemove in textBlocksToRemove)
            {
                OrderHistoryPanel.Children.Remove(textBlockToRemove);
            }

            OrderHistoryProfile.Visibility = Visibility.Visible;
            orderHistoryList = brokerService.SelectOrderHistoryByUser(user);


            foreach (var item in orderHistoryList)
            {
                // Create TextBlocks for each column and set their text
                TextBlock symbolText = new TextBlock() { Text = item.Symbol, FontSize = 25, Margin = new Thickness(0,spaceBetween,0,0), Tag = "data1" };
                TextBlock sideText = new TextBlock() { Text = item.Side, FontSize = 25, Margin = new Thickness(0,spaceBetween,0,0), Tag = "data1" };
                TextBlock typeText = new TextBlock() { Text = item.Type, FontSize = 25, Margin = new Thickness(0,spaceBetween,0,0), Tag = "data1" };
                TextBlock qtyText = new TextBlock() { Text = item.Qty.ToString(), FontSize = 25, Margin = new Thickness(0,spaceBetween,0,0), Tag = "data1" };
                TextBlock priceText = new TextBlock() { Text = item.Price.ToString(), FontSize = 25, Margin = new Thickness(0,spaceBetween,0,0), Tag = "data1" };
                TextBlock fillPriceText = new TextBlock() { Text = item.FillPrice.ToString(), FontSize = 25, Margin = new Thickness(0,spaceBetween,0,0), Tag = "data1" };
                TextBlock statusText = new TextBlock() { Text = item.Status, FontSize = 25, Margin = new Thickness(0,spaceBetween,0,0), Tag = "data1" };
                TextBlock placingTimeText = new TextBlock() { Text = item.Placingtime.ToString("MM/dd/yyyy"), FontSize = 25, Margin = new Thickness(0,spaceBetween,0,0), Tag = "data1" };
                TextBlock closingTimeText = new TextBlock() { Text = item.ClosingTime.ToString("MM/dd/yyyy"), FontSize = 25, Margin = new Thickness(0,spaceBetween,0,0), Tag = "data1" };

                // Add TextBlocks to the grid with appropriate column
                OrderHistoryPanel.Children.Add(symbolText);
                OrderHistoryPanel.Children.Add(sideText);
                OrderHistoryPanel.Children.Add(typeText);
                OrderHistoryPanel.Children.Add(qtyText);
                OrderHistoryPanel.Children.Add(priceText);
                OrderHistoryPanel.Children.Add(fillPriceText);
                OrderHistoryPanel.Children.Add(statusText);
                OrderHistoryPanel.Children.Add(placingTimeText);
                OrderHistoryPanel.Children.Add(closingTimeText);

                // Set the Grid.Row property for each TextBlock
                Grid.SetColumn(symbolText, 0);
                Grid.SetColumn(sideText, 1);
                Grid.SetColumn(typeText, 2);
                Grid.SetColumn(qtyText, 3);
                Grid.SetColumn(priceText, 4);
                Grid.SetColumn(fillPriceText, 5);
                Grid.SetColumn(statusText, 6);
                Grid.SetColumn(placingTimeText, 7);
                Grid.SetColumn(closingTimeText, 8);
                // Add the grid to the StackPanel
                //Grid.SetRow(rowGrid, OrderHistoryPanel.RowDefinitions.Count - 1);
                this.spaceBetween += 30;
            }

        }

        private void UpdateUserClick(object sender, RoutedEventArgs e)
        {
            hideAllProfile();
            UpdateUserColumn.Visibility = Visibility.Visible;


        }



        private void UpdateInformationClick(object sender, RoutedEventArgs e)
        {

            bool Errr = false;
            User myUser = new User();
            User IfMailExists = brokerService.SelectUserByEmail(changeEmail.Text);
            User IfUserNameExists = brokerService.SelectUserByUserName(changeUserName.Text);

            EmailErr.Visibility = Visibility.Collapsed;
            UserNameErr.Visibility = Visibility.Collapsed;
            FirstNameErr.Visibility = Visibility.Collapsed;
            SecNameErr.Visibility = Visibility.Collapsed;
            DateErr.Visibility = Visibility.Collapsed;
            PassErr.Visibility = Visibility.Collapsed;

            if (!checkUnder18())
            {
                DateErr.Visibility = Visibility.Visible;
                SetToolTip(DateErr, "Not over 18");
                Errr = true;
            }
            if (!changeEmail.Text.Contains("@gmail.com"))
            {
                EmailErr.Visibility = Visibility.Visible;
                SetToolTip(EmailErr, "Email must contain @gmail.com");
                Errr = true;
            }
            if (string.IsNullOrEmpty(changeEmail.Text))
            {
                EmailErr.Visibility = Visibility.Visible;
                SetToolTip(EmailErr, "Email is required");
                Errr = true;
            }
            else if (IfMailExists != null && user.Email != changeEmail.Text)
            {
                EmailErr.Visibility = Visibility.Visible;
                SetToolTip(EmailErr, "Email already exists");
                Errr = true;
            }
            if (IfUserNameExists != null && user.UserName != changeUserName.Text)
            {
                UserNameErr.Visibility = Visibility.Visible;
                SetToolTip(UserNameErr, "Username already exists");
                Errr = true;
            }
            if (string.IsNullOrEmpty(changeFirstName.Text))
            {
                FirstNameErr.Visibility = Visibility.Visible;
                SetToolTip(FirstNameErr, "First Name is required");
                Errr = true;
            }
            if (string.IsNullOrEmpty(changeSecondName.Text))
            {
                SecNameErr.Visibility = Visibility.Visible;
                SetToolTip(SecNameErr, "Second Name is required");
                Errr = true;
            }
            if (!(changeUserName.Text.Length >= 5))
            {
                UserNameErr.Visibility = Visibility.Visible;
                SetToolTip(UserNameErr, "Username must be at least 5 characters");
                Errr = true;
            }

            if (!Regex.IsMatch(changePassword.Text, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
            {
                PassErr.Visibility = Visibility.Visible;
                SetToolTip(PassErr, "Password requirements not met, password should contain at least 1 capital letter, 1 regular letter and be 8 lengh");
                Errr = true;
            }
            if (!Errr)
            {
                user.UserName = newUser.UserName;
                user.Password = newUser.Password;
                user.BirthDate = newUser.BirthDate;
                user.Email = newUser.Email;
                user.FirstName = newUser.FirstName;
                user.SecondName = newUser.SecondName;
                user.IsAdmin = newUser.IsAdmin;
                user.ID = newUser.ID;



                brokerService.UpdateUser(user);
            }
        }







        private void hideAllProfile()
        {
            OrderHistoryProfile.Visibility = Visibility.Collapsed;
            UpdateUserColumn.Visibility = Visibility.Collapsed;
        }

        private void SetToolTip(UIElement element, string errorMessage)
        {
            ToolTipService.SetToolTip(element, errorMessage);
        }

        private bool checkUnder18()
        {
            if (birthdateDatePicker.SelectedDate.HasValue)
            {
                DateTime selectedDate = birthdateDatePicker.SelectedDate.Value;
                DateTime currentDate = DateTime.Today;

                int age = currentDate.Year - selectedDate.Year;

                // Check if the birthday has occurred this year
                if (selectedDate.Date > currentDate.AddYears(-age))
                {
                    age--;
                }

                // Assuming 18 is the legal age
                if (age >= 18)
                {
                    // Person is over 18
                    // You can perform further actions or validations here
                    return true;
                }
                else
                {
                    // Person is under 18
                    return false;
                }
            }
            return false;
        }


    }
}