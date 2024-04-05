using Microsoft.AspNetCore.SignalR;
namespace ChatOnline.SignalR
{
    public class ChatHub:Hub
    {
        public async Task SendMessage(string friend,string username, string dataType, string content)
        {
            await Clients.Others.SendAsync(friend,username, dataType, content);
        }
		public async Task Online(string username)
		{
			await Clients.Others.SendAsync(username);
		}
		public async Task Online1(string username)
		{
			await Clients.Others.SendAsync(username + "onl");
		}
		public async Task Offline(string username)
		{
			await Clients.Others.SendAsync(username + "off");
		}
	}
}
