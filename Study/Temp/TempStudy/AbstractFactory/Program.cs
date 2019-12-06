using System;

namespace AbstractFactory
{
    class Program
    {
        static void Main(string[] args)
        {
            new AbstractFactory();
            creatorStoneHouse.BuildAHouse();

            var creatorWoodenHouse = new WoodenHouseFactory("Wooden Ltd.");
            creatorWoodenHouse.BuildAHouse();
        }
    }
}
