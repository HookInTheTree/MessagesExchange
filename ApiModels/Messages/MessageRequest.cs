using System.ComponentModel.DataAnnotations;

namespace MessagesExchange.ApiModels.Messages
{
    public class MessageRequest
    {
        [Required]
        public string Message { get; set; }
        
        [Required]
        public int Order { get; set; }
    }
}
