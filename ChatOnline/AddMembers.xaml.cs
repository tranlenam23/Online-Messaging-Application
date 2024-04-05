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
using System.Xml.Linq;

namespace ChatOnline
{
	/// <summary>
	/// Interaction logic for AddMembers.xaml
	/// </summary>
	public partial class AddMembers : Page
	{
		DataBaseProcess database = new DataBaseProcess();
		List<Account> List = new List<Account>();
		string a;
		GroupChat Account;
		string User;
		public Home ParentWindow { get; set; }
		public AddMembers(Home Paren, GroupChat group, string Username)
		{
			InitializeComponent();

			DataTable AddFriendList = database.DataReader("SELECT myFriends.UserName As UserName, myFriends.Avatar As Avatar from (" + "SELECT Account.UserName As UserName, Account.Avatar As Avatar from Account inner join Friends on (Account.UserName = Friends.User1 and Friends.User2 = '" + Username + "') or (Account.UserName = Friends.User2 and Friends.User1 = '" + Username + "') Where Friends.User1 ='" + Username + "' OR Friends.User2 ='" + Username + "') as myFriends Where myFriends.UserName not in ( select UserName from Account_Group where GroupID= "+group.GroupID +")");
			a = TextBox_Search.Text;
			User = Username; ParentWindow = Paren;
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
			Account = group;
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
			string userName = "";
			Button addFriend = new Button();
			if (sender is Button button && button.Tag != null)
			{
				userName = button.Tag.ToString();
				addFriend = (Button)sender;
			}
			if (!isButtonPressed)
			{
				isButtonPressed = true;
				database.DataChange("Insert into Account_Group (UserName,GroupID) values ('" + userName + "'," + Account.GroupID + ")");
				addFriend.Background = new ImageBrush(new BitmapImage(new Uri("pack://siteoforigin:,,,/Addfriendsuccess.png")));
			}
			else
			{
				isButtonPressed = false;
				database.DataChange("DELETE FROM Account_Group WHERE UserName = '" + userName + "' and GroupID = '" + Account.GroupID + "'");
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
			var a = new Profile_View(ParentWindow, temp, User);
			ParentWindow.AddFriendFrame.Navigate(a);
			TranslateTransform transform_Addfriend = (TranslateTransform)a.RenderTransform;

				DoubleAnimation animation_signup = new DoubleAnimation();
				animation_signup.To = 0;
				animation_signup.Duration = TimeSpan.FromSeconds(0.3);

				transform_Addfriend.BeginAnimation(TranslateTransform.YProperty, animation_signup);
			
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
