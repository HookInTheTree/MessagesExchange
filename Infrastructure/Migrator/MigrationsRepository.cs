
using Dapper;

namespace MessagesExchange.Infrastructure.Migrations
{
    public class MigrationsRepository : IMigrationsRepository
    {
        private readonly SqlConnectionsFactory _connectionFactory;
        public MigrationsRepository(SqlConnectionsFactory sqlConnectionsFactory)
        {
            _connectionFactory = sqlConnectionsFactory;
        }

        public async Task CreateMigrationInfo(MigrationInfo migrationInfo)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();
            try
            {
                var sql = $"INSERT INTO migrations (name) values ('{migrationInfo.Name}');";
                await connection.ExecuteAsync(sql);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<List<MigrationInfo>> GetMigrations()
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();
            try
            {
                var sql = @"SELECT 
                                id as Id,
                                name as Name,
                                sequential_number as SequentialNumber
                            from migrations";

                var result = await connection.QueryAsync<MigrationInfo>(sql);
                return result.ToList();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
