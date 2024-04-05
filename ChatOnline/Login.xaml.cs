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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public MainWindow ParentWindow { get; set; }

        DataBaseProcess database = new DataBaseProcess();
        public Login(MainWindow parent)
        {
            InitializeComponent();
            ParentWindow = parent;
            
        }
        private void UsernameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (UserName.Text == "Username" || UserName.Text == "Username or Password incorrect")
            {
                UserName.Text = "";
                UserName.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2F3180"));
            }
        }

        private void UsernameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserName.Text))
            {
                UserName.Text = "Username";
            }
        }
        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password == "")
            {
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
         private async void SignupButton_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {

            if (ParentWindow != null)
            {
                TranslateTransform transform_login = (TranslateTransform)this.RenderTransform;

                DoubleAnimation animation_login = new DoubleAnimation();
                animation_login.To = 800;
                animation_login.Duration = TimeSpan.FromSeconds(0.3);

                transform_login.BeginAnimation(TranslateTransform.YProperty, animation_login);

                await Task.Delay(TimeSpan.FromSeconds(0.3));
                var a = new Signup(ParentWindow);
                ParentWindow.mainFrame.Navigate(a);
                TranslateTransform transform_signup = (TranslateTransform)a.RenderTransform;

                DoubleAnimation animation_signup = new DoubleAnimation();
                animation_signup.To = 0;
                animation_signup.Duration = TimeSpan.FromSeconds(0.3);

                transform_signup.BeginAnimation(TranslateTransform.YProperty, animation_signup);

            }

        }
        public void Check_Login(object sender, RoutedEventArgs e)
        {
            string password;
            PasswordBox passwordBox = PasswordBox; // Thay yourPasswordBox bằng tên của PasswordBox trong ứng dụng của bạn

            SecureString securePassword = passwordBox.SecurePassword;

            IntPtr bstr = Marshal.SecureStringToBSTR(securePassword);
            try
            {
                password = Marshal.PtrToStringBSTR(bstr);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(bstr);
            }

            DataTable Accounts = database.DataReader("Select UserName, Pass, Avatar from Account where UserName = '" + UserName.Text + "' and Pass = '" + password +"'");
            Account account = new Account();
            if(UserName.Text == "" || UserName.Text == "Username")
            {
                UserName.BorderBrush = Brushes.Red;
            }else if(PasswordBox.Password == "")
            {
                placeholderText.BorderBrush = Brushes.Red;
                PasswordBox.BorderBrush = Brushes.Red;
            }else if (Accounts.Rows.Count > 0)
            {
                DataRow row = Accounts.Rows[0];

                account.UserName = row["UserName"].ToString();
                account.Pass = row["Pass"].ToString();
                account.Avatar = row["Avatar"].ToString();

                var a = new Home(ParentWindow,account);
                ParentWindow.mainFrame.Navigate(a);
            }
            else
            {
                UserName.Text = "Username or Password incorrect";
                
                PasswordBox.Password = "";
                UserName.BorderBrush = Brushes.Red;
                placeholderText.BorderBrush = Brushes.Red;
                PasswordBox.BorderBrush = Brushes.Red;
            }


        }


    }
}
