using System;
using System.Linq;
using Xunit;

namespace Sorting.Tests
{
    public class BubbleSortTest
    {

        
        private int[] _expectResult = new int[] { 6, 8, 19,20,23,41,49,53,56,87 };
        private readonly BubbleSort _bubbleSort;
        public BubbleSortTest()
        {
            _bubbleSort = new BubbleSort();           
        }


        // int[] sortedArray = bubbleSort.DoBubbleSort(unsortedArray);

        [Theory]        
        [InlineData(new int[] { 6, 20, 8, 19, 56, 23, 87, 41, 49, 53 })]
        public void DoBubbleSort_ReturnSortedArray(int[] unsortedArray)
        {
            int[]resultArray = _bubbleSort.DoBubbleSort(unsortedArray);
            Assert.True(resultArray.SequenceEqual(_expectResult));
        }
    }
}
