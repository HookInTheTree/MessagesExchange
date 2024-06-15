using System.ComponentModel.DataAnnotations.Schema;

namespace MessagesExchange.Infrastructure.Database.Messages
{
    /// <summary>
    /// Модель данных сообщения
    /// </summary>
    public class Message
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        public int OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
