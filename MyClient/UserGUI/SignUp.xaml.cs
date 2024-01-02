using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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
        private bool under18;

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
            bool Errr = false;
            User myUser = new User();
            User IfMailExists = brokerDB.SelectUserByEmail(txtEmail.Text);
            User IfUserNameExists = brokerDB.SelectUserByUserName(txtUserName.Text);
            EmailErr.Visibility = Visibility.Collapsed;
            UserNameErr.Visibility = Visibility.Collapsed;
            FirstNameErr.Visibility = Visibility.Collapsed;
            SecNameErr.Visibility = Visibility.Collapsed;
            DateErr.Visibility = Visibility.Collapsed;
            PassErr.Visibility = Visibility.Collapsed;
            PassVerErr.Visibility = Visibility.Collapsed;
            
            if (under18 == false)
            {
                DateErr.Visibility = Visibility.Visible;
                Errr = true;
            }
            if(!txtEmail.Text.Contains("@gmail.com"))
            {
                EmailErr.Visibility = Visibility.Visible;
                Errr = true;
            }
            if(txtPassword.Password != txtVerifyPassword.Password)
            {
                PassVerErr.Visibility= Visibility.Visible;
                Errr = true;
            }
            if(IfMailExists != null)
            {
                EmailErr.Visibility= Visibility.Visible;
                Errr = true;
            }
            if(IfUserNameExists != null)
            {
                UserNameErr.Visibility= Visibility.Visible;
                Errr = true;
            }
            if (string.IsNullOrEmpty(txtFirstName.Text))
            {
                FirstNameErr.Visibility = Visibility.Visible;
                Errr = true;
            }
            if (string.IsNullOrEmpty(txtSecondName.Text))
            {
                SecNameErr.Visibility = Visibility.Visible;
                Errr = true;
            }
            if (!(txtUserName.Text.Length >= 5))
            {
                UserNameErr.Visibility = Visibility.Visible;
                Errr = true;
            }

            if (!Regex.IsMatch(txtPassword.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
            {
                PassErr.Visibility = Visibility.Visible;
                Errr = true;
            }
            if (!Errr)
            {
                myUser.isMnager = false;
                myUser.Email = txtEmail.Text;
                myUser.BirthDate = (DateTime)birthdateDatePicker.SelectedDate;
                myUser.FirstName = txtFirstName.Text;
                myUser.UserName = txtUserName.Text;
                myUser.SecondName = txtSecondName.Text;
                myUser.Password = txtPassword.Password;

                brokerDB.InsertUser(myUser);

                SuccesfullLogedIn suc = new SuccesfullLogedIn(myUser);
                this.Close();
                suc.ShowDialog();
            }

        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void txtVerifyPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtVerifyPassword.Password) && txtVerifyPassword.Password.Length > 0)
            {
                textVerifyPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                textVerifyPassword.Visibility = Visibility.Visible;
            }
        }

        private void textVerifyPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtVerifyPassword.Focus();
        }

        private void Image_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            Login log = new Login();
            this.Close();
            log.ShowDialog();
        }

        private void birthdateDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
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
                    under18 = true;
                }
                else
                {
                    // Person is under 18
                    under18 = false;
                }
            }
        }
    }
}
