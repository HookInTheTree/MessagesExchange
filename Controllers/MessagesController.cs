using MessagesExchange.ApiModels.Messages;
using MessagesExchange.Infrastructure.Database.Messages;
using MessagesExchange.Infrastructure.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MessagesExchange.Controllers
{
    [Route("api/messages")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessagesRepository _messagesRepository;
        private readonly IHubContext<MessagesRealTimeHub> _hubContext;
        public MessagesController(IMessagesRepository messagesRepository, IHubContext<MessagesRealTimeHub> hubContext)
        {
            _messagesRepository = messagesRepository;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<MessageResponse>>> Get(string dateFrom = "", string dateTo = "")
        {
            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MinValue;

            if (!string.IsNullOrEmpty(dateFrom) && !DateTime.TryParse(dateFrom, out fromDate))
            {
                return BadRequest("dateFrom parameter is invalid");
            }

            if (!string.IsNullOrEmpty(dateTo) && !DateTime.TryParse(dateTo, out toDate))
            {
                return BadRequest("dateTo parameter is invalid");
            }

            List<Message> messages;
            if (fromDate != DateTime.MinValue || toDate != DateTime.MinValue)
            {
                toDate = toDate == DateTime.MinValue ? DateTime.MaxValue : toDate;
                messages = await _messagesRepository.GetAsync(fromDate, toDate);
            }
            else
            {
                messages = await _messagesRepository.GetAsync();
            }

            return Ok(
                messages.Select(x => new MessageResponse()
                {
                    Message = x.Text,
                    CreatedAt = x.CreatedAt.ToString(),
                    OrderNumber = x.OrderId
                }));
        }

        [HttpPost]
        public async Task<ActionResult<MessageResponse>> Post(MessageRequest request)
        {
            var message = await _messagesRepository.CreateAsync(new()
            {
                Id = Guid.NewGuid(),
                Text = request.Message,
                OrderId = request.Order,
                CreatedAt = DateTime.Now
            });

            var response = new MessageResponse()
            {
                Message = message.Text,
                CreatedAt = message.CreatedAt.ToString(),
                OrderNumber = message.OrderId
            };

            await _hubContext.Clients.All.SendAsync("Send", response);
            return Ok(response);
        }
    }
}
