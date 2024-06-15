namespace MessagesExchange.Infrastructure.Database.Migrator
{
    /// <summary>
    /// Задача, запускаемая на старте приложения с целью миграции базы данных
    /// </summary>
    public class Migrator : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Migrator> _logger;
        public Migrator(IServiceProvider serviceProvider, ILogger<Migrator> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var migrationsService = _serviceProvider.GetRequiredService<IMigrationsService>();

                await migrationsService.Migrate();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Critical, ex, ex.Message);
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
