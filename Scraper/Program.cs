namespace Scraper
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using AngleSharp;
    using System.Threading.Tasks;

    class Program
    {
        static void Main()
        {
            var productLinks = File.ReadAllText(@"../../../products.txt");

            var splitedProductLinks = productLinks.Split("\r\n");

            var tasks = new List<Task<string>>();

            foreach (var product in splitedProductLinks)
            {
                var task = Task.Run(() => GetProductPrice(product));
                task.Wait();
                tasks.Add(task);
            }

            var fanInTasks = Task.WhenAll(tasks);
            var productPrices = fanInTasks.ContinueWith(prev => prev.Result);
            productPrices.Wait();
            // Printing the result from the array
            for (int i = 0; i < productPrices.Result.Length; i++)
            {
                Console.WriteLine(productPrices.Result[i]);
            }

        }
        public static async Task<string> GetProductPrice(string productLink)
        {
            // Using AngleSharp to make the scraping
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            
            // Opens the url
            var document = await context.OpenAsync(productLink);

            // Finding the price span and getting the value
            var productPrice = document.QuerySelector(".price").TextContent.TrimStart('\n');

            return productPrice;
        }
    }
}
