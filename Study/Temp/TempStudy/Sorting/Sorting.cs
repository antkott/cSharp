using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Toolset;
using static System.Console;

namespace Sorting
{
    public class Sorting
    {
        public int[] BubbleSort(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array.Length - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
                Write($"current state {i}: ");
                Tools.ShowItemsEnumerable(array);

            }
            return array;
        }


        public int[] MergeSort(int[] array)
        {
            if (array.Length > 1)
            {
                int mid = array.Length / 2;
                int[] leftArray = array.Take(mid).ToArray();
                int[] rigthArray = array.Skip(mid).ToArray();

                MergeSort(leftArray);
                MergeSort(rigthArray);

                int i = 0;
                int j = 0;
                int k = 0;
                while (i < leftArray.Length && j < rigthArray.Length)
                {
                    if (leftArray[i] < rigthArray[j])
                    {
                        array[k] = leftArray[i];
                        i++;
                    }
                    else
                    {
                        array[k] = rigthArray[j];
                        j++;
                    }
                    k++;
                }

                while (i < leftArray.Length)
                {
                    array[k] = leftArray[i];
                    i++;
                    k++;
                }

                while (j < rigthArray.Length)
                {
                    array[k] = rigthArray[j];
                    j++;
                    k++;
                }

            }

            return array;
        }

        public int[] QuickSort(int[] array, int first, int last)
        {
            if (first < last)
            {
                int pivotIndex = Partition(array, first, last);

                QuickSort(array, first, pivotIndex - 1);
                QuickSort(array, pivotIndex + 1, last);
            }
            return array;
        }

        private int Partition(int[] array, int first, int last)
        {
            int pivotValue = array[first];
            int lower = first + 1;
            int upper = last;

            bool done = false;
            while (!done)
            {
                while (lower <= upper && array[lower] <= pivotValue)
                {
                    lower++;
                }
                while (array[upper] >= pivotValue && upper >= lower)
                {
                    upper--;
                }
                if (upper < lower)
                {
                    done = true;
                }
                else
                {
                    int temp = array[lower];
                    array[lower] = array[upper];
                    array[upper] = temp;
                }
            }

            int temp2 = array[first];
            array[first] = array[upper];
            array[upper] = temp2;
            return upper;
        }
    }

}


