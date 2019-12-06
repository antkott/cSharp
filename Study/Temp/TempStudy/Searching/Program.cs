using System;
using System.Collections.Generic;
using Toolset;
using static System.Console;

namespace AntKott.Study.Searching
{
    class Program
    {

        private static readonly List<int> _unsortedNumberlist = new List<int>() { 20, 6, 8, 19, 56, 23, 87, 41, 49, 53 };
        private static readonly List<int> _sortedNumberlist = new List<int>() { 6, 8, 19, 20, 23, 41, 49, 53, 56, 87 };
        static void Main(string[] args)
        {
            Searching searching = new Searching();

            int desiredNumber = 56;
            int nonexistingNumber = 666;
            WriteLine($"unordered search for {desiredNumber}");
            Write("list: ");
            Tools.ShowItemsEnumerable(_unsortedNumberlist);
            var result = searching.FindItemInUnsortedList(desiredNumber, _unsortedNumberlist);
            WriteLine($"desirednumber is '{desiredNumber}', index is: '{result}'");

            
            WriteLine($"binary search for {desiredNumber}");
            Write("list: ");
            Tools.ShowItemsEnumerable(_sortedNumberlist);
            var result2 = searching.BinarySearch(desiredNumber, _sortedNumberlist);
            WriteLine($"desirednumber is '{desiredNumber}', index is: '{result2}'");

            WriteLine($"binary search for non exists {nonexistingNumber}");
            Write("list: ");
            Tools.ShowItemsEnumerable(_sortedNumberlist);
            var result3 = searching.BinarySearch(nonexistingNumber, _sortedNumberlist);
            int intResult3 = result3.GetValueOrDefault();
            WriteLine($"desirednumber is '{nonexistingNumber}', index is: '{intResult3}'");
        }
    }
}
