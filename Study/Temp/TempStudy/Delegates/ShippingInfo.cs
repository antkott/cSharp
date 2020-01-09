using System;
using System.Collections.Generic;
using System.Linq;

namespace Shipping
{
    internal class ShippingInfo
    {
        private readonly int _additionalFee = 20;
        private (int zoneFee, bool additionalFee) _zoneFee;
        private readonly Dictionary<string, (int, bool)> _zones = new Dictionary<string, (int, bool)>()
        {
            { "zone1", (25, false)},
            { "zone2", (12, true)},
            { "zone3", (8, true)},
            { "zone4", (4, false)},
        };
        public string Zones => string.Join("; ", _zones.Select(x => x.Key).ToArray());
        private delegate int CalculateFeeDelegate(int zoneFee, int price);
        

        public void CalculateFee(string zoneName, int price)
        {
            CalculateFeeDelegate _calculateFeeDelegate = CalculateFee;
            if (_zoneFee.additionalFee == true)
            {
                _calculateFeeDelegate = CalculateFeeAddAdditionalFee;
            }
            _calculateFeeDelegate(_zoneFee.zoneFee, price);

        }

        public bool IsZoneNameCorrect(string zoneName)
        {
            _zoneFee = getZoneFee(zoneName);
            if (string.Empty == zoneName || 0 == _zoneFee.zoneFee)
            {
                Console.WriteLine("Invalid zone name");
                Console.WriteLine($" *available zones are: '{Zones}'");
                return false;
            }
            else
            {
                return true;
            }
        }

        private (int, bool) getZoneFee(string zoneName)
        {
            IEnumerable<KeyValuePair<string, (int, bool)>> zoneFee = from x in _zones
                                                                     where x.Key.Contains(zoneName)
                                                                     select x;
            return zoneFee.FirstOrDefault().Value;
        }

        private int CalculateFee(int zoneFee, int price)
        {
            int totalPrice = price * zoneFee / 100 + price;
            Console.WriteLine($"The shipping fees are '{zoneFee}'");
            Console.WriteLine($"Total price will be {totalPrice}");
            return totalPrice;
        }

        private int CalculateFeeAddAdditionalFee(int zoneFee, int price)
        {
            int totalPrice = CalculateFee(zoneFee, price);
            int totalPriceAdd = totalPrice + _additionalFee;
            Console.WriteLine($"Additional fee is '{_additionalFee}'");
            Console.WriteLine($"Total price with Additional fee will be {totalPriceAdd}");
            return totalPriceAdd;
        }
    }
}
