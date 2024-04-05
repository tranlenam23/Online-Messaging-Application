using ChatOnline.Object;
using Microsoft.Win32;
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
    /// Interaction logic for Group_View.xaml
    /// </summary>
    public partial class Group_View : Page
    {
		DataBaseProcess database = new DataBaseProcess();
		GroupChat Groupchat = new GroupChat();
		string UserName;
		public Home ParentWindow { get; set; }
		public Group_View(Home Paren, GroupChat group, string Username)
		{
            InitializeComponent();
			DataTable Group = database.DataReader("SELECT COUNT(*) AS TotalAccounts FROM Account_Group WHERE GroupID = " + group.GroupID);
			Groupchat = group; UserName = Username; ParentWindow = Paren;
			if (Group != null && Group.Rows.Count > 0)
			{
				Num_Member.Text = Group.Rows[0]["TotalAccounts"].ToString();
			}
			User_Avatar.Background = new ImageBrush(new BitmapImage(new Uri(group.Avatar)));
			Textblock_UserName.Text = group.GroupName;
		}
		private void Addmember(object sender, RoutedEventArgs e)
		{

			var a = new AddMembers(ParentWindow, Groupchat,UserName);
			ParentWindow.AddFriendFrame.Navigate(a);
			TranslateTransform transform_Addfriend = (TranslateTransform)a.RenderTransform;

			DoubleAnimation animation_signup = new DoubleAnimation();
			animation_signup.To = 0;
			animation_signup.Duration = TimeSpan.FromSeconds(0.3);

			transform_Addfriend.BeginAnimation(TranslateTransform.YProperty, animation_signup);
		}
		private void Open_ListMembers(object sender, MouseButtonEventArgs e)
		{

			var a = new List_Members(ParentWindow, Groupchat, UserName);
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
		private async void Leave_Group(object sender, MouseButtonEventArgs e)
		{
			database.DataChange("DELETE FROM Account_Group WHERE UserName = '" + UserName + "' and GroupID =" + Groupchat.GroupID);
			var a = new List_Members(ParentWindow, Groupchat, UserName);
			ParentWindow.AddFriendFrame.Navigate(a);
			TranslateTransform transform_Addfriend = (TranslateTransform)a.RenderTransform;

			DoubleAnimation animation_signup = new DoubleAnimation();
			animation_signup.To = 800;
			animation_signup.Duration = TimeSpan.FromSeconds(0.3);

			transform_Addfriend.BeginAnimation(TranslateTransform.YProperty, animation_signup);
			ParentWindow.BoxChatFrame.Navigate(null);
			await Task.Delay(TimeSpan.FromSeconds(0.3));
			ParentWindow.AddFriendFrame.Navigate(null);
			ParentWindow.Call_List_Friends();
		}
		private void OpenFileDialogButton_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Hình ảnh và Video|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.mp4;*.avi;*.mkv;*.mov|All Files|*.*";

			if (openFileDialog.ShowDialog() == true)
			{
				string selectedFilePath = openFileDialog.FileName;
				User_Avatar.Background = new ImageBrush(new BitmapImage(new Uri(selectedFilePath)));
				database.DataChange("Update GroupChat set Avatar = '" + selectedFilePath + "' where GroupID =" + Groupchat.GroupID );
				ParentWindow.Call_List_Friends();
			}
		}
	}
}
