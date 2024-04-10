using System.Data.SqlClient;

namespace backendPrueba.Data
{
    public class Connection
    {
        private readonly string _connectionString;

        public Connection(string value)
        {
            _connectionString = value;
        }

        public SqlConnection GetConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
