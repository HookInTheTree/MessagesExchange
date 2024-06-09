
namespace MessagesExchange.Infrastructure.Migrations
{
    public interface IMigrationsRepository
    {
        Task CreateMigrationInfo(MigrationInfo migrationInfo);
        Task<List<MigrationInfo>> GetMigrations();
    }
}
