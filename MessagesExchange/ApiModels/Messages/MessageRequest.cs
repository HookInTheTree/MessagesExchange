using System.ComponentModel.DataAnnotations;

namespace MessagesExchange.ApiModels.Messages
{
    public class MessageRequest
    {
        [Required(ErrorMessage ="Message is required")]
        [MaxLength(128)]
        public string Message { get; set; }
        
        [Required(ErrorMessage ="Message is required")]
        public int Order { get; set; }
    }
}
