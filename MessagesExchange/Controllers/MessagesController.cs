using MessagesExchange.ApiModels.Messages;
using MessagesExchange.Infrastructure.Database.Messages;
using MessagesExchange.Infrastructure.SignalR;
using MessagesExchange.Models;
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

        /// <summary>
        /// Метод для получения сообщений. Принимает необязательные параметры, фильтрующие сообщения по дате создания.
        /// </summary>
        /// <param name="dateFrom">Дата, с которой необходимо фильтровать сообщения</param>
        /// <param name="dateTo">Дата по которую необходимо фильтровать сообщения</param>
        /// <response code="200">Список сообщений, удовлетворяющий параметрам запроса</response>
        /// <response code="500">Непредвиденная ошибка сервера ошибка сервера</response>
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

        /// <summary>
        /// Метод для создания сообщения и оповещения подписчиков.
        /// </summary>
        /// <param name="request">Сообщение</param>
        /// <response code="200">Сообщение успешно создано, все получатели оповещены</response>
        /// <response code="400">Данные, отправленные для создания сообщения невалидны.</response>
        /// <response code="500">Непредвиденная ошибка сервера ошибка сервера</response>
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

            await _hubContext.Clients.All.SendAsync("RecieveMessage", response);
            return Ok(response);
        }
    }
}
