using Microsoft.AspNetCore.SignalR;

namespace Chat.Server.Hubs
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, string> Users = new Dictionary<string, string>();
        public override async Task OnConnectedAsync()
        {
            string username = Context.GetHttpContext().Request.Query["username"];
            Users.Add(Context.ConnectionId, username);
            await AddMessageToChat(string.Empty, $"{username} connected!");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string username = Users.FirstOrDefault(U => U.Key == Context.ConnectionId).Value;
            await AddMessageToChat(String.Empty, $"{username} Left the chat"); 
            Users.Remove(Context.ConnectionId);
        }

        public async Task AddMessageToChat(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
