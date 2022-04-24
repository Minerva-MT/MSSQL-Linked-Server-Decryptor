using System.Data.SqlClient;
using System.Data;
using System;

namespace SQL_Decryptor
{
    class Database 
    {
        private SqlConnection Connection;
        public Database(string ConnectionString)
        {
            try
            {
                Connection = new SqlConnection(ConnectionString);
                Connection.Open();
            }
            catch (Exception E)
            {
                Console.WriteLine("Error Connecting to Database: {0}", E.Message);
            }
        }

        public DataTable Select_DataTable(string Query)
        {
            SqlDataAdapter Command = new SqlDataAdapter(Query, Connection);
            DataTable dataTable = new DataTable();

            try
            {
                Command.Fill(dataTable);
            }
            catch (Exception)
            {
                Console.WriteLine("Error Querying Database");
                return null;
            }

            return dataTable;            
        }

        public object Select(string Query)
        {
            SqlCommand Command = new SqlCommand(Query, Connection);
            try 
            {
                return Command.ExecuteScalar();
            } 
            catch(Exception)
            {
                Console.WriteLine("Error Querying Database");
                return null;
            }
        }

        public bool Close()
        {
            if (Connection.State == System.Data.ConnectionState.Open)
                Connection.Close();

            return true;
        }
    }
}
