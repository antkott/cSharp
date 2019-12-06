using System;
using System.Collections.Generic;
using System.Text;

namespace Sorting
{
    public class BubbleSort
    {
        public int[] DoBubbleSort(int[] unsortedArray)
        {
            int[] sortedArray = new int[unsortedArray.Length];
            for (int i = 0; i < unsortedArray.Length; i++)
            {
                for (int j = 0; j < unsortedArray.Length - 1; j++)
                {
                    if (unsortedArray[j] > unsortedArray[j + 1])
                    {
                        var temp = unsortedArray[j];
                        unsortedArray[j] = unsortedArray[j + 1];
                        unsortedArray[j + 1] = temp;

                    }
                }
            }
             return unsortedArray;
        }
    }
}
