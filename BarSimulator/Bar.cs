namespace BarSimulator
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;

    class Bar
    {
        List<Student> students = new List<Student>();
        public List<Drink> drinks = new List<Drink>()
        {
            new Drink(1,"Water",1,100),
            new Drink(2,"Cola",2,100),
            new Drink(3,"RedBull",5,50),
            new Drink(4,"Beer",4,100),
        };
        Semaphore semaphore = new Semaphore(10, 10);

        public void Enter(Student student)
        {
            semaphore.WaitOne();
            lock (students)
            {
                students.Add(student);
            }
        }

        public void Leave(Student student)
        {
            lock (students)
            {
                students.Remove(student);
            }
            semaphore.Release();
        }

        // This method doesn't work. I tried to use lock and it still throws exception.
        public void Close()
        {
            SalesReport();
            foreach (var student in students)
            {
                Leave(student);
            }
            Console.WriteLine("Bar closed.");
        }
        private void SalesReport()
        {
            foreach (var drink in drinks)
            {
                Console.WriteLine($"{drink.SoldDrinks} {drink.Name} sold and {drink.MoneyEarned} money earned.");
            }
        }
    }
}
