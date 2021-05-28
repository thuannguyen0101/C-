using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using helloworld.entity;
using helloworld.model;
using helloworld.service;

namespace helloworld.controller
{
    public class UserController
    {
        public void createUser()
        {
            UserModel userModel = new UserModel();
            Md5 md5 = new Md5();
            Random rnd = new Random();
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            Users users = new Users();
            double one = rnd.Next(1000, 9999);
            double two = rnd.Next(1000, 9999);
            double three = rnd.Next(1000, 9999);
            double four = rnd.Next(1000, 9999);
            double Salt = rnd.Next(1000, 9999);
            users.Salt = $"{Salt}";
            string cardNumber = $"{one}{two}{three}{four}";
            Console.WriteLine("Please enter info user\n");
            Console.WriteLine("--------------------------\n");
            Console.WriteLine("please enter full name:");
            users.fullName = Console.ReadLine();
            Console.WriteLine("please enter email:");
            users.email = Console.ReadLine();
            Console.WriteLine("please enter password:");
            string password = Console.ReadLine();
            Console.WriteLine("please enter phone:");
            users.phone = Console.ReadLine();
            users.cardNumber = cardNumber;
            users.password = md5.HashPassword(password, users.Salt);
            userModel.store(users);
        }

        public Users login()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            UserModel userModel = new UserModel();
            Console.WriteLine("please enter email:");
            string email = Console.ReadLine();
            Console.WriteLine("please enter password:");
            string password = Console.ReadLine();
            Users users = userModel.login(email, password);

            if (users != null)
            {
                Console.WriteLine("login success\n");
                return users;
            }
            else
            {
                Console.WriteLine("Login false");
                return null;
            }
        }

        public Users recharge(Users users)
        {
            Transaction transaction = new Transaction();
            TranModel tranModel = new TranModel();
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            UserModel userModel = new UserModel();
            Console.WriteLine("Please enter the amount to deposit");
            double money = double.Parse(Console.ReadLine());
            if (money > 0)
            {
                string account = users.email;
                Users _users = userModel.Recharges(account, money);
                transaction.cardNumber = _users.cardNumber;
                transaction.description =
                    $"you have successfully added to your account {_users.cardNumber} the amount is {money}$ balance {_users.balance}";
                transaction.transaction_type = "recharge";
                transaction.status = 1;
                tranModel.store(transaction);
                return _users;
            }
            else
            {
                Console.WriteLine("Amount cannot be a negative number");
                return users;
            }
        }

        public Users Withdrawal(Users users)
        {
            Transaction transaction = new Transaction();
            TranModel tranModel = new TranModel();
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            UserModel userModel = new UserModel();
            Console.WriteLine("Please enter the amount to withdraw");
            double money = double.Parse(Console.ReadLine());
            if (money > 0)
            {
                if (money > double.Parse(users.balance))
                {
                    Console.WriteLine("\tInsufficient balance in the account !");
                    return users;
                }
                else
                {
                    string account = users.email;
                    Users _users = userModel.Withdrawal(account, money);
                    transaction.cardNumber = _users.cardNumber;
                    transaction.description =
                        $"you have successfully withdrawal to your account {_users.cardNumber} the amount is {money}$ balance {_users.balance}";
                    transaction.transaction_type = "withdrawal";
                    transaction.status = 1;
                    tranModel.store(transaction);
                    return _users;
                }
            }
            else
            {
                Console.WriteLine(" Amount cannot be a negative number ");
                return users;
            }
        }

        public Users transfers(Users users)
        {
            Transaction transactionReceiver = new Transaction();
            Transaction transactionRemitters = new Transaction();
            TranModel tranModel = new TranModel();
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            UserModel userModel = new UserModel();
            Console.WriteLine("Please enter the account to transfers");
            string receiver = Console.ReadLine();
            Console.WriteLine("Please enter the amount to transfers");
            double money = double.Parse(Console.ReadLine());
            string remitters = users.email;
            double sun = double.Parse(users.balance) - money;
            if (money < double.Parse(users.balance) && money > 0)
            {
                Users _users = userModel.transfers(remitters, money, receiver, sun);
                if (_users == null)
                {
                    return users;
                }
                else
                {
                    // thong bao nguoi nhan
                    transactionReceiver.cardNumber = receiver;
                    transactionReceiver.description =
                        $"you get amount {money}$ from account number {users.cardNumber}";
                    transactionReceiver.transaction_type = "receive";
                    transactionReceiver.status = 1;
                    tranModel.store(transactionReceiver);
                    // thong bao nguoi gui
                    transactionRemitters.cardNumber = users.cardNumber;
                    transactionRemitters.description =
                        $"you have successfully transfer the amount {money}$ to account {receiver} your balance is {_users.balance}$";
                    transactionRemitters.transaction_type = "transfer";
                    transactionRemitters.status = 1;
                    tranModel.store(transactionRemitters);
                    return _users;
                }
            }
            else if (money < 0)
            {
                Console.WriteLine("Amount cannot be a negative number");
                return users;
            }
            else
            {
                Console.WriteLine("\tInsufficient balance in the account !");
                return users;
            }
        }

        public void TransactionHistory(Users users)
        {
            TranModel tranModel = new TranModel();
            string cardNumber = users.cardNumber;
            List<Transaction> list = tranModel.TransactionHistory(cardNumber);

            Console.WriteLine("Transaction type \t\t Status \t\t\t Create At \t\t\t\t Description");
            foreach (Transaction transaction in list)
            {
                string a = null;
                if (transaction.status == 1)
                {
                    a = "success";
                }
                else
                {
                    a = "cancel";
                }

                Console.WriteLine(
                    $" {transaction.transaction_type}\t\t\t{a}  \t\t\t{transaction.created_At}\t\t{transaction.description}");
            }
        }
    }
}