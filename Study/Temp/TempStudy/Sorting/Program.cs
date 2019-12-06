using System;
using static System.Console;

namespace Sorting
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] unsortedArray = new int[] { 6, 20, 8, 19, 56, 23, 87, 41, 49, 53 };
            BubbleSort bubbleSort = new BubbleSort();
            int[] sortedArray = bubbleSort.DoBubbleSort(unsortedArray);
            foreach (var item in sortedArray)
            {
                Write(item+", ");
            }
            
        }
    }
}
