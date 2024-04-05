using ChatOnline.Object;
using Object;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatOnline
{
    /// <summary>
    /// Interaction logic for SignUp_Password.xaml
    /// </summary>
    public partial class SignUp_Password : Page
    {
        DataBaseProcess database = new DataBaseProcess();
        string Username;
        private MainWindow ParentWindow { get; set; }

        public SignUp_Password(MainWindow parent, string User)
        {
            InitializeComponent();
            Username = User;
            ParentWindow = parent;
        }
        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password == "")
            {
                placeholderText.Text = "Password";
                placeholderText.Visibility = Visibility.Collapsed;
                placeholderText.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2F3180"));
                PasswordBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2F3180"));
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password == "")
            {
                placeholderText.Visibility = Visibility.Visible;
            }
        }
        private void Confirm_PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Confirm_PasswordBox.Password == "")
            {
                Confirm_placeholderText.Text = "Confirm Password";
                Confirm_placeholderText.Visibility = Visibility.Collapsed;
                Confirm_placeholderText.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2F3180"));
                Confirm_PasswordBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2F3180"));
            }
        }

        private void ConfirmPasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Confirm_PasswordBox.Password == "")
            {
                Confirm_placeholderText.Visibility = Visibility.Visible;
            }
        }
        private async void LoginButton_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {

            if (ParentWindow != null)
            {
                TranslateTransform transform_login = (TranslateTransform)this.RenderTransform;

                DoubleAnimation animation_login = new DoubleAnimation();
                animation_login.To = 800;
                animation_login.Duration = TimeSpan.FromSeconds(0.3);

                transform_login.BeginAnimation(TranslateTransform.YProperty, animation_login);

                await Task.Delay(TimeSpan.FromSeconds(0.3));
                var a = new Login(ParentWindow);
                ParentWindow.mainFrame.Navigate(a);
                TranslateTransform transform_signup = (TranslateTransform)a.RenderTransform;

                DoubleAnimation animation_signup = new DoubleAnimation();
                animation_signup.To = 0;
                animation_signup.Duration = TimeSpan.FromSeconds(0.3);

                transform_signup.BeginAnimation(TranslateTransform.YProperty, animation_signup);
            }

        }

        public async void Check_SignUp(object sender, RoutedEventArgs e)
        {
            string password;
            string Confirm_password;
            PasswordBox passwordBox = PasswordBox; 
            PasswordBox Confirm_passwordBox = Confirm_PasswordBox;

            SecureString securePassword = passwordBox.SecurePassword;
            SecureString Confirm_securePassword = Confirm_passwordBox.SecurePassword;

            IntPtr bstr = Marshal.SecureStringToBSTR(securePassword);
            IntPtr Confirm_bstr = Marshal.SecureStringToBSTR(Confirm_securePassword);
            try
            {
                password = Marshal.PtrToStringBSTR(bstr);
                Confirm_password = Marshal.PtrToStringBSTR(Confirm_bstr);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(bstr);
                Marshal.ZeroFreeBSTR(Confirm_bstr);
            }
            if(PasswordBox.Password != "" && Confirm_PasswordBox.Password !="" && PasswordBox.Password == Confirm_PasswordBox.Password)
            {
                database.DataChange("Insert into Account (UserName,Pass,Avatar) values ('" + Username + "','"+ PasswordBox.Password+ "','"+ "https://static.vecteezy.com/system/resources/thumbnails/008/442/086/small/illustration-of-human-icon-user-symbol-icon-modern-design-on-blank-background-free-vector.jpg" + "')");
                database.DataChange("Insert into Profile(Username) values('"+ Username +"')");
				if (ParentWindow != null)
                {
                    TranslateTransform transform_login = (TranslateTransform)this.RenderTransform;

                    DoubleAnimation animation_login = new DoubleAnimation();
                    animation_login.To = 800;
                    animation_login.Duration = TimeSpan.FromSeconds(0.3);

                    transform_login.BeginAnimation(TranslateTransform.YProperty, animation_login);

                    await Task.Delay(TimeSpan.FromSeconds(0.3));
                    var a = new Login(ParentWindow);
                    ParentWindow.mainFrame.Navigate(a);
                    TranslateTransform transform_signup = (TranslateTransform)a.RenderTransform;

                    DoubleAnimation animation_signup = new DoubleAnimation();
                    animation_signup.To = 0;
                    animation_signup.Duration = TimeSpan.FromSeconds(0.3);

                    transform_signup.BeginAnimation(TranslateTransform.YProperty, animation_signup);
                }
            }
            else if(PasswordBox.Password == "")
            {
                PasswordBox.Password = "";
                placeholderText.BorderBrush = Brushes.Red;
                PasswordBox.BorderBrush = Brushes.Red;
            }else if(Confirm_PasswordBox.Password == "")
            {
                Confirm_PasswordBox.Password = "";
                Confirm_placeholderText.BorderBrush = Brushes.Red;
                Confirm_PasswordBox.BorderBrush = Brushes.Red;
            }
            else
            {
                placeholderText.Visibility = Visibility.Visible;
                Confirm_placeholderText.Visibility = Visibility.Visible;
                placeholderText.Text = "Passwords are not the same";
                Confirm_placeholderText.Text = "Passwords are not the same";
                placeholderText.BorderBrush = Brushes.Red;
                Confirm_placeholderText.BorderBrush = Brushes.Red;
                PasswordBox.Password = "";
                Confirm_PasswordBox.Password = "";
            }

        }
    }
}
