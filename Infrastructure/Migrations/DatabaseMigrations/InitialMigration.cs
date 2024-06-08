using Dapper;

namespace MessagesExchange.Infrastructure.Migrations.DatabaseMigrations
{
    public class InitialMigration : Migration
    {
        private readonly string _dbConnectionString;
        public InitialMigration(IConfiguration configuration)
        {
            _dbConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async override Task Execute(CancellationToken cancellationToken)
        {
            using var connection = SqlConnectionsFactory.CreateConnection(_dbConnectionString);

            await connection.OpenAsync(cancellationToken);
            try
            {
                var sql = @"
                    CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";

                    CREATE TABLE IF NOT EXISTS migrations (
                        id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
                        name TEXT NOT NULL,
                        sequential_number SERIAL NOT NULL UNIQUE
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
