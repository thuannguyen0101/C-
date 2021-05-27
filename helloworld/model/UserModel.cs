using System;
using System.Runtime.InteropServices;
using helloworld.entity;
using helloworld.helper;
using helloworld.service;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Tls;

namespace helloworld.model
{
    public class UserModel
    {
        private ConnectionHelper connectionHelper = new ConnectionHelper();
        private Users _users = null;

        public void store(Users users)
        {
            MySqlConnection connection = connectionHelper.GetConnection();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            try
            {
                cmd.CommandText =
                    "INSERT INTO `users`(`fullName`, `email`, `password`, `phone`, `Salt`, `cardNumber`) VALUES (?fullName, ?email, ?password, ?phone, ?Salt, ?cardNumber)";
                cmd.Parameters.Add("?fullName", MySqlDbType.VarChar).Value = users.fullName;
                cmd.Parameters.Add("?email", MySqlDbType.VarChar).Value = users.email;
                cmd.Parameters.Add("?password", MySqlDbType.VarChar).Value = users.password;
                cmd.Parameters.Add("?phone", MySqlDbType.VarChar).Value = users.phone;
                cmd.Parameters.Add("?Salt", MySqlDbType.VarChar).Value = users.Salt;
                cmd.Parameters.Add("?cardNumber", MySqlDbType.VarChar).Value = users.cardNumber;
                cmd.ExecuteNonQuery();
                Console.WriteLine("Create user success");
            }
            catch (Exception e)
            {
                Console.WriteLine("Create user error");
                throw;
            }
        }

        public Users login(string account, string password)
        {
            Md5 md5 = new Md5();
            MySqlConnection connection = connectionHelper.GetConnection();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            try
            {
                string hashPassword = null;
                cmd.CommandText = $"SELECT * from users WHERE email ='{account}'";
                MySqlDataReader data = cmd.ExecuteReader();
                if (data.HasRows)
                {
                    data.Read();
                    hashPassword = md5.HashPassword(password, data["Salt"].ToString());
                    string pw = data["password"].ToString();
                    data.Close();
                    if (hashPassword.Equals(pw))
                    {
                        cmd.CommandText = $"SELECT * from users WHERE password ='{hashPassword}'";
                        MySqlDataReader dataUser = cmd.ExecuteReader();
                        while (dataUser.Read())
                        {
                            var fullName = dataUser["fullName"].ToString();
                            var email = dataUser["email"].ToString();
                            var phone = dataUser["phone"].ToString();
                            var cardNumber = dataUser["cardNumber"].ToString();
                            var balance = dataUser["balance"].ToString();
                            var salt = dataUser["Salt"].ToString();
                            var pass = dataUser["password"].ToString();
                            _users = new Users(fullName, email, pass, phone, cardNumber, balance, salt);
                        }

                        dataUser.Close();
                    }

                    return _users;
                }
                else
                {
                    data.Close();
                    return null;
                }
                
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error!");
                throw;
            }
        }

        public Users Recharges(string account, double money)
        {
            double sun = 0;
            MySqlConnection connection = connectionHelper.GetConnection();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            Transaction transaction = new Transaction();
            try
            {
                cmd.CommandText = $"SELECT * from users WHERE email ='{account}'";
                MySqlDataReader data = cmd.ExecuteReader();
                if (data.Read())
                {
                    sun = double.Parse(data["balance"].ToString()) + money;
                    data.Close();
                }

                if (sun != 0)
                {
                    cmd.CommandText = $"UPDATE `users` SET `balance`='{sun}' WHERE email ='{account}'";
                    MySqlDataReader a = cmd.ExecuteReader();
                    Console.WriteLine($"successful recharge {money}$ balance {sun}");
                    a.Close();
                }

                cmd.CommandText = $"SELECT * from users WHERE email ='{account}'";
                MySqlDataReader dataUser = cmd.ExecuteReader();
                while (dataUser.Read())
                {
                    var fullName = dataUser["fullName"].ToString();
                    var email = dataUser["email"].ToString();
                    var phone = dataUser["phone"].ToString();
                    var cardNumber = dataUser["cardNumber"].ToString();
                    var balance = dataUser["balance"].ToString();
                    var salt = dataUser["Salt"].ToString();
                    var pass = dataUser["password"].ToString();
                    _users = new Users(fullName, email, pass, phone, cardNumber, balance, salt);
                }
                dataUser.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error!");
                throw;
            }

            return _users;
        }

