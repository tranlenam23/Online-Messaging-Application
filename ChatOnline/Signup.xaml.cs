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
    /// Interaction logic for Signup.xaml
    /// </summary>
    /// 
    public partial class Signup : Page
    {
        DataBaseProcess database = new DataBaseProcess();

        public Signup()
        {
            InitializeComponent();
        }
        private MainWindow ParentWindow { get; set; }
        public Signup(MainWindow parent)
        {
            InitializeComponent();
            ParentWindow = parent;
        }
        private void UsernameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (UserName.Text == "Username" || UserName.Text == "Username is already taken")
            {
                UserName.Text = "";
            }
        }

        private void UsernameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserName.Text))
            {
                UserName.Text = "Username";
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
        public async void Check_Signup(object sender, RoutedEventArgs e)
        {
            

            DataTable Accounts = database.DataReader("Select UserName from Account where UserName = '" + UserName.Text +"'");
            Account account = new Account();
            if (UserName.Text == "" || UserName.Text == "Username" || UserName.Text == "Username is already taken")
            {
                UserName.BorderBrush = Brushes.Red;
            }
            else if (Accounts.Rows.Count > 0)
            {
                UserName.Text = "Username is already taken";
                UserName.BorderBrush = Brushes.Red;
            }
            else
            {
                if (ParentWindow != null)
                {
                    TranslateTransform transform_login = (TranslateTransform)this.RenderTransform;

                    DoubleAnimation animation_login = new DoubleAnimation();
                    animation_login.To = 800;
                    animation_login.Duration = TimeSpan.FromSeconds(0.3);

                    transform_login.BeginAnimation(TranslateTransform.YProperty, animation_login);

                    await Task.Delay(TimeSpan.FromSeconds(0.3));
                    var a = new SignUp_Password(ParentWindow, UserName.Text);
                    ParentWindow.mainFrame.Navigate(a);
                    TranslateTransform transform_signup = (TranslateTransform)a.RenderTransform;

                    DoubleAnimation animation_signup = new DoubleAnimation();
                    animation_signup.To = 0;
                    animation_signup.Duration = TimeSpan.FromSeconds(0.3);

                    transform_signup.BeginAnimation(TranslateTransform.YProperty, animation_signup);
                }
            }


        }

    }
}
