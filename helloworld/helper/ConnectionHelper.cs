using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace helloworld.helper
{
    public class ConnectionHelper
    {
        private static string Uid = "root";
        private static string password = "";
        private static string ServerPort = "3306";
        private static string ServerName = "127.0.0.1";
        private static string DatabaseName = "spring-hero-bank";
        
        private static readonly string connStr = $"server={ServerName};user={Uid};database={DatabaseName};port={ServerPort};password={password}";

        public ConnectionHelper()
        {
        }

        public static MySqlConnection _Connection = null;

        public  MySqlConnection GetConnection()
        {
            if (_Connection == null)
            {
               
                _Connection = new MySqlConnection(connStr);
                _Connection.Open(); 
            }
            else if (_Connection.State == ConnectionState.Closed)
            {
                Console.WriteLine("Old Connection Mysql ");
                _Connection.Open();
            }
            return _Connection;
        } 
    }
}