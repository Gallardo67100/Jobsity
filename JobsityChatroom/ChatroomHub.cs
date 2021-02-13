using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace JobsityChatroom
{
    public class ChatroomHub : Hub
    {
        public async Task SendMessage(string user, string message, DateTime timestamp)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message, timestamp);
        }
    }
}
