namespace MessagesExchange.Infrastructure.Migrations
{
    public interface IMigrationsRepository
    {
        public Task CreateMigrationInfo(MigrationInfo migrationInfo);
    }
}
