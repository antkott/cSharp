using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeTrySimulation
{
    public static class ChangeSolution
    {
        public static Change NotOptimalChange(long s)
        {
            if (s < 2)
            {
                return null;
            }
            long ten = 0;
            long five = 0;
            long two = 0;

            if (s >= 10)
            {
                ten = s / 10;
                var b10Remain5 = (s - ten * 10) % 5;
                var b10Remain2 = (s - ten * 10) % 2;
                if (ten > 1 && b10Remain5 != 0 && b10Remain2 != 0)
                {
                    ten -= 1;
                    b10Remain5 = (s - ten * 10) % 5;
                    b10Remain2 = (s - ten * 10 - 5) % 2;
                }
                if (ten > 0 && b10Remain5 != 0 && b10Remain2 != 0)
                {
                    ten = 0;
                }
                s -= ten * 10;
            }
            if (s >= 5)
            {
                five = s / 5;
                var b5Remain2 = (s - five * 5) % 2;
                if (five > 1 && b5Remain2 != 0)
                {
                    five -= 1;
                    b5Remain2 = (s - five * 5) % 2;
                }
                if (five > 0 && b5Remain2 != 0)
                {
                    five = 0;
                }
                s -= five * 5;
            }
            if (s >= 2)
            {
                var coin2Remain = s % 2;
                if (coin2Remain > 0)
                {
                    return null;
                }
                two = s / 2;
            }
            return new Change
            {
                coin2 = two,
                bill5 = five,
                bill10 = ten
            };
        }

        public static Change OptimalChange(long s)
        {
            var m = new Change();
            while (s > 0)
            {
                if (s % 2 == 1)
                {
                    s -= 5;
                    m.bill5++;
                }
                else if (s >= 10)
                {
                    long nb10 = s / 10;
                    s -= nb10 * 10;
                    m.bill10 += nb10;
                }
                else
                {
                    long nb2 = s / 2;
                    s -= nb2 * 2;
                    m.coin2 += nb2;
                }
            }

            return s < 0 ? null : m;
        }

        public static Change OptimalChange2(long s)
        {
            Change c = new Change();
            if (s % 2 == 1)
            {
                s -= 5;
                if (s < 0)
                {
                    return null;
                }
                c.bill5 = 1;
            }
            c.bill10 = s / 10;
            c.coin2 = (s % 10) / 2;

            return c;
        }
    }
}
