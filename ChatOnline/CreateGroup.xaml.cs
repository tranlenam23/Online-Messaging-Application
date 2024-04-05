using ChatOnline.Object;
using Object;
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

namespace ChatOnline
{
	/// <summary>
	/// Interaction logic for CreateGroup.xaml
	/// </summary>
	public partial class CreateGroup : Page
	{
		DataBaseProcess database = new DataBaseProcess();
		string Username;
		public CreateGroup(string username)
		{
			InitializeComponent();
			Username = username;
		}

		private void Create(object sender, MouseButtonEventArgs e)
		{
			if(Name_Group.Text != "")
			{
				database.DataChange("DECLARE @NewGroupID TABLE (GroupID INT); insert into GroupChat(GroupName,Avatar) OUTPUT INSERTED.GroupID INTO @NewGroupID values(N'" + Name_Group.Text + "','https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR8kyEPc77fKT0nJxmeeHYfsngjRK_ulgQe4XTwAIv8jNdH8XPA6ImOxEwnh6Advk-1b3g&usqp=CAU'); INSERT INTO Account_Group (UserName, GroupID) SELECT '" + Username + "', GroupID FROM @NewGroupID;");
				Create_button.Text = "Succsess";
				Create_button.IsEnabled = false;
			}
        }
    }
}
