using Dapper;
using MessagesExchange.Models;

namespace MessagesExchange.Infrastructure.Database.Messages
{
    /// <summary>
    /// Реализация репозитория сообщений
    /// </summary>
    public class MessagesRepository : IMessagesRepository
    {
        private readonly SqlConnectionsFactory _connectionsFactory;
        public MessagesRepository(SqlConnectionsFactory sqlConnectionsFactory)
        {
            _connectionsFactory = sqlConnectionsFactory;
        }

        public async Task<Message> CreateAsync(Message message)
        {
            var sql = @"INSERT INTO messages (id, text, order_id, created_at) VALUES (@Id, @Text, @OrderId, @CreatedAt)";

            using var connection = _connectionsFactory.CreateConnection();
            await connection.ExecuteAsync(
                sql: sql,
                param: new
                {
                    message.Id,
                    message.Text,
                    message.OrderId,
                    message.CreatedAt
                });

            return message;
        }

        public async Task<Message> GetAsync(Guid id)
        {
            var sql = @"SELECT 
                                id as Id,
                                text as Text,
                                order_id as OrderId,
                                created_at as CreatedAt
                            from messages
                            where id = @Id";

            using var connection = _connectionsFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Message>(sql, new { Id = id });
        }

        public async Task<List<Message>> GetAsync(DateTime from, DateTime to)
        {
            var sql = @"SELECT 
                                id as Id,
                                text as Text,
                                order_id as OrderId,
                                created_at as CreatedAt
                            from messages
                            where created_at >= @From and created_at <= @To";

            using var connection = _connectionsFactory.CreateConnection();
            var result = await connection.QueryAsync<Message>(sql, new { From = from, To = to });
            return result.ToList();
        }

        public async Task<List<Message>> GetAsync()
        {
            var sql = @"SELECT 
                                id as Id,
                                text as Text,
                                order_id as OrderId,
                                created_at as CreatedAt
                            from messages";

            using var connection = _connectionsFactory.CreateConnection();
            var result = await connection.QueryAsync<Message>(sql);
            return result.ToList();
        }
    }
}
