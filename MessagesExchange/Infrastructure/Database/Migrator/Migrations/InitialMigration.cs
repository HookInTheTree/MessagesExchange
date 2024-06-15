using Dapper;
using MessagesExchange.Infrastructure.Database;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace MessagesExchange.Infrastructure.Database.Migrator.Migrations
{
    /// <summary>
    /// Миграция инициализации базы данных и таблицы миграций
    /// </summary>
    public class InitialMigration : Migration
    {
        private readonly SqlConnectionsFactory _connectionsFactory;
        private readonly IConfiguration _configuration;
        public InitialMigration(SqlConnectionsFactory connectionsFactory, IConfiguration configuration)
        {
            _connectionsFactory = connectionsFactory;
            _configuration = configuration;
        }

        /// <summary>
        /// Метод для применения миграции
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async override Task Execute(CancellationToken cancellationToken)
        {
            await CreateDatabase(cancellationToken);
            await CreateExtensions(cancellationToken);
            await CreateMigrationsTable(cancellationToken);
        }

        /// <summary>
        /// Метод для получения названия базы данных 
        /// </summary>
        /// <param name="defaultConnectionString"></param>
        /// <returns></returns>
        private string GetDatabaseName(string defaultConnectionString)
        {
            return defaultConnectionString.Split(';')
                .FirstOrDefault(x => x.Contains("Database="))
                .Split('=')
                .LastOrDefault();
        }

        /// <summary>
        /// Метод для создания базы данных
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task CreateDatabase(CancellationToken cancellationToken)
        {
            var databaseName = GetDatabaseName(_configuration.GetConnectionString("DefaultConnection"));
            var sql = @$"CREATE DATABASE {databaseName}";

            using var connection = _connectionsFactory.CreateConnection(_configuration.GetConnectionString("MigrationsConnection"));
            await connection.ExecuteAsync(sql, cancellationToken);
        }

        /// <summary>
        /// Метод для установки расширений
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task CreateExtensions(CancellationToken cancellationToken)
        {
            var sql = @"CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";";

            using var connection = _connectionsFactory.CreateConnection();
            await connection.ExecuteAsync(sql, cancellationToken);
        }

        /// <summary>
        /// Метод для 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task CreateMigrationsTable(CancellationToken cancellationToken)
        {
            var sql = @$"
                    CREATE TABLE IF NOT EXISTS migrations (
                        id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
                        name TEXT NOT NULL,
                        sequential_number SERIAL NOT NULL UNIQUE
                    );
                ";

            using var connection = _connectionsFactory.CreateConnection();
            await connection.ExecuteAsync(sql, cancellationToken);
        }
    }
}
