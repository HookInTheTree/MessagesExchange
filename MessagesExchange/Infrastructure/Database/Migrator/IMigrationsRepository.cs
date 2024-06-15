namespace MessagesExchange.Infrastructure.Database.Migrator
{
    /// <summary>
    /// Контракт репозитория миграций
    /// </summary>
    public interface IMigrationsRepository
    {
        /// <summary>
        /// Метод для создания записи о миграции
        /// </summary>
        /// <param name="migrationInfo"></param>
        /// <returns></returns>
        Task CreateMigrationInfo(MigrationInfo migrationInfo);
        
        /// <summary>
        /// Метод для получения записей о миграциях
        /// </summary>
        /// <returns></returns>
        Task<List<MigrationInfo>> GetMigrations();
    }
}
