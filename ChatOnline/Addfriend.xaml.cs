using ChatOnline.Object;
using Object;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    /// Interaction logic for Addfriend.xaml
    /// </summary>
    public partial class Addfriend : Page
    {
		DataBaseProcess database = new DataBaseProcess();
		List<Account> List = new List<Account>();
		string a;
		Account Account;
		public Home ParentWindow { get; set; }
		public Addfriend(Home home,Account account)
        {
            InitializeComponent();
			DataTable AddFriendList = database.DataReader("Select UserName, Avatar from Account where UserName Not in (Select User2 from Friends where User1 = '"+account.UserName+ "' UNION Select User1 from Friends where User2 = '" + account.UserName +"') and UserName <> '"+ account.UserName+"'");
			a = TextBox_Search.Text;
			foreach (DataRow row in AddFriendList.Rows)
			{
				Account acc = new Account
				{
					UserName = row["UserName"].ToString(),
					Avatar = row["Avatar"].ToString(),
				};

				List.Add(acc);
			}
			AddFriend_ListView.ItemsSource = List;
			Account = account;
			ParentWindow = home;
		}
		private void Search_TextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			if (TextBox_Search.Text == a)
			{
				TextBox_Search.Text = "";
			}

		}

		private void Search_TextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(TextBox_Search.Text))
			{
				TextBox_Search.Text = a;
			}
		}
		private void TextBox_Search_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				string searchText = TextBox_Search.Text;
				List<Account> search = List.Where(info => info.UserName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
				AddFriend_ListView.ItemsSource = null;
				AddFriend_ListView.ItemsSource = search;

			}
		}
		private bool isButtonPressed = false;
		private void Button_Addfriends(object sender, RoutedEventArgs e)
		{
			string userName="";
			Button addFriend = new Button();
			if (sender is Button button && button.Tag != null)
			{
				userName = button.Tag.ToString();
				addFriend = (Button)sender;
			}
			if (!isButtonPressed)
			{
				isButtonPressed = true;
				database.DataChange("Insert into Friends (User1,User2) values ('" + Account.UserName + "','" + userName + "')");
				addFriend.Background = new ImageBrush(new BitmapImage(new Uri("pack://siteoforigin:,,,/Addfriendsuccess.png")));
			}
			else
			{
				isButtonPressed = false;
				database.DataChange("DELETE FROM Friends WHERE User1 = '"+ Account.UserName + "' and User2 = '" + userName + "'");
				addFriend.Background = new ImageBrush(new BitmapImage(new Uri("pack://siteoforigin:,,,/Addfriend.png")));
			}
			
		}
		private void View_Friend(object sender, RoutedEventArgs e)
		{
			Button clickedButton = (Button)sender;

			string textValue = clickedButton.Tag.ToString();
			string avatar = List.FirstOrDefault(acc => acc.UserName == textValue)?.Avatar;
			Account temp = new Account();
			temp.UserName = textValue;
			temp.Avatar = avatar;
			var a = new Profile_View(ParentWindow, temp, Account.UserName);
			ParentWindow.AddFriendFrame.Navigate(a);
			TranslateTransform transform_Addfriend = (TranslateTransform)a.RenderTransform;

			DoubleAnimation animation_signup = new DoubleAnimation();
			animation_signup.To = 0;
			animation_signup.Duration = TimeSpan.FromSeconds(0.3);

			transform_Addfriend.BeginAnimation(TranslateTransform.YProperty, animation_signup);

		}
	}
}
