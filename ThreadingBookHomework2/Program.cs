namespace ThreadingBookHomework2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using ThreadingShopHomework;

    class Program
    {
        static List<Item> items = new List<Item>()
        {
            new Item() {Name = "Book", Quantity = 200, Id = 1},
            new Item() {Name = "Chair", Quantity = 200, Id = 2},
            new Item() {Name = "Laptop", Quantity = 200, Id = 3},
            new Item() {Name = "Computer", Quantity = 400, Id = 4},
            new Item() {Name = "Phone", Quantity = 200, Id = 5},
        };
        static object objLock = new object();

        static void Buyer()
        {
            Random rand = new Random();
            for (int i = 0; i < 90; i++)
            {
                Monitor.Enter(objLock);

                var quantityToBuy = rand.Next(1, 20);
                var idOfItem = rand.Next(1, 6);

                var item = items.Where(x => x.Id == idOfItem).FirstOrDefault();
                if (item.Quantity - quantityToBuy < 0)
                {
                    var quantity = item.Quantity == 0 ? 0 : item.Quantity;
                    Console.WriteLine($"Not enough quantity. We have {quantity} left from {item.Name}.");
                    Console.WriteLine(new string('=', 60));
                    continue;
                }

                item.Quantity -= quantityToBuy;

                Console.WriteLine($"{quantityToBuy} items bought from {item.Name}.");
                Console.WriteLine(new string('=', 60));

                Thread.Sleep(3);
                Monitor.Exit(objLock);
            }
        }

        static void Supplier()
        {
            Random rand = new Random();
            for (int i = 0; i < 5; i++)
            {
                Monitor.Enter(objLock);
                var itemsToSupply = rand.Next(1, 20);

                var idOfItem = rand.Next(1, 6);

                var item = items.Where(x => x.Id == idOfItem).FirstOrDefault();
                item.Quantity += itemsToSupply;

                Console.WriteLine($"{itemsToSupply} items restocked from {item.Name}.");
                Console.WriteLine(new string('=', 60));

                Thread.Sleep(3);
                Monitor.Exit(objLock);
            }
        }

        static void Main()
        {
            var threadBuyer = new Thread(Buyer);
            var threadSupplier = new Thread(Supplier);

            threadBuyer.Start();
            threadSupplier.Start();

            threadBuyer.Join();
            threadSupplier.Join();

            foreach (var item in items)
            {
                Console.WriteLine(item.Name + ": " + item.Quantity);
            }
        }
    }
}
