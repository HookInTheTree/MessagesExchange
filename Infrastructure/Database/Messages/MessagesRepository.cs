using Dapper;
using MessagesExchange.Infrastructure.Database;

namespace MessagesExchange.Infrastructure.Database.Messages
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly SqlConnectionsFactory _connectionsFactory;
        public MessagesRepository(SqlConnectionsFactory sqlConnectionsFactory)
        {
            _connectionsFactory = sqlConnectionsFactory;
        }

        public async Task<Message> CreateAsync(Message message)
        {
            using var connection = _connectionsFactory.CreateConnection();
            await connection.OpenAsync();
            try
            {
                var sql = @"INSERT INTO messages (id, text, order_id, created_at) VALUES (@Id, @Text, @OrderId, @CreatedAt)";

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
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<Message> GetAsync(Guid id)
        {
            using var connection = _connectionsFactory.CreateConnection();
            await connection.OpenAsync();
            try
            {
                var sql = @"SELECT 
                                id as Id,
                                text as Text,
                                order_id as OrderId,
                                created_at as CreatedAt
                            from messages
                            where id = @Id";

                return await connection.QueryFirstOrDefaultAsync<Message>(sql, new { Id = id });
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<List<Message>> GetAsync(DateTime from, DateTime to)
        {
            using var connection = _connectionsFactory.CreateConnection();
            await connection.OpenAsync();
            try
            {
                var sql = @"SELECT 
                                id as Id,
                                text as Text,
                                order_id as OrderId,
                                created_at as CreatedAt
                            from messages
                            where created_at >= @From and created_at <= @To";

                var result = await connection.QueryAsync<Message>(sql, new { From = from, To = to });
                return result.ToList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<List<Message>> GetAsync()
        {
            using var connection = _connectionsFactory.CreateConnection();
            await connection.OpenAsync();
            try
            {
                var sql = @"SELECT 
                                id as Id,
                                text as Text,
                                order_id as OrderId,
                                created_at as CreatedAt
                            from messages";

                var result = await connection.QueryAsync<Message>(sql);
                return result.ToList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
