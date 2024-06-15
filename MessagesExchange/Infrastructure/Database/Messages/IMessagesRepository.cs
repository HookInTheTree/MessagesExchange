using MessagesExchange.Models;

namespace MessagesExchange.Infrastructure.Database.Messages
{
    /// <summary>
    /// Контракт репозитория сообщений
    /// </summary>
    public interface IMessagesRepository
    {
        Task<Message> CreateAsync(Message message);
        Task<Message> GetAsync(Guid id);
        Task<List<Message>> GetAsync(DateTime from, DateTime to);
        Task<List<Message>> GetAsync();
    }
}
