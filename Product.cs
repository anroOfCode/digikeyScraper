using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigikeyApi
{
    public enum PartStatus
    {
        InStock,
        NonStock
    }

    public class Product
    {
        public string partNumber { get; set; }
        public int quantityAvailiable { get; set; }
        public PartStatus status { get; set; }
        public SortedDictionary<int, decimal> pricing { get; set; }
        public string warning { get; set; }
        public decimal calculatePrice(int quantity)
        {
            SortedDictionary<int, decimal>.Enumerator e = pricing.GetEnumerator();
            int smallestQuantity = -1;
            decimal smallestPrice = 0;
            while (e.MoveNext())
            {
                if (quantity >= e.Current.Key)
                {
                    if (e.Current.Key > smallestQuantity || smallestQuantity == -1)
                    {
                        smallestQuantity = e.Current.Key;
                        smallestPrice = e.Current.Value;
                    }
                }
            }
            if (smallestPrice == 0)
            {
                smallestPrice = pricing.ElementAt(0).Value;
            }
            return smallestPrice * quantity;
        }
    }
}
