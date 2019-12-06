using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StudyDemo.Demo
{
    class AsyncDemo : DemoRoot, IDemonstrable
    {
        public void RunDemo()
        {
            ShowRunDemoInformation();
            RunDemo1();
        }

        private void RunDemo1()
        {
            FactorialAsync(10);
            //Console.WriteLine("Enter Number: ");
            //int n = Int32.Parse(Console.ReadLine());
            //Console.WriteLine($"its {n * n}");
            Console.Read();
            Console.WriteLine($"END demo");
        }

        private async void FactorialAsync(int fackEnd)
        {
            Console.WriteLine("Start FactorialAsync"); // выполняется синхронно
            int x = await Task.Run(() => Factorial(fackEnd));  // выполняется асинхронно
            Console.WriteLine("end FactorialAsync");
            Console.WriteLine($"Factorial =  {x}");
            Console.Read();
        }

        private int Factorial(int fackEnd)
        {
            int result = 1;
            for (int i = 1; i <= fackEnd; i++)
            {
                result *= i;
            }

            Thread.Sleep(10000);
             return result;
        }
    }
}
