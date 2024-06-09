
using MessagesExchange.Infrastructure.Migrations;

namespace MessagesExchange.Infrastructure.Migrator
{
    public class Migrator : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        public Migrator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
