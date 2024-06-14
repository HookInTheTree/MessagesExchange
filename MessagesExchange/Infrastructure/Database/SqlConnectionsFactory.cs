using Npgsql;
using System.Data.Common;

namespace MessagesExchange.Infrastructure.Database
{
    public class SqlConnectionsFactory
    {
        private readonly IConfiguration _configuration;
        public SqlConnectionsFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbConnection CreateConnection()
        {
            return new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public DbConnection CreateConnection(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
