using System;
using System.Text;
using helloworld.controller;
using helloworld.entity;

namespace helloworld.view
{
    public class Menu
    {
        public void MenusBank()
        {
            UserController userController = new UserController();
            Users users = null;

            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            Console.WriteLine("\tWelcome to spring-hero-bank by Thuận Nguyễn");
            while (true)
            {
                if (users == null)
                {
                    Console.WriteLine("please enter choice (1-3)\n");
                    Console.WriteLine("-1: Create new account");
                    Console.WriteLine("-2: Login account");
                    Console.WriteLine("-3: Exit");
                    int a = int.Parse(Console.ReadLine());
                    if (a == 1 || a == 2 || a == 3)
                    {
                        switch (a)
                        {
                            case 1:
                                Console.WriteLine("Create new account\n");
                                userController.createUser();
                                break;
                            case 2:
                                Console.WriteLine("Login account\n");
                                users = userController.login();
                                break;
                        }

                        if (a == 3)
                        {
                            Console.WriteLine("bey bey");
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter options(1-3)");
                    }
                }
                else
                {
                    Console.WriteLine($"\tWelcome: {users.fullName}");
                    Console.WriteLine($"\tBalance: {users.balance}$");
                    Console.WriteLine("-1: Transfers");
                    Console.WriteLine("-2: Recharge");
                    Console.WriteLine("-3: Withdrawal");
                    Console.WriteLine("-4: Transaction history");
                    Console.WriteLine("-5: Logout");
                    int b = int.Parse(Console.ReadLine());
                    if (b != null)
                    {
                        switch (b)
                        {
                            case 1:
                                users = userController.transfers(users);
                                break;
                            case 2:
                                users = userController.recharge(users);
                                break;
                            case 3:
                                users = userController.Withdrawal(users);
                                break;
                            case 4:
                                userController.TransactionHistory(users);
                                break;
                            case 5:
                                users = null;
                                break;
                        }
                    }
                }
            }
        }
    }
}