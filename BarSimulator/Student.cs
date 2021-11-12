namespace BarSimulator
{
    using System;
    using System.Linq;
    using System.Threading;

    class Student
    {
        volatile bool isClosed = false;

        enum NightlifeActivities { Walk, VisitBar, GoHome };
        enum BarActivities { Drink, Dance, Leave };

        Random random = new Random();

        public string Name { get; set; }

        public int Age { get; set; }

        public Bar Bar { get; set; }

        public int Budget { get; set; }

        private NightlifeActivities GetRandomNightlifeActivity()
        {
            int n = random.Next(10);
            if (n < 3) return NightlifeActivities.Walk;
            if (n < 8) return NightlifeActivities.VisitBar;
            return NightlifeActivities.GoHome;
        }

        private BarActivities GetRandomBarActivity()
        {
            int n = random.Next(10);
            if (n < 4) return BarActivities.Dance;
            if (n < 9) return BarActivities.Drink;
            return BarActivities.Leave;
        }
        // This method doesn't work.I tried to use volatile and nothing changed. More than one thread go inside.
        
        private void CheckIfBarClosed()
        {
            if (!isClosed)
            {
                int n = random.Next(10);
                if (n == 9)
                {
                    isClosed = true;
                  //  Console.WriteLine("Bool set to true");
                }
            }
        }

        // Checks if a student can buy a random drink
        private bool BuyDrink()
        {
            if (Budget < 0)
            {
                Console.WriteLine("Not enough money.");
                return false;
            }
            var drinkId = random.Next(1, 5);
            var drink = Bar.drinks.FirstOrDefault(x => x.Id == drinkId);

            if (drink.Quantity <= 0)
            {
                Console.WriteLine($"{drink.Name} is not available.");
                return false;
            }

            if (drink.Price > Budget)
            {
                Console.WriteLine($"Not enough money.The {drink.Name} costs {drink.Price}.");
                return false;
            }

            Budget -= drink.Price;
            drink.Quantity--;
            drink.SoldDrinks++;
            drink.MoneyEarned += drink.Price;

            return true;
        }

        private void WalkOut()
        {
            Console.WriteLine($"{Name} is walking in the streets.");
            Thread.Sleep(100);
        }

        private void VisitBar()
        {
            CheckIfBarClosed();
            if (!isClosed)
            {
                if (Age > 18)
                {
                    Console.WriteLine($"{Name} is getting in the line to enter the bar.");
                    // leaves randomly
                    if (random.Next(0, 10) == 6)
                    {
                        Console.WriteLine($"{Name} decided to leave.");
                        return;
                    }
                    Bar.Enter(this);
                    Console.WriteLine($"{Name} entered the bar!");
                    bool staysAtBar = true;
                    while (staysAtBar)
                    {
                        var nextActivity = GetRandomBarActivity();
                        switch (nextActivity)
                        {
                            case BarActivities.Dance:
                                Console.WriteLine($"{Name} is dancing.");
                                Thread.Sleep(100);
                                break;
                            case BarActivities.Drink:
                                var result = BuyDrink();
                                if (!result) break;
                                Console.WriteLine($"{Name} is drinking.");
                                Thread.Sleep(100);
                                break;
                            case BarActivities.Leave:
                                Console.WriteLine($"{Name} is leaving the bar.");
                                Bar.Leave(this);
                                staysAtBar = false;
                                break;
                            default: throw new NotImplementedException();
                        }
                    }
                }
                Console.WriteLine("You need to be at least 18 years old to visit the bar.");
            }
            else
            {
                Bar.Close();
            }
        }

        public void PaintTheTownRed()
        {
            CheckIfBarClosed();
            if (!isClosed)
            {
                WalkOut();
                bool staysOut = true;
                while (staysOut)
                {
                    var nextActivity = GetRandomNightlifeActivity();
                    switch (nextActivity)
                    {
                        case NightlifeActivities.Walk:
                            WalkOut();
                            break;
                        case NightlifeActivities.VisitBar:
                            VisitBar();
                            staysOut = false;
                            break;
                        case NightlifeActivities.GoHome:
                            staysOut = false;
                            break;
                        default: throw new NotImplementedException();
                    }
                }
                Console.WriteLine($"{Name} is going back home.");
            }
            else
            {
                return;
            }
        }

        public Student(string name, Bar bar, int budget, int age)
        {
            this.Name = name;
            this.Bar = bar;
            this.Budget = budget;
            this.Age = age;
        }
    }
}
