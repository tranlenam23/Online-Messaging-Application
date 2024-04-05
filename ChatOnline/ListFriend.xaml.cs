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
    /// Interaction logic for ListFriend.xaml
    /// </summary>
    public partial class ListFriend : Page
    {
		DataBaseProcess database = new DataBaseProcess();
		List<Account> List = new List<Account>();
		string a;
		Account Account;
		string MyName;
		public Home ParentWindow { get; set; }
		public ListFriend(Home home, Account account, string myname)
		{
			InitializeComponent();
			DataTable FriendList = database.DataReader("SELECT Account.UserName As UserName, Account.Avatar As Avatar from Account inner join Friends on (Account.UserName = Friends.User1 and Friends.User2 = '" + account.UserName + "') or (Account.UserName = Friends.User2 and Friends.User1 = '" + account.UserName + "') Where Friends.User1 ='" + account.UserName + "' OR Friends.User2 ='" + account.UserName + "'");
			a = TextBox_Search.Text;
			foreach (DataRow row in FriendList.Rows)
			{
				Account acc = new Account
				{
					UserName = row["UserName"].ToString(),
					Avatar = row["Avatar"].ToString(),
				};

				List.Add(acc);
			}
			Friend_ListView.ItemsSource = List;
			Account = account;
			MyName = myname;
			ParentWindow = home;
		}
		private void View_Friend(object sender, RoutedEventArgs e)
		{
			Button clickedButton = (Button)sender;

			string textValue = clickedButton.Tag.ToString();
			string avatar= List.FirstOrDefault(acc => acc.UserName == textValue)?.Avatar;
			Account temp = new Account();
			temp.UserName = textValue;
			temp.Avatar = avatar;
			var a = new Profile_View(ParentWindow, temp, MyName);
			var b = new Profile(ParentWindow,temp);
			if (textValue == MyName)
			{
				ViewFriendFrame.Navigate(b);
				TranslateTransform transform_Addfriend = (TranslateTransform)b.RenderTransform;

				DoubleAnimation animation_signup = new DoubleAnimation();
				animation_signup.To = 0;
				animation_signup.Duration = TimeSpan.FromSeconds(0.3);

				transform_Addfriend.BeginAnimation(TranslateTransform.YProperty, animation_signup);
			}
			else
			{
				ViewFriendFrame.Navigate(a);
				TranslateTransform transform_Addfriend = (TranslateTransform)a.RenderTransform;

				DoubleAnimation animation_signup = new DoubleAnimation();
				animation_signup.To = 0;
				animation_signup.Duration = TimeSpan.FromSeconds(0.3);

				transform_Addfriend.BeginAnimation(TranslateTransform.YProperty, animation_signup);
			}
			
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
				Friend_ListView.ItemsSource = null;
				Friend_ListView.ItemsSource = search;

			}
		}
		private void Close(object sender, RoutedEventArgs e)
		{
			TranslateTransform transform_login = (TranslateTransform)this.RenderTransform;

			DoubleAnimation animation_login = new DoubleAnimation();
			animation_login.To = 800;
			animation_login.Duration = TimeSpan.FromSeconds(0.3);

			transform_login.BeginAnimation(TranslateTransform.YProperty, animation_login);
			
		}
	}
}
