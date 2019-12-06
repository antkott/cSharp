using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractFactory
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

    //abstract product1
    public abstract class School
    {
         public abstract void Type { get; }        
    }

    public abstract class AbstractWoodenHouse
    {

        public abstract string Type { get; }
    }

    //conctrete product1
    class StoneHouse : AbstractStoneHouse
    {
        private readonly string _type;
        public StoneHouse()
        {
            _type = nameof(StoneHouse);
            Console.WriteLine($"'{_type}' was built");
        }
        public override string Type { get => _type; }
    }

    //abstract product2
    

    //conctrete product2
    class WoodenHouse : AbstractWoodenHouse
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
