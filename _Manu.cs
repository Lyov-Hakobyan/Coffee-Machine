using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace CoffeeMachine
{
    static class _Manu
    {
        public static void ReadManuFromDB(List<Coffee_Type> manu)
        {
            string connectionstring =
                ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                string command = "select * from Coffee_Manu";

                connection.Open();

                SqlCommand com = new SqlCommand(command, connection);

                SqlDataReader reader = com.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Coffee_Type data = new Coffee_Type(
                            (int)reader["ID"],
                            (string)reader["coffee_name"],
                            (double)reader["coffee_dose"],
                            (double)reader["water_dose"],
                            (double)reader["sugar_dose"],
                            (int)reader["price"]);

                        manu.Add(data);
                    }
                }
                reader.Close();
            }
        }
        public static void PrintManu(List<Coffee_Type> manu)
        {
            for (int i = 0; i < manu.Count; i++)
            {
                Console.WriteLine(getCofee(i,manu));
            }
            Console.WriteLine();
        }
        public static string getCofee(int i,List<Coffee_Type> manu)
        {
            return 
                manu[i].ID + ":" +
                manu[i].coffee_name + "  (" + 
                manu[i].price +"$  )";
        }
        public static List<Coffee_Type> Check(List<Coffee_Type> manu, Buyer buyer)
        {
            List<Coffee_Type> AllCoffeeForThisPrice = new List<Coffee_Type>();

            for (int i = 0,j = 1; i < manu.Count; i++)
            {
                if (buyer.Money >= manu[i].price)
                {
                    AllCoffeeForThisPrice.Add(new Coffee_Type(
                        j++,
                        manu[i].coffee_name, 
                        manu[i].coffee_dose,
                        manu[i].water_dose, 
                        manu[i].sugar_dose, 
                        manu[i].price));
                }
            }
            return AllCoffeeForThisPrice;
        }
    }
}
