using System;
using System.Collections.Generic;
using System.Text;

namespace FactoryMethod1
{
    //creator
    public abstract class AbstractFactory
    {
        public AbstractFactory(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        private string _name;
        public string Name { get => _name; set { _name = value; Console.WriteLine($"{value} assigned to build project"); } }
        public abstract AbstractStoneHouse BuildStoneHouse();
        public abstract AbstractWoodenHouse BuildWoodenHouse();
    }

    //concrete creator
    public class StoneHouseFactory : AbstractFactory
    {
        public StoneHouseFactory(string name) : base(name)
        {
        }

        public override AbstractStoneHouse BuildStoneHouse()
        {
            Console.WriteLine($"'{Name}' start slowly build a house");
            return new StoneHouse();
        }

        public override AbstractWoodenHouse BuildWoodenHouse()
        {
            Console.WriteLine($"'{Name}' start slowly build a house");
            return new WoodenHouse();
        }
    }

    //concrete creator
    public class WoodenHouseFactory : AbstractFactory
    {
        public WoodenHouseFactory(string name) : base(name)
        {
        }


        public override AbstractStoneHouse BuildStoneHouse()
        {
            Console.WriteLine($"'{Name}' start quick build a house");
            return new StoneHouse();
        }

        public override AbstractWoodenHouse BuildWoodenHouse()
        {
            Console.WriteLine($"'{Name}' start quick build a house");
            return new WoodenHouse();
        }
    }

}
