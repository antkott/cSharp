using System;
using System.Collections.Generic;
using System.Text;

namespace Decorator
{

    //Decorator:
    public abstract class PizzaDecorator : Pizza {
        protected Pizza pizza;

        protected PizzaDecorator(string name, Pizza pizza):base (name)
        {
            this.pizza = pizza ?? throw new ArgumentNullException(nameof(pizza));
        }
    }

    //ConcreteDecorator A
    public class TomatoPizza : PizzaDecorator
    {
        public TomatoPizza(Pizza pizza) : base(pizza.Name+", with tomato", pizza)
        {
        }

        public override int GetCost()
        {
            return pizza.GetCost() + 3;
        }
    }

    //ConcreteDecorator B
    public class CheesePizza : PizzaDecorator
    {
        public CheesePizza(Pizza pizza) : base(pizza.Name + ", with Cheese", pizza)
        {
        }

        public override int GetCost()
        {
            return pizza.GetCost() + 5;
        }
    }
}
