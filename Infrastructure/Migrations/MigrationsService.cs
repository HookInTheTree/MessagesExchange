
using Dapper;
using MessagesExchange.Infrastructure.Migrations.DatabaseMigrations;
using Microsoft.AspNetCore.Connections;
using Npgsql;

namespace MessagesExchange.Infrastructure.Migrations
{
    public class MigrationsService : IMigrationsService
    {
        private readonly IEnumerable<Migration> _migrations;
        private readonly IMigrationsRepository _repository;

        public MigrationsService(IMigrationsRepository repo, IEnumerable<Migration> migrations)
        {
            _repository = repo;
            _migrations = migrations;
        }

        public async Task Migrate()
        {
            foreach (var migration in _migrations)
            {
                await migration.Execute();
                await _repository.CreateMigrationInfo(new MigrationInfo()
                {
                    Id = new Guid(),
                    Name = migration.GetType().Name
                });
            }
        }
    }
}
