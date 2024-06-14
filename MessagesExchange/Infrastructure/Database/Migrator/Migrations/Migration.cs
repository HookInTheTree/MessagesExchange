namespace MessagesExchange.Infrastructure.Database.Migrator.Migrations
{
    public abstract class Migration
    {
        public abstract Task Execute(CancellationToken cancellationToken = default);
    }
}
