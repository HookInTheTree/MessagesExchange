using MessagesExchange.ApiModels.Messages;
using MessagesExchange.Data.Messages;
using Microsoft.AspNetCore.Mvc;

namespace MessagesExchange.Controllers
{
    [Route("api/messages")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessagesRepository _messagesRepository;
        public MessagesController(IMessagesRepository messagesRepository)
        {
            _messagesRepository = messagesRepository;
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

            return Ok(new MessageResponse()
            {
                Message = message.Text,
                CreatedAt = message.CreatedAt.ToString(),
                OrderNumber = message.OrderId
            });
        }
    }
}
