namespace MessagesExchange.Infrastructure.Database.Migrator
{
    /// <summary>
    /// Модель данных миграции
    /// </summary>
    public class MigrationInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int SequentialNumber { get; set; }
    }
}
