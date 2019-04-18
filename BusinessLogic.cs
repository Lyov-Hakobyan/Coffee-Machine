using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading;

namespace CoffeeMachine
{
    static class BusinessLogic
    {
        public static void Deposit(string CoinAmount, ref Buyer buyer,ref bool InsertionComplete)
        {
            switch (CoinAmount)
            {
                case ("1"):
                    Console.Clear();
                    buyer.Money += 50;
                    break;
                case ("2"):
                    Console.Clear();
                    buyer.Money += 100;
                    break;
                case ("3"):
                    Console.Clear();
                    buyer.Money += 200;
                    break;
                case ("4"):
                    Console.Clear();
                    buyer.Money += 500;
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid entry try again");
                    Thread.Sleep(1500);
                    Console.Clear();
                    break;
            }
        }

        public static void RemovePriceFromMoney(Coffee_Type selectCoffee, Buyer buyer)
        {
            buyer.Money -= selectCoffee.price;
        }
        
        
        public static void ReturningChange(Buyer buyer)
        {
            if(buyer.Money == 0)
            {
                Console.WriteLine("NO CHANGE!");
                Thread.Sleep(2000);
                Console.Clear();
                return;
            }
            Console.WriteLine($"THANK YOU!\nYOUR CHANGE - {buyer.Money}$");
            buyer.Money = 0;
            Thread.Sleep(2000);
            Console.Clear();
            
        }
    }
}