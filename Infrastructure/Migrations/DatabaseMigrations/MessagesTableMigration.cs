
using Dapper;

namespace MessagesExchange.Infrastructure.Migrations.DatabaseMigrations
{
    public class MessagesTableMigration : Migration
    {
        private readonly string _dbString;
        public MessagesTableMigration(IConfiguration configuration)
        {
            _dbString = configuration.GetConnectionString("DefaultConnection");
        }

        public override async Task Execute(CancellationToken cancellationToken = default)
        {
            using var connection = SqlConnectionsFactory.CreateConnection(_dbString);

            await connection.OpenAsync(cancellationToken);
            try
            {
                var sql = @"
                    CREATE TABLE IF NOT EXISTS Message (
                        Id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
                        Name TEXT NOT NULL,
                        CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
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
