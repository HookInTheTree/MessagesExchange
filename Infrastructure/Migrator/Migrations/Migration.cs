namespace MessagesExchange.Infrastructure.Migrations.DatabaseMigrations
{
    public abstract class Migration
    {
        public abstract Task Execute(CancellationToken cancellationToken = default);
    }
}
