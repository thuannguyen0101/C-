using System;
using System.Collections.Generic;
using helloworld.entity;
using helloworld.helper;
using MySql.Data.MySqlClient;

namespace helloworld.model
{
    public class TranModel
    {
        private ConnectionHelper connectionHelper = new ConnectionHelper();
        public void store (Transaction transaction)
        {
            MySqlConnection connection = connectionHelper.GetConnection();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            try
            {
                cmd.CommandText =
                    "INSERT INTO `transaction`(`cardNumber`, `discription`, `status`, `transaction_type`) VALUES (?cardNumber, ?discription, ?status, ?transaction_type)";
                cmd.Parameters.Add("?cardNumber", MySqlDbType.VarChar).Value = transaction.cardNumber;
                cmd.Parameters.Add("?discription", MySqlDbType.VarChar).Value = transaction.description;
                cmd.Parameters.Add("?status", MySqlDbType.VarChar).Value = transaction.status;
                cmd.Parameters.Add("?transaction_type", MySqlDbType.VarChar).Value = transaction.transaction_type;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("error");
                throw;
            }
        }
        public List<Transaction> TransactionHistory (string cardNumber)
        {
            
            MySqlConnection connection = connectionHelper.GetConnection();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            List<Transaction> transactionslist = new List<Transaction>();
            try
            {
                cmd.CommandText =
                    $"SELECT * FROM `transaction` WHERE cardNumber = '{cardNumber}'";
                MySqlDataReader data= cmd.ExecuteReader();
                while (data.Read())
                {
                    Transaction transaction = new Transaction(data["cardNumber"].ToString(), data["discription"].ToString(), int.Parse(data["status"].ToString()), data["created_At"].ToString(),data["transaction_type"].ToString());
                    transactionslist.Add(transaction);
                }
                data.Close();
                return transactionslist;
            }
            catch (Exception e)
            {
                Console.WriteLine("error");
                throw;
            }
        }
    }
}