namespace BarSimulator
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            Bar bar = new Bar();
            Random random = new Random();
            List<Thread> studentThreads = new List<Thread>();
            for (int i = 1; i < 30; i++)
            {
                    var age = random.Next(12, 60);
                    var student = new Student(i.ToString(), bar,30,age);           
                    var thread = new Thread(student.PaintTheTownRed);
                    thread.Start();
                    studentThreads.Add(thread);
            }

            foreach (var t in studentThreads) t.Join();
            Console.WriteLine();
            Console.WriteLine("The party is over.");
            Console.ReadLine();
        }
    }
}
