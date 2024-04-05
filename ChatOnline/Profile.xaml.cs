using ChatOnline.Object;
using Microsoft.Win32;
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
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
		DataBaseProcess database = new DataBaseProcess();
		Account acc;
		private string newAvatar = "";
		public Home ParentWindow { get; set; }
		public Profile(Home home,Account account)
        {
            InitializeComponent();
			DataTable MyAccount = database.DataReader("Select Avatar from Account where UserName = '" + account.UserName + "'");
			DataTable MyProfile = database.DataReader("Select * from Profile where UserName ='" + account.UserName + "'");
			DataTable NumberOfFriends = database.DataReader("SELECT COUNT(*) AS NumberOfFriends From Friends Where User1 ='" + account.UserName + "' OR User2 ='" + account.UserName +"'");
			DataTable Sent = database.DataReader("Select COUNT(*) AS NumberOfFSent from ChatData where SendUser ='" + account.UserName + "'");
			ParentWindow = home;
			Textblock_UserName.Text = account.UserName;
			if (NumberOfFriends != null && NumberOfFriends.Rows.Count > 0)
			{
				Num_Friends.Text = NumberOfFriends.Rows[0]["NumberOfFriends"].ToString();
			}
			if (Sent != null && Sent.Rows.Count > 0)
			{
				Num_Messages.Text = Sent.Rows[0]["NumberOfFSent"].ToString();
			}
			acc = account;
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
		}

		private void Gender_enable(object sender, MouseButtonEventArgs e)
		{
			Gender.IsReadOnly = false;
			Gender.Focus();
		}
		private void Email_enable(object sender, MouseButtonEventArgs e)
		{
			Email.IsReadOnly = false;
			Email.Focus();
		}
		private void Facebook_enable(object sender, MouseButtonEventArgs e)
		{
			Facebook.IsReadOnly = false;
			Facebook.Focus();
		}
		private void Describe_enable(object sender, MouseButtonEventArgs e)
		{
			Describe.IsReadOnly = false;
			Describe.Focus();
		}
		private void IsreadOnly(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				// Đặt IsReadOnly thành true
				if (sender is TextBox textBox)
				{
					textBox.IsReadOnly = true;
					if(textBox.Name == "Gender")
					{
						database.DataChange("Update Profile set Gender = '" + textBox.Text + "' where UserName ='" + acc.UserName +"'");
					}else if(textBox.Name == "Email")
					{
						database.DataChange("Update Profile set Email = '" + textBox.Text + "' where UserName ='" + acc.UserName + "'");
					}
					else if (textBox.Name == "Facebook")
					{
						database.DataChange("Update Profile set Facebook = '" + textBox.Text + "' where UserName ='" + acc.UserName + "'");
					}
					else if (textBox.Name == "Describe")
					{
						database.DataChange("Update Profile set Describe = '" + textBox.Text + "' where UserName ='" + acc.UserName + "'");
					}
				}
			}
		}
		private void IsreadOnly_LostFocus(object sender, RoutedEventArgs e)
		{
			
				if (sender is TextBox textBox)
				{
					textBox.IsReadOnly = true;
					if(textBox.Name == "Gender")
					{
						database.DataChange("Update Profile set Gender = '" + textBox.Text + "' where UserName ='" + acc.UserName + "'");
					}else if(textBox.Name == "Email")
					{
						database.DataChange("Update Profile set Email = '" + textBox.Text + "' where UserName ='" + acc.UserName + "'");
					}
					else if (textBox.Name == "Facebook")
					{
						database.DataChange("Update Profile set Facebook = '" + textBox.Text + "' where UserName ='" + acc.UserName + "'");
					}
					else if (textBox.Name == "Describe")
					{
						database.DataChange("Update Profile set Describe = '" + textBox.Text + "' where UserName ='" + acc.UserName + "'");
					}
				}
		}
		private void OpenFileDialogButton_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Hình ảnh và Video|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.mp4;*.avi;*.mkv;*.mov|All Files|*.*";

			if (openFileDialog.ShowDialog() == true)
			{
				string selectedFilePath = openFileDialog.FileName;
				User_Avatar.Background = new ImageBrush(new BitmapImage(new Uri(selectedFilePath)));
				database.DataChange("Update Account set Avatar = '" + selectedFilePath + "' where UserName ='" + acc.UserName + "'");
				ParentWindow.User_Avatar.Background = new ImageBrush(new BitmapImage(new Uri(selectedFilePath)));
			}
		}
		private void OpenFacebook_Click(object sender, MouseButtonEventArgs e)
		{
			if(Facebook.Text.Length > 0)
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

			var a = new ListFriend(ParentWindow, acc, acc.UserName);
			AddFriendFrame.Navigate(a);
			TranslateTransform transform_Addfriend = (TranslateTransform)a.RenderTransform;

			DoubleAnimation animation_signup = new DoubleAnimation();
			animation_signup.To = 0;
			animation_signup.Duration = TimeSpan.FromSeconds(0.3);

			transform_Addfriend.BeginAnimation(TranslateTransform.YProperty, animation_signup);
		}
	}
}
