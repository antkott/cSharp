using System;
using System.Collections.Generic;
using System.Text;

namespace FactoryMethod1
{
    
    //product
    public abstract class House
    {
        protected House()
        {
            Console.WriteLine($"{nameof(House)} general blueprint will be used");
        }

        abstract public string Type { get; }

    }

    //conctrete product1
    class StoneHouse : House
    {
        private readonly string _type;
        public StoneHouse()
        {
            _type = nameof(StoneHouse);
            Console.WriteLine($"'{_type}' was built");
        }

        public override string Type { get => _type; }
    }

    //conctrete product2
    class WoodenHouse : House
    {
        private readonly string _type;
        public WoodenHouse()
        {
            _type = nameof(WoodenHouse);
            Console.WriteLine($"'{_type}' was built");
        }
        public override string Type { get => _type; }
    }
}
