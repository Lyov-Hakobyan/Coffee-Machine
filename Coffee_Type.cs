using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachine
{
    class Coffee_Type
    {
        public int ID { get; set; }
        public string coffee_name { get; set; }
        public double coffee_dose { get; set; }
        public double water_dose { get; set; }
        public double sugar_dose { get; set; }
        public int price { get; set; }

        public Coffee_Type(int id,string name, double coffee, double water, double sugar,int price)
        {
            this.ID = id;
            this.coffee_name = name;
            this.coffee_dose = coffee;
            this.water_dose = water;
            this.sugar_dose = sugar;
            this.price = price;
        }
    }
}