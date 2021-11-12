namespace BarSimulator
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Drink
    {
        public Drink(int id,string name, int price, int quantity)
        {
            this.Id = id;
            this.Name = name;
            this.Price = price;
            this.Quantity = quantity;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; }

        public int SoldDrinks { get; set; }

        public int MoneyEarned { get; set; }
    }
}
