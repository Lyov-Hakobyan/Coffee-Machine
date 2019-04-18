using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachine
{
    class Storage
    {
        public double coffee;
        public double water;
        public double sugar;

        public Storage(double coffee, double water, double sugar)
        {
            this.coffee = coffee;
            this.water = water;
            this.sugar = sugar;
        }
    }
}
