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
	/// Interaction logic for List_Members.xaml
	/// </summary>
	public partial class List_Members : Page
	{
		DataBaseProcess database = new DataBaseProcess();
		List<Account> List = new List<Account>();
		string a;
		string MyName;
		public Home ParentWindow { get; set; }
		public List_Members(Home Paren, GroupChat groupchat, string myname)
		{
			InitializeComponent();
			DataTable FriendList = database.DataReader("SELECT A.UserName as UserName, A.Avatar as Avatar FROM Account A JOIN Account_Group AG ON A.UserName = AG.UserName WHERE AG.GroupID = " + groupchat.GroupID);
			a = TextBox_Search.Text;
			ParentWindow = Paren;
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
			MyName = myname;
		}
		private void View_Friend(object sender, RoutedEventArgs e)
		{
			Button clickedButton = (Button)sender;

			string textValue = clickedButton.Tag.ToString();
			string avatar = List.FirstOrDefault(acc => acc.UserName == textValue)?.Avatar;
			Account temp = new Account();
			temp.UserName = textValue;
			temp.Avatar = avatar;
			var a = new Profile_View(ParentWindow, temp, MyName);
			var b = new Profile(ParentWindow,temp);
			if (textValue == MyName)
			{
				ParentWindow.AddFriendFrame.Navigate(b);
				TranslateTransform transform_Addfriend = (TranslateTransform)b.RenderTransform;

				DoubleAnimation animation_signup = new DoubleAnimation();
				animation_signup.To = 0;
				animation_signup.Duration = TimeSpan.FromSeconds(0.3);

				transform_Addfriend.BeginAnimation(TranslateTransform.YProperty, animation_signup);
			}
			else
			{
				ParentWindow.AddFriendFrame.Navigate(a);
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
