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
        private static readonly List<string> _duplicatedItemsList = new List<string>(){"apple", "pear", "orange", "banana", "apple",
         "orange", "apple", "pear", "banana", "orange",
         "apple", "kiwi", "pear", "apple", "orange" };
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

            WriteLine($"==============");
            OtherAlgorithm otherAlgorithm = new OtherAlgorithm();
            Write("duplicated List: ");
            Tools.ShowItemsEnumerable(_duplicatedItemsList);
            var result4 = otherAlgorithm.Filtering(_duplicatedItemsList);
            Write("NonDuplictaed List: ");
            Tools.ShowItemsEnumerable(result4);
            
            Write("value counter for: ");
            Tools.ShowItemsEnumerable(_duplicatedItemsList);
            var result5 = otherAlgorithm.ValueCounter(_duplicatedItemsList);
            Write("value counter: ");
            Tools.ShowItemsEnumerable(result5);

            Write("get Max value for: ");
            Tools.ShowItemsEnumerable(_unsortedNumberlist);
            var result6 = otherAlgorithm.MaxValue(_unsortedNumberlist);
            Write($"Max value: {result6}");
        }
    }
}
