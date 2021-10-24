namespace ThreadingBookHomework2
{
    using System;
    using System.Threading;

    class Program
    {
        static int items = 1000;
        static object objLock = new object();

        static void Buyer()
        {
            Random rand = new Random();
            for (int i = 0; i < 90; i++)
            {
                Monitor.Enter(objLock);
                var boughtItems = rand.Next(1, 20);
                items -= boughtItems;
                Console.WriteLine($"{boughtItems} items bought.");
                Console.WriteLine(new string('=', 60));
                Thread.Sleep(3);
                // items--;
                Monitor.Exit(objLock);
            }
        }

        static void Supplier()
        {
            Random rand = new Random();
            for (int i = 0; i < 5; i++)
            {
                Monitor.Enter(objLock);
                var addedItems = rand.Next(1, 20);
                items += addedItems;
                // items += 15;
                Console.WriteLine($"{addedItems} items restocked.");
                Console.WriteLine(new string('=',60));
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
            Console.WriteLine(items);
        }
    }
}
