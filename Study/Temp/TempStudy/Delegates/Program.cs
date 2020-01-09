using System;


namespace Shipping
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string zoneName;
            ShippingInfo shippingInfo = new ShippingInfo();
            do
            {
                Console.WriteLine();
                Console.WriteLine("What is the destination zone?");
                Console.WriteLine(" *type 'exit' for exit");
                zoneName = Console.ReadLine();
                if ("exit" == zoneName || !shippingInfo.IsZoneNameCorrect(zoneName))
                {
                    continue;
                }
                Console.WriteLine("What is the item price?");
                int.TryParse(Console.ReadLine(), out int price);
                if (0 == price)
                {
                    Console.WriteLine("Invalid item price");
                    continue;
                }
                shippingInfo.CalculateFee(zoneName, price);
            } while (zoneName != "exit");
            Console.WriteLine("  exit..");
        }
    }
}
