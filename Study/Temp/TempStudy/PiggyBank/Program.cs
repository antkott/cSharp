using System;

namespace PiggyBank
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string readLine;
            PiggyBank piggyBank = new PiggyBank(550);
            do
            {

                Console.WriteLine();
                Console.WriteLine("How much to deposit?");
                Console.WriteLine(" *type 'exit' for exit");
                readLine = Console.ReadLine();
                if (!readLine.Equals("exit"))
                {
                    int.TryParse(readLine, out int amount);
                    if (0 == amount)
                    {
                        Console.WriteLine("Invalid deposit value");
                        continue;
                    }
                    piggyBank.Balance = amount;
                }
            } while (readLine != "exit");
            Console.WriteLine("  exit..");
        }
    }
}