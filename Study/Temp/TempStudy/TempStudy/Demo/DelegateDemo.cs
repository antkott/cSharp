using System;
using System.Collections.Generic;
using System.Text;

namespace StudyDemo
{
    public class DelegateDemo
    {

        private delegate double TestDelegate(double x, double y);

        private double Sum(double x, double y)
        {
            return x + y;
        }

        public void RunDemo()
        {
            Console.WriteLine("Delegate through a seperate method");
            TestDelegate testDelegate = new TestDelegate(this.Sum);

            double v = testDelegate(1, 2);
            Console.WriteLine(v);


            Console.WriteLine("Delegate through a anonymous method");
            TestDelegate testDelegate2 = delegate (double x, double y)
            {
                return x + y;
            };
            double v1 = testDelegate2(1, 2);
            Console.WriteLine(v1);

            Console.WriteLine("Delegate through a lamda method");
            TestDelegate testDelegate3 = (x, y) =>
            {
                return x + y;
            };
            double v2 = testDelegate3(1, 2);
            Console.WriteLine(v2);

            Console.ReadKey();
        }
    }
}
