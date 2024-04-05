using ChatOnline.Object;
using Microsoft.AspNetCore.SignalR.Client;
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
using Wpf.Ui.Controls;

namespace ChatOnline
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public MainWindow ParentWindow { get; set; }
        DataBaseProcess database = new DataBaseProcess();
        Account account;
        string a;
        List<Information> boxchat = new List<Information>();
        HubConnection connection = new HubConnectionBuilder().WithUrl("http://localhost:5000/chat").Build();
        public Home(MainWindow parent, Account Account)
        {
            InitializeComponent();
            ParentWindow = parent;
            account = Account;
            if(account.Avatar != "")
            {
            Uri imageUri = new Uri(account.Avatar);
            BitmapImage image = new BitmapImage(imageUri);
            ImageBrush imageBrush = User_Avatar.Background as ImageBrush;
            imageBrush.ImageSource = image;
            }
            Mediator.Subscribe(account.UserName, Call_List_Friends);
            connection.StartAsync();
            connection.On<string, string, string>(account.UserName, (username, datatype, content) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Call_List_Friends();
                });
            });
			database.DataChange("Update Account set Online = 'LightGreen' where UserName ='" + Account.UserName + "'");
			DataTable Friends = database.DataReader("SELECT UserName FROM GetChatDataWithLatestContent('" + Account.UserName + "') ORDER BY LastTime DESC;");
			foreach (DataRow row in Friends.Rows)
			{
				
				connection.On(row["UserName"].ToString(), () =>
				{
					Application.Current.Dispatcher.Invoke(() =>
					{
						foreach (var border in this.FindVisualChildren<Border>(this))
						{
							if ((string)border.Tag == row["UserName"].ToString())
							{
								border.BorderBrush = Brushes.LightGreen;
							}
						}
                        Call_List_Friends();
						connection.InvokeAsync("Online1", account.UserName);
					});
				});
				connection.On(row["UserName"].ToString() + "onl", () =>
				{
					Application.Current.Dispatcher.Invoke(() =>
					{
						foreach (var border in this.FindVisualChildren<Border>(this))
						{
							if ((string)border.Tag == row["UserName"].ToString())
							{
								border.BorderBrush = Brushes.LightGreen;
							}
						}
					});
				});
				connection.On(row["UserName"].ToString() + "off", () =>
				{
					Application.Current.Dispatcher.Invoke(() =>
					{
						foreach (var border in this.FindVisualChildren<Border>(this))
						{
							if ((string)border.Tag == row["UserName"].ToString())
							{
								border.BorderBrush = Brushes.Transparent;
							}
						}
					});
				});
			}

			connection.InvokeAsync("Online", account.UserName);

			a = TextBox_Search.Text;
			Loaded += Home_Loaded;
		}
        public void Call_List_Friends()
        {
            boxchat.Clear();
            Chats_ListView.ItemsSource = null;
            foreach (var radioButton in Menu.Children.OfType<RadioButton>().Where(rb => rb.GroupName == "MyGroup"))
            {
                if(radioButton.Name == "Friend_button" && radioButton.IsChecked == true)
                {
                    DataTable Friends = database.DataReader("SELECT * FROM GetChatDataWithLatestContent('" + account.UserName + "') ORDER BY LastTime DESC;");

                    foreach (DataRow row in Friends.Rows)
                    {
                        Information IF = new Information
                        {

                            UserName = row["UserName"].ToString(),
                            Avatar = row["Avatar"].ToString(),
                            IDChatBox = (int)row["FriendsID"],
                            Datatype = row["Datatype"].ToString(),
							TypeBoxChat = 0,
							Bordercolor = row["Online"].ToString()
						};
                        string latestSendTime = row["LatestSendTime"].ToString();

                        if (IF.Datatype =="Text" && latestSendTime.Length > 12)
                        {
                            IF.NewMessage = latestSendTime.Substring(0, 12) + "...";
                        }
                        else if(IF.Datatype == "Text" || IF.Datatype =="Icon")
                        {
                            IF.NewMessage = latestSendTime;
                        }
						else if (IF.Datatype == "Image")
						{
							IF.NewMessage = "Image";
						}
						else
						{
							IF.NewMessage = "Start now";
						}
						if (row["LastTime"] != DBNull.Value)
                        {
                            DateTime temp = (DateTime)row["LastTime"];
                            if (temp.Date == DateTime.Today)
                            {
                                IF.LastTime = temp.ToString("HH:mm");
                            }
                            else
                            {
                                IF.LastTime = temp.ToString("dd/MM");
                            }
                        }
                        else
                        {
                            IF.LastTime = "";
                        }
                        if (row["Flag"] == DBNull.Value || (int)row["Flag"] == 1)
                        {
                            IF.Font = "Normal";
                        }
                        else if ((int)row["Flag"] == 0 && row["SendUser"].ToString() != account.UserName)
                        {
                            IF.Font = "Bold";
                        }
                        else
                        {
                            IF.Font = "Normal";
                        }
                        boxchat.Add(IF);
                    }
                }else if(radioButton.Name == "Group_button" && radioButton.IsChecked == true)
                {
                    DataTable Groups = database.DataReader("select * from GetinformationGroup('" + account.UserName + "') ORDER BY LastTime DESC;");

                    foreach (DataRow row in Groups.Rows)
                    {
                        Information IF = new Information
                        {

                            UserName = row["GroupName"].ToString(),
                            Avatar = row["Avatar"].ToString(),
                            IDChatBox = (int)row["GroupID"],
							Datatype = row["Datatype"].ToString(),
							Font = "Normal",
                            TypeBoxChat = 1
                        };
                        string latestSendTime = row["LatestSendTime"].ToString();

						if (IF.Datatype == "Text" && latestSendTime.Length > 12)
						{
							IF.NewMessage = latestSendTime.Substring(0, 12) + "...";
						}
						else if (IF.Datatype == "Text" || IF.Datatype == "Icon")
						{
							IF.NewMessage = latestSendTime;
						}
						else if (IF.Datatype == "Image")
						{
							IF.NewMessage = "Image";
						}
						else
						{
							IF.NewMessage = "Start now";
						}
						if (row["LastTime"] != DBNull.Value)
                        {
                            DateTime temp = (DateTime)row["LastTime"];
                            if (temp.Date == DateTime.Today)
                            {
                                IF.LastTime = temp.ToString("HH:mm");
                            }
                            else
                            {
                                IF.LastTime = temp.ToString("dd/MM");
                            }
                        }
                        else
                        {
                            IF.LastTime = "";
                        }

                        boxchat.Add(IF);
                    }
                }
            }
            
            Chats_ListView.ItemsSource = boxchat;
        }
        private void RadioButton_Friends(object sender, RoutedEventArgs e)
        {
			foreach (var radioButton in Menu.Children.OfType<RadioButton>().Where(rb => rb.GroupName == "MyGroup"))
			{
                radioButton.BorderThickness = new Thickness(0);
			}
			AddFriendFrame.NavigationService.Navigate(null);
			RadioButton checkedRadioButton = sender as RadioButton;
            checkedRadioButton.IsChecked = true;
            checkedRadioButton.BorderThickness = new Thickness(0,0,0,2);
            boxchat.Clear();
            Chats_ListView.ItemsSource = null;
            if (checkedRadioButton != null && checkedRadioButton.IsChecked == true)
            {
                // Xử lý khi RadioButton được chọn
                DataTable Friends = database.DataReader("SELECT * FROM GetChatDataWithLatestContent('" + account.UserName + "') ORDER BY LastTime DESC;");

                foreach (DataRow row in Friends.Rows)
                {
                    Information IF = new Information
                    {

                        UserName = row["UserName"].ToString(),
                        Avatar = row["Avatar"].ToString(),
                        IDChatBox = (int)row["FriendsID"],
						Datatype = row["Datatype"].ToString(),
						TypeBoxChat = 0,
						Bordercolor = row["Online"].ToString()
					};
                    string latestSendTime = row["LatestSendTime"].ToString();

					if (IF.Datatype == "Text" && latestSendTime.Length > 12)
					{
						IF.NewMessage = latestSendTime.Substring(0, 12) + "...";
					}
					else if (IF.Datatype == "Text" || IF.Datatype == "Icon")
					{
						IF.NewMessage = latestSendTime;
					}
					else if (IF.Datatype == "Image")
					{
						IF.NewMessage = "Image";
					}
					else
					{
						IF.NewMessage = "Start now";
					}
					if (row["LastTime"] != DBNull.Value)
                    {
                        DateTime temp = (DateTime)row["LastTime"];
                        if (temp.Date == DateTime.Today)
                        {
                            IF.LastTime = temp.ToString("HH:mm");
                        }
                        else
                        {
                            IF.LastTime = temp.ToString("dd/MM");
                        }
                    }
                    else
                    {
                        IF.LastTime = "";
                    }

                    if (row["Flag"] == DBNull.Value || (int)row["Flag"] == 1)
                    {
                        IF.Font = "Normal";
                    }
                    else if((int)row["Flag"] == 0 && row["SendUser"].ToString() != account.UserName)
                    {
                        IF.Font = "Bold";
                    }
                    else
                    {
                        IF.Font = "Normal";
                    }
                    boxchat.Add(IF);
                }
                Chats_ListView.ItemsSource = boxchat;
                
            }
			
		}
        private void RadioButton_Groups(object sender, RoutedEventArgs e)
        {
            foreach (var radioButton in Menu.Children.OfType<RadioButton>().Where(rb => rb.GroupName == "MyGroup"))
            {
                radioButton.BorderThickness = new Thickness(0);
            }
			AddFriendFrame.NavigationService.Navigate(null);
			RadioButton checkedRadioButton = sender as RadioButton;
            checkedRadioButton.IsChecked = true;
            checkedRadioButton.BorderThickness = new Thickness(0, 0, 0, 2);
            boxchat.Clear();
            Chats_ListView.ItemsSource = null;
            DataTable Groups = database.DataReader("select * from GetinformationGroup('" + account.UserName + "') ORDER BY LastTime DESC;");

            foreach (DataRow row in Groups.Rows)
            {
                Information IF = new Information
                {

                    UserName = row["GroupName"].ToString(),
                    Avatar = row["Avatar"].ToString(),
                    IDChatBox = (int)row["GroupID"],
					Datatype = row["Datatype"].ToString(),
					Font = "Normal",
                    TypeBoxChat = 1
                };
                string latestSendTime = row["LatestSendTime"].ToString();

				if (IF.Datatype == "Text" && latestSendTime.Length > 12)
				{
					IF.NewMessage = latestSendTime.Substring(0, 12) + "...";
				}
				else if (IF.Datatype == "Text" || IF.Datatype == "Icon")
				{
					IF.NewMessage = latestSendTime;
				}
				else if(IF.Datatype == "Image")
				{
					IF.NewMessage = "Image";
				}else
                {
                    IF.NewMessage = "Start now";
                }
				if (row["LastTime"] != DBNull.Value)
                {
                    DateTime temp = (DateTime)row["LastTime"];
                    if (temp.Date == DateTime.Today)
                    {
                        IF.LastTime = temp.ToString("HH:mm");
                    }
                    else
                    {
                        IF.LastTime = temp.ToString("dd/MM");
                    }
                }
                else
                {
                    IF.LastTime = "";
                }

                boxchat.Add(IF);
            }
            Chats_ListView.ItemsSource = boxchat;
            
        }
		private void RadioButton_Addfriends(object sender, RoutedEventArgs e)
        {

			foreach (var radioButton in Menu.Children.OfType<RadioButton>().Where(rb => rb.GroupName == "MyGroup"))
			{
				radioButton.BorderThickness = new Thickness(0);
			}
			RadioButton checkedRadioButton = sender as RadioButton;
			checkedRadioButton.IsChecked = true;
			checkedRadioButton.BorderThickness = new Thickness(0, 0, 0, 2);

			var a = new Addfriend(this,account);
			AddFriendFrame.Navigate(a);
			TranslateTransform transform_Addfriend = (TranslateTransform)a.RenderTransform;

			DoubleAnimation animation_signup = new DoubleAnimation();
			animation_signup.To = 0;
			animation_signup.Duration = TimeSpan.FromSeconds(0.3);

			transform_Addfriend.BeginAnimation(TranslateTransform.YProperty, animation_signup);
		}
		private void RadioButton_CreateGroup(object sender, RoutedEventArgs e)
		{

			foreach (var radioButton in Menu.Children.OfType<RadioButton>().Where(rb => rb.GroupName == "MyGroup"))
			{
				radioButton.BorderThickness = new Thickness(0);
			}
			RadioButton checkedRadioButton = sender as RadioButton;
			checkedRadioButton.IsChecked = true;
			checkedRadioButton.BorderThickness = new Thickness(0, 0, 0, 2);

			var a = new CreateGroup(account.UserName);
			AddFriendFrame.Navigate(a);
			TranslateTransform transform_Addfriend = (TranslateTransform)a.RenderTransform;

			DoubleAnimation animation_signup = new DoubleAnimation();
			animation_signup.To = 0;
			animation_signup.Duration = TimeSpan.FromSeconds(0.3);

			transform_Addfriend.BeginAnimation(TranslateTransform.YProperty, animation_signup);
		}
		private void RadioButton_Profile(object sender, RoutedEventArgs e)
		{

			foreach (var radioButton in Menu.Children.OfType<RadioButton>().Where(rb => rb.GroupName == "MyGroup"))
			{
				radioButton.BorderThickness = new Thickness(0);
			}
			RadioButton checkedRadioButton = sender as RadioButton;
			checkedRadioButton.IsChecked = true;
			checkedRadioButton.BorderThickness = new Thickness(0, 0, 0, 2);

			var a = new Profile(this,account);
			AddFriendFrame.Navigate(a);
			TranslateTransform transform_Addfriend = (TranslateTransform)a.RenderTransform;

			DoubleAnimation animation_signup = new DoubleAnimation();
			animation_signup.To = 0;
			animation_signup.Duration = TimeSpan.FromSeconds(0.3);

			transform_Addfriend.BeginAnimation(TranslateTransform.YProperty, animation_signup);
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
        private void Choose_BoxChat(object sender, MouseButtonEventArgs e)
        {
            TextBlock clickedTextBlock = (TextBlock)sender;
            string textValue = clickedTextBlock.Text;
            foreach(Information IF in boxchat)
            {
                if (IF.UserName == textValue)
                {
                    BoxChatFrame.Navigate(new BoxChat(this,account.UserName,IF.UserName,IF.Avatar,IF.IDChatBox,IF.TypeBoxChat));
                    return;
                }
            }
            
        }



        private void TextBox_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string searchText = TextBox_Search.Text;
                List<Information> search = boxchat.Where(info => info.UserName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                Chats_ListView.ItemsSource = null;
                Chats_ListView.ItemsSource = search;

            }
        }
		public void NavigateToPage(Page page)
		{
			AddFriendFrame.Navigate(page);
			TranslateTransform transform_Addfriend = (TranslateTransform)page.RenderTransform;

			DoubleAnimation animation_signup = new DoubleAnimation();
			animation_signup.To = 0;
			animation_signup.Duration = TimeSpan.FromSeconds(0.3);

			transform_Addfriend.BeginAnimation(TranslateTransform.YProperty, animation_signup);
		}
		private void signout(object sender, RoutedEventArgs e)
		{
			database.DataChange("Update Account set Online = 'Transparent' where UserName ='" + account.UserName + "'");
			connection.InvokeAsync("Offline", account.UserName);
			ParentWindow.mainFrame.Navigate(null);
			var a = new Login(ParentWindow);
			ParentWindow.mainFrame.Navigate(a);
			TranslateTransform transform = (TranslateTransform)a.RenderTransform;

			DoubleAnimation animation = new DoubleAnimation();
			animation.To = 0;
			animation.Duration = TimeSpan.FromSeconds(0.7);

			transform.BeginAnimation(TranslateTransform.YProperty, animation);
		}
		
		private void Home_Loaded(object sender, RoutedEventArgs e)
		{
			// Lấy cửa sổ chứa trang Home
			Window mainWindow = Window.GetWindow(this);

			if (mainWindow != null)
			{
				// Thêm sự kiện xử lý cho sự kiện Closing của cửa sổ
				mainWindow.Closing += MainWindow_Closing;
			}
		}
		private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			database.DataChange("Update Account set Online = 'Transparent' where UserName ='" + account.UserName + "'");
			connection.InvokeAsync("Offline", account.UserName);
		}
		public IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
		{
			if (depObj != null)
			{
				for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
				{
					DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
					if (child != null && child is T)
					{
						yield return (T)child;
					}

					foreach (T childOfChild in FindVisualChildren<T>(child))
					{
						yield return childOfChild;
					}
				}
			}
		}
	}
}
