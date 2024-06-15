using Npgsql;
using System.Data.Common;

namespace MessagesExchange.Infrastructure.Database
{
    /// <summary>
    /// Фабрика для создания подключений к базе данных
    /// </summary>
    public class SqlConnectionsFactory
    {
        private readonly IConfiguration _configuration;
        public SqlConnectionsFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Метод для создания дефолтного подключения к базе данных
        /// </summary>
        /// <returns></returns>
        public DbConnection CreateConnection()
        {
            return new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
        
        /// <summary>
        /// Метод для создания подключения к базе данных
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных</param>
        /// <returns></returns>
        public DbConnection CreateConnection(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
