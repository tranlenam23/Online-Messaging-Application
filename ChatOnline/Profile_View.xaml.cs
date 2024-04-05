using ChatOnline.Object;
using Object;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
    /// Interaction logic for Profile_View.xaml
    /// </summary>
    public partial class Profile_View : Page
    {
		DataBaseProcess database = new DataBaseProcess();
		Account acc;
		string MyName;
		public Home ParentWindow { get; set; }
		public Profile_View(Home home,Account account, string myname)
		{
			InitializeComponent();
			DataTable MyAccount = database.DataReader("Select Avatar from Account where UserName = '" + account.UserName + "'");
			DataTable MyProfile = database.DataReader("Select * from Profile where UserName ='" + account.UserName + "'");
			DataTable NumberOfFriends = database.DataReader("SELECT COUNT(*) AS NumberOfFriends From Friends Where User1 ='" + account.UserName + "' OR User2 ='" + account.UserName + "'");

			Textblock_UserName.Text = account.UserName;
			if (NumberOfFriends != null && NumberOfFriends.Rows.Count > 0)
			{
				Num_Friends.Text = NumberOfFriends.Rows[0]["NumberOfFriends"].ToString();
			}
			acc = account; MyName = myname; ParentWindow = home;
			if (MyAccount != null && MyAccount.Rows.Count > 0)
			{
				string avatar = MyAccount.Rows[0]["Avatar"].ToString();
				User_Avatar.Background = new ImageBrush(new BitmapImage(new Uri(avatar)));
			}
			if (MyProfile != null && MyProfile.Rows.Count > 0)
			{
				Gender.Text = MyProfile.Rows[0]["Gender"]?.ToString() ?? "";
				Email.Text = MyProfile.Rows[0]["Email"]?.ToString() ?? "";
				Facebook.Text = MyProfile.Rows[0]["Facebook"]?.ToString() ?? "";
				Describe.Text = MyProfile.Rows[0]["Describe"]?.ToString() ?? "";
			}
			DataTable FriendList = database.DataReader("SELECT Account.UserName As UserName from Account inner join Friends on (Account.UserName = Friends.User1 and Friends.User2 = '" + myname + "') or (Account.UserName = Friends.User2 and Friends.User1 = '" + myname + "') Where Friends.User1 ='" + myname + "' OR Friends.User2 ='" + myname + "'");
			bool isinList = FriendList.AsEnumerable().Any(row => row.Field<string>("UserName") == account.UserName);

			if (isinList)
			{
				AddFriend_button.Content = "UnFriend";
			}
			else
			{
				AddFriend_button.Content = "Add Friend";
			}
		}

		private void OpenFacebook_Click(object sender, MouseButtonEventArgs e)
		{
			if (Facebook.Text.Length > 0)
			{
				string url = "https://www.facebook.com/" + Facebook.Text;

				// Mở trình duyệt mặc định và điều hướng đến trang web
				Process.Start(new ProcessStartInfo
				{
					FileName = url,
					UseShellExecute = true
				});

			}
		}
		private void Open_ListFriend(object sender, MouseButtonEventArgs e)
		{

			var a = new ListFriend(ParentWindow, acc,MyName);
			AddFriendFrame.Navigate(a);
			TranslateTransform transform_Addfriend = (TranslateTransform)a.RenderTransform;

			DoubleAnimation animation_signup = new DoubleAnimation();
			animation_signup.To = 0;
			animation_signup.Duration = TimeSpan.FromSeconds(0.3);

			transform_Addfriend.BeginAnimation(TranslateTransform.YProperty, animation_signup);
		}
		private void AddFriend(object sender, RoutedEventArgs e)
		{
			if(AddFriend_button.Content == "Add Friend")
			{
				database.DataChange("Insert into Friends (User1,User2) values ('" + MyName + "','" + acc.UserName + "')");
				AddFriend_button.Content = "UnFriend";
			}else if(AddFriend_button.Content == "UnFriend")
			{
				try
				{
					database.DataChange("DELETE FROM Friends WHERE User1 = '" + MyName + "' and User2 = '" + acc.UserName + "'");

				}
				catch (Exception ex)
				{
				}
				try
				{
					database.DataChange("DELETE FROM Friends WHERE User1 = '" + acc.UserName + "' and User2 = '" + MyName + "'");
				}
				catch (Exception ex)
				{
				}
				AddFriend_button.Content = "Add Friend";
			}
		}
		private void Close(object sender, RoutedEventArgs e)
		{
			TranslateTransform transform_login = (TranslateTransform)this.RenderTransform;

			DoubleAnimation animation_login = new DoubleAnimation();
			animation_login.To = 800;
			animation_login.Duration = TimeSpan.FromSeconds(0.3);

			transform_login.BeginAnimation(TranslateTransform.YProperty, animation_login);
			ParentWindow.Call_List_Friends();
		}
	}
}
