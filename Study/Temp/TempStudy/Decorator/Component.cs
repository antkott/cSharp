using System;
using System.Collections.Generic;
using System.Text;

namespace Decorator
{
    
    //component
    public abstract class Pizza
    {
        protected Pizza(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; protected set; }
        public abstract int GetCost();
    }

    //ConcreteComponent 1
    public class ItalianPizza : Pizza
    {
        public ItalianPizza() : base("Italian pizza")
        {
        }
        public override int GetCost() { return 10; }
    }

    //ConcreteComponent 2
    public class BulgerianPizza : Pizza
    {
        public BulgerianPizza() : base("Bulgerian pizza")
        {
        }
        public override int GetCost() { return 8; }
    }
}
