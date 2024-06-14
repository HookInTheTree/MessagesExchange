using System.ComponentModel.DataAnnotations;

namespace MessagesExchange.ApiModels.Messages
{
    public class MessageRequest
    {
        [Required]
        [MaxLength(128)]
        public string Message { get; set; }
        
        [Required]
        public int Order { get; set; }
    }
}
