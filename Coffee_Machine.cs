using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading;

namespace CoffeeMachine
{
    class Coffee_Machine
    {
        public Buyer buyer;
        public List<Coffee_Type> Manu;

        public Coffee_Machine()
        {
            buyer = new Buyer();
            Manu = new List<Coffee_Type>();
        }

        public void StartWorking()
        {
            _Manu.ReadManuFromDB(Manu);
            while (true)
            {
                string readKey;
                //Console.WriteLine("Press 1 to Insert coins press 0 to  end");
                Console.WriteLine($"YOUR BALANCE - {buyer.Money}$" +
                    $"\nPRESS...\n" +
                    $"1 - TO GO TO THE MANU.\n" +
                    $"2 - TO GET CHANGE.\n" +
                    $"3 - TO INSERT COINS.\n" +
                    $"4 - TO ORDER COFFEE.");

                readKey = Console.ReadKey().KeyChar.ToString().ToLower();
                switch (readKey)
                {
                    case ("1"):
                        Console.Clear();
                        _Manu.PrintManu(Manu);
                        break;
                    case ("2"):
                        Console.Clear();
                        BusinessLogic.ReturningChange(buyer);
                        break;
                    case ("3"):
                        Console.Clear();
                        CoinInserter();
                        Console.Clear();
                        break;
                    case ("4"):
                        Console.Clear();
                        ChooseCoffee();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid entry try again");
                        Thread.Sleep(1500);
                        Console.Clear();
                        break;
                }
            }
        }
        public void CoinInserter ()
        {
            bool InsertionComplete = false;

            do
            {
                Console.WriteLine($"YOUR BALANCE - {buyer.Money}$\n" +
                    $"Select coin...\n1 - 50$\n2 - 100$\n3 - 200$\n4 - 500$\n" +
                    $"Press M to go to the MAIN MANU.\n" +
                    $"Press O to ORDER COFFEE.");
                string CoinReadKey = Console.ReadKey().KeyChar.ToString().ToLower();
                if (CoinReadKey == "o")
                {
                    Console.Clear();
                    ChooseCoffee();
                    break;
                }
                else if(CoinReadKey == "m")
                {
                    break;
                }
                BusinessLogic.Deposit(CoinReadKey,
                    ref buyer, ref InsertionComplete);

            } while (InsertionComplete == false);
        }
        public void ChooseCoffee()
        {
            List<Coffee_Type> AllCoffeeForThisPrice = new List<Coffee_Type>();
            AllCoffeeForThisPrice = _Manu.Check(Manu,buyer);
            if (AllCoffeeForThisPrice.Count != 0)
            {
                while (true)
                {
                    Console.WriteLine($"YOUR BALANCE - { buyer.Money}$\n" +
                        $"Press 0 to go to the MAIN MANU.\n\n" +
                        $"Here is all the coffee for this price!\n" +
                        $"Enter the number of coffee\n");
                    _Manu.PrintManu(AllCoffeeForThisPrice);
                    int selectCoffee;
                    try
                    {
                        selectCoffee = int.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        throw new Exception();
                    }
                    if (selectCoffee > 0 && selectCoffee <= AllCoffeeForThisPrice.Count)
                    {
                        MakeCoffee(AllCoffeeForThisPrice[selectCoffee - 1]);
                        break;
                    }
                    else if(selectCoffee == 0)
                    {
                        break;
                    }
                    Console.Clear();
                    Console.WriteLine("Invalid entry try again");
                    Thread.Sleep(1500);
                    Console.Clear();
                }
            }
            else
            {
                Console.WriteLine("TOO LITTLE MONEY.");
                Thread.Sleep(1500);
                Console.Clear();
            }
        }
        public void MakeCoffee(Coffee_Type selectCoffee)
        {
            string connectionstring = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                string selectQuerry = "select * from Ingredients";

                connection.Open();
                SqlCommand command = new SqlCommand(selectQuerry, connection);
                SqlDataReader reader = command.ExecuteReader();
                Storage storage = null;

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        storage = new Storage(
                            (double)reader["coffee"],
                            (double)reader["water"],
                            (double)reader["sugar"]);
                    }
                }
                else
                {
                    throw new Exception();
                }

                reader.Close();

                if (selectCoffee.coffee_dose <= storage.coffee &&
                    selectCoffee.water_dose <= storage.water && 
                    selectCoffee.sugar_dose <= storage.sugar)
                {
                    storage.coffee -= selectCoffee.coffee_dose;
                    storage.water -= selectCoffee.water_dose;
                    storage.sugar -= selectCoffee.sugar_dose;

                    string updateQuerry =
                                            $"update Ingredients set water = {storage.water}," +
                                            $"sugar = {storage.sugar}," +
                                            $"coffee = {storage.coffee}";

                    SqlCommand com = new SqlCommand(updateQuerry, connection);

                    com.ExecuteNonQuery();
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("Your coffee is complet!\n" +
                           "Press 1 to take the change\n" +
                           "Press 2 to go to the MAIN MANU");
                        string PressKey = Console.ReadKey().KeyChar.ToString().ToLower();
                        switch (PressKey)
                        {
                            case ("1"):
                                Console.Clear();
                                BusinessLogic.RemovePriceFromMoney(selectCoffee, buyer);
                                BusinessLogic.ReturningChange(buyer);
                                return;
                            case ("2"):
                                Console.Clear();
                                BusinessLogic.RemovePriceFromMoney(selectCoffee, buyer);
                                return;
                            default:
                                Console.Clear();
                                Console.WriteLine("Invalid entry try again");
                                break;
                        }
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Coffee machine is out of ingredients");
                    Thread.Sleep(1500);
                    Console.Clear();
                }
            }
        }
    }
}