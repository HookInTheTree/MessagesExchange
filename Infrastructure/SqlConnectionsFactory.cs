using Npgsql;
using System.Data.Common;

namespace MessagesExchange.Infrastructure
{
    public static class SqlConnectionsFactory
    {
        public static DbConnection CreateConnection(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
