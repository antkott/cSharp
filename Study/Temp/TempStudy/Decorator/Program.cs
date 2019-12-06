using System;

namespace Decorator
{
    class Program
    {
        static void Main(string[] args)
        {
            Pizza pizza1 = new ItalianPizza();
            Console.WriteLine($"Name '{pizza1.Name}'");
            Console.WriteLine($"Cost {pizza1.GetCost()}");
            pizza1 = new TomatoPizza(pizza1);
            Console.WriteLine($"Name '{pizza1.Name}'");
            Console.WriteLine($"Cost {pizza1.GetCost()}");


            Pizza pizza2 = new ItalianPizza();
            pizza2 = new CheesePizza(pizza2);
            Console.WriteLine($"Name '{pizza2.Name}'");
            Console.WriteLine($"Cost {pizza2.GetCost()}");
           

            Pizza pizza3 = new BulgerianPizza();
            pizza3 = new CheesePizza(pizza3);
            pizza3 = new TomatoPizza(pizza3);
            Console.WriteLine($"Name '{pizza3.Name}'");
            Console.WriteLine($"Cost {pizza3.GetCost()}");
        }
    }
}
