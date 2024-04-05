using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using MaterialDesignThemes.Wpf;
using System.Data;
using Object;
using ChatOnline.Object;
using System.ComponentModel.Design;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using Microsoft.Owin.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows.Controls.Primitives;
using Emoji.Wpf;
using System.ComponentModel;
using System.Xml.Linq;
namespace ChatOnline
{
    /// <summary>
    /// Interaction logic for BoxChat.xaml
    /// </summary>
    public partial class BoxChat : Page
    {

        public Home ParentWindow { get; set; }
        DataBaseProcess database = new DataBaseProcess();
        private DispatcherTimer scrollTimer;
        private string Send_image="";
        string UserName, FriendName, Avatar;
        int id,type;
        int topOffset = 0;
        HubConnection connection = new HubConnectionBuilder().WithUrl("http://localhost:5000/chat").Build();
        public BoxChat(Home parent,string username,string friendname,string avatar,int ID,int TypeBoxChat)
        {
            InitializeComponent();
            ParentWindow = parent;
            UserName = username; FriendName = friendname; id = ID;type = TypeBoxChat;
            Avatar = avatar;
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(Avatar));
            AvatarButton.Background = imageBrush;
            Load_Message();
            TextBox_send.Text = "Gửi tin nhắn tới " + FriendName;
            connection.StartAsync();
            if(TypeBoxChat == 0)
            {
                connection.On<string,string, string>(UserName, (username,datatype, content) =>
                {
                    if (username != FriendName)
                    {
                        Mediator.Notify("PerformFunctionOnPage1");
                    }
                    else
                    {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
						if (datatype == "Icon")
						{
							Get_Message_Icon(username, datatype, content);
						}
						else
						{
							Get_Message(username, datatype, content);
						}
						Mediator.Notify(UserName);
                    });
                    }
                });
            }else if(TypeBoxChat == 1)
            {
                connection.On<string, string, string>(FriendName, (username, datatype, content) =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (datatype == "Icon")
                        {
                            Get_Message_Icon(username, datatype, content);
						}
                        else
                        {
                        Get_Message(username, datatype, content);
                        }
                        Mediator.Notify(UserName);
                        Mediator.Notify("PerformFunctionOnPage1");
                    });
                });
            }
		}
        private void OpenFileDialogButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Hình ảnh và Video|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.mp4;*.avi;*.mkv;*.mov|All Files|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                Send_image = selectedFilePath;
                Image_send.Source = new BitmapImage(new Uri(selectedFilePath));
            }
        }
        public void Load_Message()
        {
            DataTable DataChat = database.DataReader("Select SendTime,Datatype,Content,SendUser,IDChatData from ChatData where IDChatData =" +id+" order by SendTime asc");
            List<ChatData> datachats = new List<ChatData>();
            foreach (DataRow row in DataChat.Rows)
            {
                ChatData datachat = new ChatData
                {
                    SendTime = (DateTime)row["SendTime"],
                    Datatype = row["Datatype"].ToString(),
                    Content = row["Content"].ToString(),
                    sendUser = row["SendUser"].ToString(),
                    IDChatData = (int)row["IDChatData"]
                };

                datachats.Add(datachat);
            }

            foreach (ChatData datachat in datachats)
            {
                Border border = new Border();
                border.CornerRadius = new CornerRadius(10);
                LinearGradientBrush linearGradientBrush = new LinearGradientBrush();

                // Đặt màu nền cho Gradient
                double opacityPercentage = 50; // Độ mờ 50%
                byte opacityValue = (byte)(255 * opacityPercentage / 100);
                linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(opacityValue, 46, 49, 146), 0.0));
                linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(opacityValue, 27, 255, 255), 1.0));

                
                border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E205B"));
                Button roundButton = new Button
                {
                    Width = 20,
                    Height = 20
                };
                if(type == 0)
                {
                roundButton.Background = new ImageBrush(new BitmapImage(new Uri(Avatar)));
                }
                else if(type == 1)
                {
                    DataTable avatar = database.DataReader("Select Avatar from Account where UserName = '" + datachat.sendUser + "'");
                    string avatarValue = avatar.Rows[0]["Avatar"].ToString();
                    roundButton.Background = new ImageBrush(new BitmapImage(new Uri(avatarValue)));
                }

                // Đặt ControlTemplate cho Button để tạo hình tròn
                ControlTemplate controlTemplate = new ControlTemplate(typeof(Button));
                FrameworkElementFactory ellipse = new FrameworkElementFactory(typeof(Ellipse));
                ellipse.SetValue(Ellipse.WidthProperty, 20.0);
                ellipse.SetValue(Ellipse.HeightProperty, 20.0);
                ellipse.SetValue(Ellipse.FillProperty, new TemplateBindingExtension(Button.BackgroundProperty));
                FrameworkElementFactory grid = new FrameworkElementFactory(typeof(Grid));
                grid.AppendChild(ellipse);
                controlTemplate.VisualTree = grid;
                roundButton.Template = controlTemplate;


                if (datachat.Datatype == "Text")
                {
                    System.Windows.Controls.TextBlock message = new System.Windows.Controls.TextBlock();
                    message.Text = datachat.Content;
                    message.Foreground = new SolidColorBrush(Colors.White);
                    message.Padding = new Thickness(10);
                    message.MaxWidth = 300;
                    message.TextWrapping = TextWrapping.Wrap;
                    // Đặt tọa độ Y cho System.Windows.Controls.TextBlock trong Border
                    Canvas.SetTop(border, topOffset);
                    Canvas.SetTop(roundButton, topOffset);
                    // Thêm System.Windows.Controls.TextBlock vào Border
                    border.Child = message;
                    border.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    Size desiredSize = border.DesiredSize;

                    double estimatedHeight = desiredSize.Height + 10;
                    if (datachat.sendUser == UserName)
                    {
                        Canvas.SetRight(border, 20);
                    }
                    else
                    {
                        
                        Canvas.SetLeft(border, 35);
                        Canvas.SetLeft(roundButton, 5);
                        myCanvas.Children.Add(roundButton);
                        border.Background = linearGradientBrush;
                    }

                    // Thêm Border vào Canvas
                    myCanvas.Children.Add(border);
                    topOffset += (int)estimatedHeight;

                }
                else if (datachat.Datatype == "Image")
                {
					// Xử lý hiển thị hình ảnh
					System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                    
                    // Thiết lập Source của hình ảnh từ datachat.Content (điều này đòi hỏi bạn có đường dẫn hình ảnh)
                    image.Source = new BitmapImage(new Uri(datachat.Content));

                    // Đặt tọa độ cho hình ảnh trong Border
                    Canvas.SetTop(border, topOffset);
                    Canvas.SetTop(roundButton, topOffset);

                    // Giới hạn kích thước của hình ảnh
                    image.MaxWidth = 300;
                    image.MaxHeight = 200;

                    // Thêm hình ảnh vào Border
                    border.Child = image;

                    if (datachat.sendUser == UserName)
                    {
                        Canvas.SetRight(border, 20);
                    }
                    else
                    {
                        Canvas.SetLeft(border, 35);
                        Canvas.SetLeft(roundButton, 5);
                        myCanvas.Children.Add(roundButton);
                    }

                    // Thêm Border vào Canvas
                    myCanvas.Children.Add(border);
                    topOffset += 220;
                }else if (datachat.Datatype == "Icon")
				{
					border.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
					Emoji.Wpf.TextBlock message = new Emoji.Wpf.TextBlock();
					message.Text = datachat.Content;
                    message.FontSize = 40;
					Canvas.SetTop(border, topOffset);
					Canvas.SetTop(roundButton, topOffset);
					border.Child = message;
					border.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
					Size desiredSize = border.DesiredSize;

					double estimatedHeight = desiredSize.Height + 10;
					if (datachat.sendUser == UserName)
					{
						Canvas.SetRight(border, 20);
					}
					else
					{

						Canvas.SetLeft(border, 35);
						Canvas.SetLeft(roundButton, 5);
						myCanvas.Children.Add(roundButton);
					}

					// Thêm Border vào Canvas
					myCanvas.Children.Add(border);
					topOffset += (int)estimatedHeight;

				}

			}
            myCanvas.Height = topOffset ;
            myScrollViewer.ScrollToBottom();
            TextBlock_FriendName.Text = FriendName;

        }
        public async void Send_Message(object sender, RoutedEventArgs e)
        {
            if (TextBox_send.Text!= "" && TextBox_send.Text != "Gửi tin nhắn tới " + FriendName)
            {
            database.DataChange("Insert into ChatData (IDChatData,SendTime,Datatype,Content,SendUser) values ("+id + ",GETDATE(),'Text',N'" + TextBox_send.Text + "',N'" + UserName + "')");
                await connection.InvokeAsync("SendMessage", FriendName, UserName,"Text", TextBox_send.Text);
                Border border = new Border();
                border.CornerRadius = new CornerRadius(10);


                border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E205B"));

                System.Windows.Controls.TextBlock message = new System.Windows.Controls.TextBlock();
                message.Text = TextBox_send.Text;
                message.Foreground = new SolidColorBrush(Colors.White);
                message.Padding = new Thickness(10);
                message.MaxWidth = 300;
                message.TextWrapping = TextWrapping.Wrap;
                Canvas.SetTop(border, topOffset);
                border.Child = message;

                Canvas.SetRight(border, 20);
                myCanvas.Children.Add(border);
                border.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                Size desiredSize = border.DesiredSize;

                double estimatedHeight = desiredSize.Height + 10;
                topOffset += (int)estimatedHeight;
                myCanvas.Height = topOffset;
                TextBox_send.Text = "";
                myScrollViewer.ScrollToVerticalOffset(myScrollViewer.VerticalOffset + 1);
                ScrollToBottomWithAnimation();

                Mediator.Notify(UserName);
            }
            if(Send_image != "")
            {
                database.DataChange("Insert into ChatData (IDChatData,SendTime,Datatype,Content,SendUser) values (" + id + ",GETDATE(),'Image',N'" + Send_image + "',N'" + UserName + "')");
                await connection.InvokeAsync("SendMessage", FriendName, UserName, "Image", Send_image);
                Border border = new Border();
                border.CornerRadius = new CornerRadius(10);
				System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = new BitmapImage(new Uri(Send_image));

                Canvas.SetTop(border, topOffset);

                image.MaxWidth = 300;
                image.MaxHeight = 200;

                border.Child = image;
                Canvas.SetRight(border, 20);

                myCanvas.Children.Add(border);
                topOffset += 220;
                myCanvas.Height = topOffset;
                Image_send.Source = null;
                Send_image = "";
                myScrollViewer.ScrollToVerticalOffset(myScrollViewer.VerticalOffset + 1);
                ScrollToBottomWithAnimation();

                Mediator.Notify(UserName);
            }
            
        }
        public void Get_Message(string username,string datatype, string content)
        {
                    
                    Border border = new Border();
                    border.CornerRadius = new CornerRadius(10);
                    LinearGradientBrush linearGradientBrush = new LinearGradientBrush();

                    // Đặt màu nền cho Gradient
                    double opacityPercentage = 50; // Độ mờ 50%
                    byte opacityValue = (byte)(255 * opacityPercentage / 100);
                    linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(opacityValue, 46, 49, 146), 0.0));
                    linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(opacityValue, 27, 255, 255), 1.0));

                    border.Background = linearGradientBrush;
                    Button roundButton = new Button
                    {
                        Width = 20,
                        Height = 20
                    };
                    if (type == 0)
                    {
                        roundButton.Background = new ImageBrush(new BitmapImage(new Uri(Avatar)));
                    }
                    else if (type == 1)
                    {
                        DataTable avatar = database.DataReader("Select Avatar from Account where UserName = '" + username + "'");
                        string avatarValue = avatar.Rows[0]["Avatar"].ToString();
                        roundButton.Background = new ImageBrush(new BitmapImage(new Uri(avatarValue)));
                    }

            // Đặt ControlTemplate cho Button để tạo hình tròn
            ControlTemplate controlTemplate = new ControlTemplate(typeof(Button));
                    FrameworkElementFactory ellipse = new FrameworkElementFactory(typeof(Ellipse));
                    ellipse.SetValue(Ellipse.WidthProperty, 20.0);
                    ellipse.SetValue(Ellipse.HeightProperty, 20.0);
                    ellipse.SetValue(Ellipse.FillProperty, new TemplateBindingExtension(Button.BackgroundProperty));
                    FrameworkElementFactory grid = new FrameworkElementFactory(typeof(Grid));
                    grid.AppendChild(ellipse);
                    controlTemplate.VisualTree = grid;
                    roundButton.Template = controlTemplate;


                    if (datatype == "Text")
                    {
                        System.Windows.Controls.TextBlock message = new System.Windows.Controls.TextBlock();

                        message.Text = content;
                        message.Foreground = new SolidColorBrush(Colors.White);
                        message.Padding = new Thickness(10);
                        message.MaxWidth = 300;
                        message.TextWrapping = TextWrapping.Wrap;
                        // Đặt tọa độ Y cho System.Windows.Controls.TextBlock trong Border
                        Canvas.SetTop(border, topOffset);
                        Canvas.SetTop(roundButton, topOffset);
                        // Thêm System.Windows.Controls.TextBlock vào Border
                        border.Child = message;
                        border.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                        Size desiredSize = border.DesiredSize;
                        
                        double estimatedHeight = desiredSize.Height + 10;
                            Canvas.SetLeft(border, 35);
                            Canvas.SetLeft(roundButton, 5);
                            myCanvas.Children.Add(roundButton);

                        // Thêm Border vào Canvas
                        myCanvas.Children.Add(border);
                        topOffset += (int)estimatedHeight;

                    }else if (datatype == "Image")
                    {
				        System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                        image.Source = new BitmapImage(new Uri(content));

                        Canvas.SetTop(border, topOffset);
                        Canvas.SetTop(roundButton, topOffset);
                        image.MaxWidth = 300;
                        image.MaxHeight = 200;

                        border.Child = image;
                        Canvas.SetLeft(border, 35);
                        Canvas.SetLeft(roundButton, 5);
                        myCanvas.Children.Add(roundButton);

                        myCanvas.Children.Add(border);
                        topOffset += 220;
                        myCanvas.Height = topOffset;

                    }

            myCanvas.Height = topOffset;
                    myScrollViewer.ScrollToVerticalOffset(myScrollViewer.VerticalOffset + 1);
                    ScrollToBottomWithAnimation();
        }
        public void Get_Message_Icon(string username, string datatype, string content)
        {
			Border border = new Border();
			border.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
			Button roundButton = new Button
			{
				Width = 20,
				Height = 20
			};
			if (type == 0)
			{
				roundButton.Background = new ImageBrush(new BitmapImage(new Uri(Avatar)));
			}
			else if (type == 1)
			{
				DataTable avatar = database.DataReader("Select Avatar from Account where UserName = '" + username + "'");
				string avatarValue = avatar.Rows[0]["Avatar"].ToString();
				roundButton.Background = new ImageBrush(new BitmapImage(new Uri(avatarValue)));
			}

			// Đặt ControlTemplate cho Button để tạo hình tròn
			ControlTemplate controlTemplate = new ControlTemplate(typeof(Button));
			FrameworkElementFactory ellipse = new FrameworkElementFactory(typeof(Ellipse));
			ellipse.SetValue(Ellipse.WidthProperty, 20.0);
			ellipse.SetValue(Ellipse.HeightProperty, 20.0);
			ellipse.SetValue(Ellipse.FillProperty, new TemplateBindingExtension(Button.BackgroundProperty));
			FrameworkElementFactory grid = new FrameworkElementFactory(typeof(Grid));
			grid.AppendChild(ellipse);
			controlTemplate.VisualTree = grid;
			roundButton.Template = controlTemplate;



			    Emoji.Wpf.TextBlock message = new Emoji.Wpf.TextBlock();
			    message.Text = content;
			    message.FontSize = 40;
			    Canvas.SetTop(border, topOffset);
			    Canvas.SetTop(roundButton, topOffset);
			    border.Child = message;
			    border.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
				Size desiredSize = border.DesiredSize;

				double estimatedHeight = desiredSize.Height + 10;
				Canvas.SetLeft(border, 35);
				Canvas.SetLeft(roundButton, 5);
				myCanvas.Children.Add(roundButton);

				// Thêm Border vào Canvas
				myCanvas.Children.Add(border);
				topOffset += (int)estimatedHeight;
			myCanvas.Height = topOffset;
			myScrollViewer.ScrollToVerticalOffset(myScrollViewer.VerticalOffset + 1);
			ScrollToBottomWithAnimation();

		}
        private void ResetListFriend()
        {

        }
        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        private void SendTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TextBox_send.Text == "Gửi tin nhắn tới " + FriendName)
            {
                TextBox_send.Text = "";
            }
        }

        private void SendTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBox_send.Text))
            {
                TextBox_send.Text = "Gửi tin nhắn tới " + FriendName;
            }
        }
        private void ScrollToBottomWithAnimation()
        {
            scrollTimer = new DispatcherTimer();
            scrollTimer.Interval = TimeSpan.FromMilliseconds(10);
            scrollTimer.Tick += ScrollTimer_Tick;
            scrollTimer.Start();
        }
        private void Checked(object sender, MouseButtonEventArgs e)
        {
            database.DataChange("Update ChatData Set Flag = 1 where IDChatData = " + id + " AND SendTime = (SELECT MAX(SendTime) FROM ChatData WHERE IDChatData = " + id + ")");

        }

		private void myPicker_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{

		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            database.DataChange("Update ChatData Set Flag = 1 where IDChatData = " + id + " AND SendTime = (SELECT MAX(SendTime) FROM ChatData WHERE IDChatData = " + id + ")");
            Mediator.Notify(UserName);
        }
        private void ScrollTimer_Tick(object sender, EventArgs e)
        {
            if (myScrollViewer.VerticalOffset >= myScrollViewer.ExtentHeight - myScrollViewer.ViewportHeight)
            {
                scrollTimer.Stop();
            }
            else
            {
                myScrollViewer.ScrollToVerticalOffset(myScrollViewer.VerticalOffset + 10);
            }
        }
		private async void MyPicker_OnPicked(object sender,EmojiPickedEventArgs e)
		{
			string selectedEmoji = e.Emoji;
			database.DataChange("Insert into ChatData (IDChatData,SendTime,Datatype,Content,SendUser) values (" + id + ",GETDATE(),'Icon',N'" + selectedEmoji + "',N'" + UserName + "')");
			await connection.InvokeAsync("SendMessage", FriendName, UserName, "Icon", selectedEmoji);
			Border border = new Border();
			border.CornerRadius = new CornerRadius(10);


			border.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

			Emoji.Wpf.TextBlock message = new Emoji.Wpf.TextBlock();
			message.Text = selectedEmoji;
            message.FontSize = 40;
			Canvas.SetTop(border, topOffset);
			border.Child = message;

			Canvas.SetRight(border, 20);
			myCanvas.Children.Add(border);
			border.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
			Size desiredSize = border.DesiredSize;

			double estimatedHeight = desiredSize.Height + 10;
			topOffset += (int)estimatedHeight;
			myCanvas.Height = topOffset;
			TextBox_send.Text = "";
			myScrollViewer.ScrollToVerticalOffset(myScrollViewer.VerticalOffset + 1);
			ScrollToBottomWithAnimation();

			Mediator.Notify(UserName);
		}
		private void View_Friend(object sender, RoutedEventArgs e)
		{
            
            if(type == 0)
            {
				Account temp = new Account();
				temp.UserName = FriendName;
				temp.Avatar = Avatar;
				var a = new Profile_View(ParentWindow, temp, UserName);
				ParentWindow.NavigateToPage(a);
			}
			else if(type == 1)
            {
                GroupChat temp = new GroupChat();
                temp.GroupName = FriendName;
                temp.Avatar = Avatar;
                temp.GroupID = id;
                var a = new Group_View(ParentWindow,temp, UserName);
				ParentWindow.NavigateToPage(a);
			}
            
		}
	}
}
