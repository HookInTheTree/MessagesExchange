
using Dapper;

namespace MessagesExchange.Infrastructure.Migrations.DatabaseMigrations
{
    public class MessagesTableMigration : Migration
    {
        private readonly SqlConnectionsFactory _connectionsFactory;
        public MessagesTableMigration(SqlConnectionsFactory connectionsFactory)
        {
            _connectionsFactory = connectionsFactory;
        }

        public override async Task Execute(CancellationToken cancellationToken = default)
        {
            using var connection = _connectionsFactory.CreateConnection();

            await connection.OpenAsync(cancellationToken);
            try
            {
                var sql = @"
                    CREATE TABLE IF NOT EXISTS messages (
                        id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
                        text TEXT NOT NULL,
                        order_id int not null,
                        created_at timestamp without time zone NOT NULL
                    );
                ";
                await connection.ExecuteAsync(sql, cancellationToken);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
