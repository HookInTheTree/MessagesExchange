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

        private string GetClientIdentifier()
        {
            return Context.GetHttpContext().Request.Query["identifier"];
        }
    }
}
