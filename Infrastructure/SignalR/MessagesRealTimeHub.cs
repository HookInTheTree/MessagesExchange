using MessagesExchange.ApiModels.Messages;
using Microsoft.AspNetCore.SignalR;

namespace MessagesExchange.Infrastructure.SignalR
{
    public class MessagesRealTimeHub:Hub
    {
        public async Task Send(MessageResponse message)
        {
            await Clients.Group(GetClientIdentifier()).SendAsync("Receive", message);
        }

        public override async Task OnConnectedAsync()
        {
            var userIdentifier = Context.GetHttpContext().Request.Query["identifier"];
            await Groups.AddToGroupAsync(Context.ConnectionId, GetClientIdentifier());
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (exception != null)
            {
                //logging
            }
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetClientIdentifier());
        }

        private string GetClientIdentifier()
        {
            return Context.GetHttpContext().Request.Query["identifier"];
        }
    }
}
