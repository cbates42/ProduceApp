using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceApp
{
    internal class Produce
    {
        public string name;
        public string location;
        public float price;
        public string uom;
        public DateTime sellbydate;

        public Produce(string name, string location, float price, string uom, DateTime sellbydate)
        {
            this.name = name;
            this.location = location;
            this.price = price;
            this.uom = uom;
            this.sellbydate = sellbydate;
        }

    }
}