        public Users Withdrawal(string account, double money)
        {
            double sun = 0;
            MySqlConnection connection = connectionHelper.GetConnection();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            try
            {
                cmd.CommandText = $"SELECT * from users WHERE email ='{account}'";
                MySqlDataReader data = cmd.ExecuteReader();
                if (data.Read())
                {
                    sun = double.Parse(data["balance"].ToString()) - money;
                    data.Close();
                }
                cmd.CommandText = $"UPDATE `users` SET `balance`='{sun}' WHERE email ='{account}'";
                MySqlDataReader a = cmd.ExecuteReader();
                Console.WriteLine($"successful withdrawal {money}$ balance {sun}");
                a.Close();
                cmd.CommandText = $"SELECT * from users WHERE email ='{account}'";
                MySqlDataReader dataUser = cmd.ExecuteReader();
                while (dataUser.Read())
                {
                    var fullName = dataUser["fullName"].ToString();
                    var email = dataUser["email"].ToString();
                    var phone = dataUser["phone"].ToString();
                    var cardNumber = dataUser["cardNumber"].ToString();
                    var balance = dataUser["balance"].ToString();
                    var salt = dataUser["Salt"].ToString();
                    var pass = dataUser["password"].ToString();
                    _users = new Users(fullName, email, pass, phone, cardNumber, balance, salt);
                }
                dataUser.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error!");
                throw;
            }
            return _users;
        }
        public Users transfers(string account, double money , string receiver ,double sun)
        {
            double total = 0;
            MySqlConnection connection = connectionHelper.GetConnection();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            
            try
            {
                cmd.CommandText = $"SELECT * from users WHERE cardNumber ='{receiver}'";
                
                MySqlDataReader datareceiver = cmd.ExecuteReader();
                if (datareceiver.Read())
                {
                    Console.WriteLine($"are you sure you want to transfer the amount {money}$ Name {datareceiver["fullName"]} cardNumber {datareceiver["cardNumber"]}");
                    Console.WriteLine("choose ◉ y confirm");
                    Console.WriteLine("choose ◉ n cancel");
                    string choice = Console.ReadLine();
                    total =  double.Parse(datareceiver["balance"].ToString()) + money;
                    string U = datareceiver["cardNumber"].ToString();
                    datareceiver.Close();
                    switch (choice)
                    {
                        case "y":
                           
                                if (U != null)
                                {
                                    cmd.CommandText = $"UPDATE `users` SET `balance`='{total}' WHERE cardNumber ='{receiver}'";
                                    MySqlDataReader a = cmd.ExecuteReader();
                                    a.Close();
                                    cmd.CommandText = $"UPDATE `users` SET `balance`='{sun}' WHERE email ='{account}'";
                                    MySqlDataReader y = cmd.ExecuteReader();
                                    y.Close();
                                    cmd.CommandText = $"SELECT * from users WHERE email ='{account}'";
                                    MySqlDataReader dataUser = cmd.ExecuteReader();
                                    while (dataUser.Read())
                                    {
                                        var fullName = dataUser["fullName"].ToString();
                                        var email = dataUser["email"].ToString();
                                        var phone = dataUser["phone"].ToString();
                                        var cardNumber = dataUser["cardNumber"].ToString();
                                        var balance = dataUser["balance"].ToString();
                                        var salt = dataUser["Salt"].ToString();
                                        var pass = dataUser["password"].ToString();
                                        _users = new Users(fullName, email, pass, phone, cardNumber, balance, salt);
                                    }
                                    dataUser.Close();
                                }
                                datareceiver.Close();
                                break;
                        case "n":
                            Console.WriteLine("Cancel");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("\tWrong account number!");
                    return null;
                }
               
            }
            catch (Exception e)
            {
                Console.WriteLine("Error!");
                throw;
            }
            return _users;
        }
    }
}