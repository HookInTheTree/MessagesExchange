using MessagesExchange.Infrastructure.Database.Migrator.Migrations;

namespace MessagesExchange.Infrastructure.Database.Migrator
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
            List<MigrationInfo> executedMigrations;
            List<Migration> migrationsToExecute;

            try
            {
                executedMigrations = await _repository.GetMigrations();
                migrationsToExecute = _migrations.Where(m => !executedMigrations.Any(em => em.Name != m.GetType().Name))
                    .ToList();
            }
            catch
            {
                migrationsToExecute = _migrations.ToList();
            }

            foreach (var migration in migrationsToExecute)
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
