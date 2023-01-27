using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTrySimulation
{

    /* 
     * In this exercise, you have to analyze records of temperature to find the closest to zero.
 Sample temperatures. Here, -1 is the closest to 0.
 Implement the method int ComputeClosestToZero(int[] ts) which takes an array of temperatures ts and returns the temperature closest to 0.

     Constraints:

     If the array is empty, the method should return 0
     0 ≤ ts size ≤ 10000
     If two temperatures are equally close to zero, the positive temperature must be returned. For example, if the input is -5 and 5, then 5 must be returned.

             Result is correct with a simple data set: {7 5 9 1 4} -> 1
             Problem solving+70 pts
             It works with -273 alone
             Problem solving+20 pts
             It works with 5526 alone
             Problem solving+20 pts
             It works when inputs contains only negative numbers: : {-15 -7 -9 -14 -12} -> -7
             Problem solving+35 pts
             It works with two negative temperatures that are equal: { -10 - 10} -> -10
             Problem solving+35 pts
             The solution displays 0 if no temperature
             Problem solving+35 pts
             When two temperatures are as close to 0, then the positive wins: { 15 - 7 9 14 7 12} -> 7
             Problem solving+85 pts
    */

    public class Temperature
    {
        public static int ComputeClosestToZero(int[] ts)
        {
            if (ts.Length == 0) return 0;
            int min = int.MaxValue;
            for (var i = 0; i < ts.Length; i++)
            {
                var currValue = ts[i];
                var curr = Math.Abs(currValue);
                if (currValue + min == 0 && currValue > 0)
                {
                    min = currValue;
                    continue;
                }
                if (curr < Math.Abs(min))
                {

                    min = currValue;
                }
            }
            return min;
        }

        public static int ComputeClosestToZero2(int[] ints)
        {
            if (ints == null || ints.Length == 0) return 0;
            int closest = ints[0];
            foreach (int i in ints)
            {
                int abs = Math.Abs(i);
                int absClosest = Math.Abs(closest);
                if (abs < absClosest)
                {
                    closest = i;
                }
                else if (abs == absClosest && closest < 0)
                {
                    closest = i;
                }
            }
            return closest;
        }
    }
}
