using System;

namespace FactoryMethod1
{
    class Program
    {
        static void Main(string[] args)
        {
            var creatorStoneHouse = new StoneHouseFactory("Stone Ltd.");
            creatorStoneHouse.BuildAHouse();

            var creatorWoodenHouse = new WoodenHouseFactory("Wooden Ltd.");
            creatorWoodenHouse.BuildAHouse();

            //Stone Ltd. assigned to build project
            //Stone Ltd. start build a house
            //House general blueprint will be used
            //StoneHouse was built

            //Wooden Ltd.assigned to build project
            //Wooden Ltd. start build a house
            //House general blueprint will be used
            //WoodenHouse was built
        }
    }
}
