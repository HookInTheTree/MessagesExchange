
using Dapper;
using MessagesExchange.Infrastructure.Database;

namespace MessagesExchange.Infrastructure.Database.Migrator
{
    /// <summary>
    /// Реализация репозитория сообщений
    /// </summary>
    public class MigrationsRepository : IMigrationsRepository
    {
        private readonly SqlConnectionsFactory _connectionFactory;
        public MigrationsRepository(SqlConnectionsFactory sqlConnectionsFactory)
        {
            _connectionFactory = sqlConnectionsFactory;
        }

        public async Task CreateMigrationInfo(MigrationInfo migrationInfo)
        {
            var sql = $"INSERT INTO migrations (name) values ('{migrationInfo.Name}');";
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql);
        }

        public async Task<List<MigrationInfo>> GetMigrations()
        {
            var sql = @"SELECT 
                                id as Id,
                                name as Name,
                                sequential_number as SequentialNumber
                            from migrations";

            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.QueryAsync<MigrationInfo>(sql);
            return result.ToList();
        }
    }
}
