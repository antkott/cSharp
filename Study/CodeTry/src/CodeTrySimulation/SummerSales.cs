using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTrySimulation
{
    /*
     
     It's almost the Summer Sales!
 
You work for a shop that wishes to give a discount of discount% to the most expensive item purchased by a given customer during the sales period. Only one product can benefit from the discount. 
 
You are tasked by the shop owner to implement the method CalculateTotalPrice(prices, discount) which takes the list of prices of the products purchased by a customer and the percentage discount as parameters and returns the total purchase price as an integer (rounded down if the total is a float number).
 
Constraints:

    0 ≤ discount ≤ 100
    0 < price of a product < 100000
    0 < number of products < 100


    Simple sum
Problem solving+35 pts
Good sale
Problem solving+35 pts
Large discount
Problem solving+35 pts
Correct rouding
Reliability+35 pts
One item free
Problem solving+35 pts
No sales
Problem solving+35 pts
Big purchase
Problem solving+30 pts
Same price
Reliability+30 pts
One item only
Reliability+30 pts
     
     */

    public static class SummerSales
    {

        public static int CalculateTotalPrice(int[] prices, int discount)
        {
            if (prices.Length == 0) {
                return 0; 
            }
            Console.Error.WriteLine($"Discount {discount}");
            float sum = 0;
            var discountIsApplyed = false;
            var maxPrice = prices.Max();
            prices.ToList().ForEach(itemPrice =>
            {
                Console.Error.Write($"'{itemPrice}', ");
                if (itemPrice == maxPrice && !discountIsApplyed)
                {
                    var discountValue = maxPrice * ((float)discount / 100);
                    var discountedPrice = itemPrice - discountValue;
                    sum += discountedPrice;
                    discountIsApplyed = true;
                }
                else
                {
                    sum += itemPrice;
                }
            });
            return Convert.ToInt32(Math.Round(sum, MidpointRounding.ToZero));
        }
    }
}
