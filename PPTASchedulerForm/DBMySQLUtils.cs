using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace PPTASchedulerForm
{
    public static class DBMySQLUtils
    {
        private static string connectionString { get; set; }

        public static void Init(string connString)
        {
            connectionString = connString;
        }

      
        private static MySqlConnection GetDBConnection()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            return connection;
        }

        public static int ExecuteQuery(string sql)
        {
            int rowsAffected = 0;
            MySqlConnection connection = null;

            try
            {
                connection = GetDBConnection();

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }

            return rowsAffected;
        }

        public static DataTable ExecuteStatement(string sql)
        {
            MySqlConnection connection = null;
            DataTable dataTable = new DataTable();

            try
            {
                connection = GetDBConnection();
                
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(command))
                    {
                        da.Fill(dataTable);
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }

            return dataTable;
        }

    }
}
