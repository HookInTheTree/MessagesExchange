namespace MessagesExchange.Infrastructure.Database.Migrator.Migrations
{
    /// <summary>
    /// Абстрактный класс, определяющий семейство миграций
    /// </summary>
    public abstract class Migration
    {
        /// <summary>
        /// Метод для применения миграции
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public abstract Task Execute(CancellationToken cancellationToken = default);
    }
}
