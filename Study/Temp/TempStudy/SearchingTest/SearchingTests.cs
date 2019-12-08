using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace AntKott.Study.Searching.Test
{
    public class SearchingTests
    {

        private readonly Searching _searching;
        private readonly ITestOutputHelper output;

        public SearchingTests(ITestOutputHelper output)
        {
            _searching = new Searching();
            this.output = output;
            output.WriteLine("CONSTR");
        }


        public static IEnumerable<object[]> UnsortedData()
        {
            yield return new object[] { new List<int>() { 20, 6, 8, 19, 56, 23, 87, 41, 49, 53 }, 20, 0 };
            yield return new object[] { new List<int>() { 20, 6, 8, 19, 56, 23, 87, 41, 49, 53 }, 99, null };
        }

        [Theory]
        [MemberData(nameof(UnsortedData))]
        public void SearchingUnsortedList_ReturnDesiredValue(List<int> unsortedArray, int desiredNumber, int? expected)
        {
            //v2 = v1.GetValueOrDefault();
            int? result = _searching.FindItemInUnsortedList(desiredNumber, unsortedArray);
            output.WriteLine("Running ");
            Assert.Equal(expected, result);
        }


        public static IEnumerable<object[]> BinarySearch_Data()
        {
            yield return new object[] { new List<int>() { 6, 8, 19, 20, 23, 41, 49, 53, 56, 87 }, 19, 2 };
            yield return new object[] { new List<int>() { 6, 8, 19, 20, 23, 41, 49, 53, 56, 87 }, 99, null };
        }

        [Theory]
        [MemberData(nameof(BinarySearch_Data))]
        public void BinarySearch_ReturnDesiredValue(List<int> sortedArray, int desiredNumber, int? expected)
        {
            int? result = _searching.BinarySearch(desiredNumber, sortedArray);
            output.WriteLine("Running ");
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> IsSorted_Data()
        {
            yield return new object[] { new List<int>() { 6, 8, 19, 20, 23, 41, 49, 53, 56, 87 }, true };
            yield return new object[] { new List<int>() { 20, 6, 8, 19, 56, 23, 87, 41, 49, 53 }, false };
        }

        [Theory]
        [MemberData(nameof(IsSorted_Data))]
        public void IsSorted_Test(List<int> list, bool expected)
        {
            bool result = _searching.IsSorted(list);
            output.WriteLine("Running ");
            Assert.Equal(expected, result);
        }
    }
}