using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace YieldTest

{

    public static class StringExtention{
        public static string ToStringAdditional(this object obj) {
            return obj.ToString() + " my test";
        }    
    }


    class Program
    {
        static void Main()
        {
            Console.WriteLine($"({MethodBase.GetCurrentMethod().Name}): start");

            static IEnumerable<int> GetTestCollection() {
                Console.WriteLine($"({MethodBase.GetCurrentMethod().Name}): start");
                List<int> list = new List<int>();
                Console.WriteLine($"({MethodBase.GetCurrentMethod().Name}): add 1 to collection");
                list.Add(1);
                Console.WriteLine($"({MethodBase.GetCurrentMethod().Name}): add 2 to collection");
                list.Add(2);
                Console.WriteLine($"({MethodBase.GetCurrentMethod().Name}): add 3 to collection");
                list.Add(3);
                return list;
            }

            static IEnumerable<int> GetTestYieldCollection()
            {
                Console.WriteLine($"({MethodBase.GetCurrentMethod().Name}): start");
                List<int> list = new List<int>();
                Console.WriteLine($"({MethodBase.GetCurrentMethod().Name}): add 1 to collection");
                list.Add(1);
                yield return list[0];
                Console.WriteLine($"({MethodBase.GetCurrentMethod().Name}): add 2 to collection");
                list.Add(2);
                yield return list[1];
                Console.WriteLine($"({MethodBase.GetCurrentMethod().Name}): add 3 to collection");
                list.Add(3);
                yield return list[2];
                Console.WriteLine($"({MethodBase.GetCurrentMethod().Name}): add 4 to collection");
                list.Add(4);
                yield return list[3];
            }
            
            IEnumerable<int> enumerable = GetTestCollection();
            Console.WriteLine($"Ready to iterate through the collection {nameof(GetTestYieldCollection)}");
            Console.WriteLine($"get first element");
            int v = enumerable.ElementAt(0);
            Console.WriteLine();
            
            IEnumerable<int> enumerable2 = GetTestYieldCollection();
            Console.WriteLine($"Ready to yield-iterate through the collection {nameof(GetTestYieldCollection)}");
            Console.WriteLine($"get first element");
            int v1 = enumerable2.ElementAt(0);
            Console.WriteLine($"get 3 element");
            int v3 = enumerable2.ElementAt(2);
            Console.WriteLine();

            Console.WriteLine("LINQ get second element from IEnumerable");
            Console.WriteLine(enumerable2?.Skip(1)?.FirstOrDefault());
            Console.WriteLine(enumerable2?.ElementAtOrDefault(1));
            Console.WriteLine();


            Console.WriteLine("Extension methods test");
            var str = "testString";
            Console.WriteLine(str.ToStringAdditional());





        }
    }
}
