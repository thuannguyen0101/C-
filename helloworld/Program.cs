using System;
using System.Collections.Concurrent;
using System.Text;
using helloworld.controller;
using helloworld.entity;
using helloworld.helper;
using helloworld.model;
using helloworld.service;
using helloworld.view;
using MySql.Data.MySqlClient;

namespace helloworld
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Menu menu = new Menu();
            menu.MenusBank();
        }
    }
}