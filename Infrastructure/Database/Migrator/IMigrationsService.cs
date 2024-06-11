namespace MessagesExchange.Infrastructure.Database.Migrator
{
    public interface IMigrationsService
    {
        public Task Migrate();
    }
}
