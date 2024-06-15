namespace MessagesExchange.Infrastructure.Database.Migrator
{
    /// <summary>
    /// Контракт для сервиса миграций
    /// </summary>
    public interface IMigrationsService
    {
        /// <summary>
        /// Метод для мигрирования базы данных
        /// </summary>
        /// <returns></returns>
        public Task Migrate();
    }
}
