using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//<Company>.(<Product>|<Technology>)[.<Feature>][.<Subnamespace>]
namespace AntKott.Study.Searching
{
    public class Searching
    {

        public int? FindItemInUnsortedList(int desiredItem, List<int> inputList)
        {
            foreach (var item in inputList.Select((value, i) => new { i, value }))
            {
                var value = item.value;
                var index = item.i;
                if (value == desiredItem)
                {
                    return index;
                }
            }
            return null;
        }

        public int? BinarySearch(int desiredItem, List<int> inputList)
        {
            var listSize = inputList.Count()-1;
            var lowerIndex = 0;
            var upperIndex = listSize;

            while (lowerIndex<=upperIndex)
            {
                var midPoint = (lowerIndex + upperIndex) / 2;

                if (inputList[midPoint]==desiredItem)
                {
                    return midPoint;
                }

                if (desiredItem > inputList[midPoint])
                {
                    lowerIndex = midPoint + 1;
                }
                else {
                    upperIndex = midPoint - 1;
                }
            }
            //if (lowerIndex>upperIndex)
            //{
            //    return null;
            //}
            return null;
        }
    }
}
