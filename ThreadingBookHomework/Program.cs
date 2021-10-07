namespace ThreadingBookHomework
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            string book = File.ReadAllText(@"../../../BraveNewWorld.txt");
            string[] splitedBook = book.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            Stopwatch noThreadStopwatch = new Stopwatch();
            // if I use Replace it still counts characters wrong, however the logic in the method is right
            // book = book.Replace("\n", "");
            noThreadStopwatch.Start();

            NumberOfWords(splitedBook);
            ShortestWord(splitedBook);
            LongestWord(splitedBook);
            AverageWordLength(splitedBook);
            TopFiveMostCommonWords(splitedBook);
            TopFiveLeastCommonWords(splitedBook);

            noThreadStopwatch.Stop();

            Console.WriteLine("Non-threaded time: " + noThreadStopwatch.ElapsedMilliseconds);
            Console.WriteLine(new string('=',30));

            Stopwatch threadStopwatch = new Stopwatch();

            threadStopwatch.Start();

            var numberOfWords = new Thread(() => NumberOfWords(splitedBook));
            numberOfWords.Start();
            var shortestWord = new Thread(() => ShortestWord(splitedBook));
            shortestWord.Start();
            var longestWord = new Thread(() => LongestWord(splitedBook));
            longestWord.Start();
            var averageWordLength = new Thread(() => AverageWordLength(splitedBook));
            averageWordLength.Start();
            var  topFiveMostCommonWords = new Thread(() => TopFiveMostCommonWords(splitedBook));
            topFiveMostCommonWords.Start();
            var topFiveLeastCommonWords  = new Thread(() => TopFiveLeastCommonWords(splitedBook));
            topFiveLeastCommonWords.Start();

            threadStopwatch.Stop();

            Console.WriteLine("Threaded time: " + threadStopwatch.ElapsedMilliseconds);
        }

        static void NumberOfWords(string[] splitedBook)
        {
            Console.WriteLine($"Words count: {splitedBook.Length}");
        }

        static void ShortestWord(string[] splitedBook)
        {
            Console.WriteLine($"Shortest word: {splitedBook.OrderBy(x => x.Length).FirstOrDefault()}");
        }

        static void LongestWord(string[] splitedBook)
        { 
            Console.WriteLine($"Longest word: {splitedBook.OrderByDescending(x => x.Length).FirstOrDefault()}");
        }

        static void AverageWordLength(string[] splitedBook)
        {
            Console.WriteLine($"Average word length: {splitedBook.Average(x=>x.Length):F2}");
        }

        static void TopFiveMostCommonWords(string[] splitedBook)
        {
            var dict = new Dictionary<string, int>();
            foreach (var word in splitedBook)
            {
                if (!dict.ContainsKey(word))
                {
                    dict.Add(word, 1);
                }
                else
                {
                    dict[word] += 1;
                }
            }
            var fiveWords = dict.OrderByDescending(x => x.Value).Take(5).ToList();
            Console.WriteLine($"Top five most common words: ");
            foreach (var w in fiveWords)
            {
                Console.WriteLine($"{w.Key}");
            }
        }

        static void TopFiveLeastCommonWords(string[] splitedBook)
        {
            var dict = new Dictionary<string, int>();
            foreach (var word in splitedBook)
            {
                if (!dict.ContainsKey(word))
                {
                    dict.Add(word, 1);
                }
                else
                {
                    dict[word] += 1;
                }
            }
            var fiveWords = dict.OrderBy(x => x.Value).Take(5).ToList();
            Console.WriteLine($"Top five least common words: ");
            foreach (var w in fiveWords)
            {
                Console.WriteLine($"{w.Key}");
            }
        }
    }
}
