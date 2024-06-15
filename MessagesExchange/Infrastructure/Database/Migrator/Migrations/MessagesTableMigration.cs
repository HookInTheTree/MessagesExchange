
using Dapper;
using MessagesExchange.Infrastructure.Database;

namespace MessagesExchange.Infrastructure.Database.Migrator.Migrations
{
    /// <summary>
    /// Миграция для создания таблицы сообщений
    /// </summary>
    public class MessagesTableMigration : Migration
    {
        private readonly SqlConnectionsFactory _connectionsFactory;
        public MessagesTableMigration(SqlConnectionsFactory connectionsFactory)
        {
            _connectionsFactory = connectionsFactory;
        }

        /// <summary>
        /// Метод для применения миграций
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task Execute(CancellationToken cancellationToken = default)
        {
            var sql = @"
                CREATE TABLE IF NOT EXISTS messages (
                    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
                    text TEXT NOT NULL,
                    order_id int not null,
                    created_at timestamp without time zone NOT NULL
                );
            ";

            using var connection = _connectionsFactory.CreateConnection();
            await connection.ExecuteAsync(sql, cancellationToken);
        }
    }
}
