
using Microsoft.Owin.Hosting;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.AspNetCore.SignalR;
namespace ChatOnline
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
			InitializeComponent();
			var a = new Login(this);
			mainFrame.Navigate(a );
			TranslateTransform transform = (TranslateTransform)a.RenderTransform;

			DoubleAnimation animation = new DoubleAnimation();
			animation.To = 0;
			animation.Duration = TimeSpan.FromSeconds(0.7);

			transform.BeginAnimation(TranslateTransform.YProperty, animation);
			// Kiểm tra xem cửa sổ đã được tạo chưa
			if (!IsWindowAlreadyCreated)
			{
				IsWindowAlreadyCreated = true;

				// Tạo một cửa sổ MainWindow mới
				var newMainWindow = new MainWindow();
				newMainWindow.Show();
			}
		}

        private static bool IsWindowAlreadyCreated = false;
		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn đóng cửa sổ?", "Xác nhận", MessageBoxButton.OK);

			if (result == MessageBoxResult.OK)
			{
				e.Cancel = false;
			}
		}
	}

}
