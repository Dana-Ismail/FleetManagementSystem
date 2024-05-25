using System;
using Npgsql;

namespace FMSCore
{
    public class DatabaseConnection
    {
    
        private const string _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=danaavocado;Database=DanaIsmail_FMS";
        public DatabaseConnection()
        {
         //   _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabase"]?.ConnectionString;
        }

        public NpgsqlConnection OpenConnection()
        {
            var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            Console.WriteLine("connected successfully");
            return connection;
        }
        public void ExecuteNonQuery(string query)
        {
            using (var connection = OpenConnection())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
                Console.WriteLine("success");
            }
        }

        public NpgsqlDataReader ExecuteReader(string query)
        {
            var connection = OpenConnection();
            var command = new NpgsqlCommand(query, connection);
            Console.WriteLine("success");
            return command.ExecuteReader();
        }


    }
}
