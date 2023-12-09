using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UserGUI.BrokerReference;

namespace UserGUI
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {

        private DateTime previousSelectedDate;
        private ServiceBrokerClient brokerDB = new ServiceBrokerClient();

        public SignUp()
        {
            InitializeComponent();
            previousSelectedDate = birthdateDatePicker.SelectedDate ?? DateTime.MinValue;
            birthdateDatePicker.SelectedDate = DateTime.Today;
        }


        private void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtEmail.Focus();
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEmail.Text) && txtEmail.Text.Length > 0)
            {
                textEmail.Visibility = Visibility.Collapsed;
            }
            else
            {
                textEmail.Visibility = Visibility.Visible;
            }
        }

        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Focus();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0)
            {
                textPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                textPassword.Visibility = Visibility.Visible;
            }
        }

        private void textUserName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtUserName.Focus();
        }

        private void txtUserName_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUserName.Text) && txtUserName.Text.Length > 0)
            {
                textUserName.Visibility = Visibility.Collapsed;
            }
            else
            {
                textUserName.Visibility = Visibility.Visible;
            }
        }

        private void textFirstName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtFirstName.Focus();
        }

        private void txtFirstName_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFirstName.Text) && txtFirstName.Text.Length > 0)
            {
                textFirstName.Visibility = Visibility.Collapsed;
            }
            else
            {
                textFirstName.Visibility = Visibility.Visible;
            }
        }

        private void textSecondName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtSecondName.Focus();
        }

        private void txtSecondName_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSecondName.Text) && txtSecondName.Text.Length > 0)
            {
                textSecondName.Visibility = Visibility.Collapsed;
            }
            else
            {
                textSecondName.Visibility = Visibility.Visible;
            }
        }

        private void signUpButtonClick(object sender, RoutedEventArgs e)
        {
            User myUser = new User();
            User IfMailExists = brokerDB.SelectUserByEmail(txtEmail.Text);
            User IfUserNameExists = brokerDB.SelectUserByUserName(txtUserName.Text);
            if ((IfMailExists == null) && (IfUserNameExists == null))
            {
                myUser.isMnager = false;
                myUser.Email = txtEmail.Text;
                myUser.BirthDate = (DateTime)birthdateDatePicker.SelectedDate;
                myUser.FirstName = txtFirstName.Text;
                myUser.UserName = txtUserName.Text;
                myUser.SecondName = txtSecondName.Text;
                myUser.Password = txtPassword.Password;

                brokerDB.InsertUser(myUser);
            }

        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
