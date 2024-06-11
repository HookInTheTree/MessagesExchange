using Dapper;
using MessagesExchange.Infrastructure.Database;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace MessagesExchange.Infrastructure.Database.Migrator.Migrations
{
    public class InitialMigration : Migration
    {
        private readonly SqlConnectionsFactory _connectionsFactory;
        private readonly IConfiguration _configuration;
        public InitialMigration(SqlConnectionsFactory connectionsFactory, IConfiguration configuration)
        {
            _connectionsFactory = connectionsFactory;
            _configuration = configuration;
        }

        public async override Task Execute(CancellationToken cancellationToken)
        {
            await CreateDatabase(cancellationToken);
            await Initialize(cancellationToken);
        }

        private string GetDatabaseName(string defaultConnectionString)
        {
            return defaultConnectionString.Split(';')
                .FirstOrDefault(x => x.Contains("Database="))
                .Split('=')
                .LastOrDefault();
        }

        private async Task CreateDatabase(CancellationToken cancellationToken)
        {
            using var connection = _connectionsFactory.CreateConnection(_configuration.GetConnectionString("MigrationsConnection"));

            var databaseName = GetDatabaseName(_configuration.GetConnectionString("DefaultConnection"));

            await connection.OpenAsync(cancellationToken);
            try
            {
                var sql = @$"
                    CREATE DATABASE {databaseName}
                ";

                await connection.ExecuteAsync(sql, cancellationToken);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        private async Task Initialize(CancellationToken cancellationToken)
        {
            using var connection = _connectionsFactory.CreateConnection();

            await connection.OpenAsync(cancellationToken);
            try
            {
                var sql = @$"
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
