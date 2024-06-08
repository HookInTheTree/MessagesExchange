
using Dapper;

namespace MessagesExchange.Infrastructure.Migrations
{
    public class MigrationsRepository : IMigrationsRepository
    {
        private readonly string _dbString;
        public MigrationsRepository(IConfiguration configuration)
        {
            _dbString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task CreateMigrationInfo(MigrationInfo migrationInfo)
        {
            using var connection = SqlConnectionsFactory.CreateConnection(_dbString);
            await connection.OpenAsync();
            try
            {
                var sql = $"INSERT INTO migrations (name) ('{migrationInfo.Name}');";
                await connection.ExecuteAsync(sql);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
