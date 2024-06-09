﻿namespace MessagesExchange.Data.Messages
{
    public interface IMessagesRepository
    {
        Task<Message> CreateAsync(Message message);
        Task<Message> GetAsync(Guid id);
        Task<List<Message>> GetAsync(DateTime from, DateTime to);
        Task<List<Message>> GetAsync();
    }
}
