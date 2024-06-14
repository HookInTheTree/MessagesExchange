namespace MessagesExchange.Infrastructure.Database.Migrator
{
    public interface IMigrationsRepository
    {
        Task CreateMigrationInfo(MigrationInfo migrationInfo);
        Task<List<MigrationInfo>> GetMigrations();
    }
}
