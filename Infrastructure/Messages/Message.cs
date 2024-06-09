using System.ComponentModel.DataAnnotations.Schema;

namespace MessagesExchange.Data.Messages
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        public int OrderId{ get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
