using System;
using System.Linq;
using Xunit;

namespace Sorting.Tests
{
    public class SortingTest
    {


        private readonly int[] _expectResult = new int[] { 6, 8, 19, 20, 23, 41, 49, 53, 56, 87 };
        private readonly Sorting _sorting;
        public SortingTest()
        {
            _sorting = new Sorting();
        }


        // int[] sortedArray = bubbleSort.BubbleSort(unsortedArray);

        [Theory]
        [InlineData(new int[] { 6, 20, 8, 19, 56, 23, 87, 41, 49, 53 })]
        public void DoBubbleSort_ReturnSortedArray(int[] unsortedArray)
        {
            int[] resultArray = _sorting.BubbleSort(unsortedArray);
            Assert.True(resultArray.SequenceEqual(_expectResult));
        }

        [Theory]
        [InlineData(new int[] { 6, 20, 8, 19, 56, 23, 87, 41, 49, 53 })]
        public void MergeSort_ReturnSortedArray(int[] unsortedArray)
        {
            int[] resultArray = _sorting.MergeSort(unsortedArray);
             Assert.True(resultArray.SequenceEqual(_expectResult));
        }

        [Theory]
        [InlineData(new int[] { 6, 20, 8, 19, 56, 23, 87, 41, 49, 53 })]
        public void QuickSort_ReturnSortedArray(int[] unsortedArray)
        {
            int[] resultArray = _sorting.QuickSort(unsortedArray, 0, unsortedArray.Length - 1);
            Assert.True(resultArray.SequenceEqual(_expectResult));
        }
    }
}
