using System;
using static System.Console;
using Toolset;


namespace Sorting
{
    internal class Program
    {
        private static readonly int[] _unsortedArray = new int[] { 20, 6, 8, 19, 56, 23, 87, 41, 49, 53 };

        private static void Main(string[] args)
        {
            
            Sorting _sorting = new Sorting();

            WriteLine("Bubble sort");
            Write("Unsorted array: ");
            int[] unsortedBubbleSortArray = new int[_unsortedArray.Length];
            Array.Copy(_unsortedArray, unsortedBubbleSortArray, _unsortedArray.Length);
            Tools.ShowItemsEnumerable(unsortedBubbleSortArray);
            
            int[] sortedArray = _sorting.BubbleSort(unsortedBubbleSortArray);
            Write("sorted array: ");
            Tools.ShowItemsEnumerable(sortedArray);

            WriteLine("Merge sort");
            Write("Unsorted array: ");
            int[] unsortedMergeSortArray = new int[_unsortedArray.Length];
            Array.Copy(_unsortedArray, unsortedMergeSortArray, _unsortedArray.Length);
            Tools.ShowItemsEnumerable(unsortedMergeSortArray);
            int[] _sortedMergeSortArray = _sorting.MergeSort(unsortedMergeSortArray);
            Write($"sorted array:");
            Tools.ShowItemsEnumerable(_sortedMergeSortArray);

            WriteLine("Quick sort");
            Write("Unsorted array: ");
            int[] unsortedQuickSortArray = new int[_unsortedArray.Length];
            Array.Copy(_unsortedArray, unsortedQuickSortArray, _unsortedArray.Length);
            Tools.ShowItemsEnumerable(unsortedMergeSortArray);
            int[] _sortedQuickSortArray = _sorting.QuickSort(unsortedQuickSortArray, 0, unsortedQuickSortArray.Length - 1);
            Write($"sorted array:");
            Tools.ShowItemsEnumerable(_sortedQuickSortArray);
        }
    }
}
