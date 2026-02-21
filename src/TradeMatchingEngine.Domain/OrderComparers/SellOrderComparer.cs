
using TradeMatchingEngine.Domain.Models;

namespace TradeMatchingEngine.Domain.OrderComparers;

public class SellOrderComparer : IComparer<Order>
{
    // Sort by price ascending, then by timestamp ascending -1 means x is first and 1 means y is first.
    public int Compare(Order x, Order y)
    {
        if (x == null || y == null)
        {
            throw new ArgumentException("Orders being compared cannot be null.");
        }

        if(x.price < y.price)
        {
            return -1;
        }
        else if(x.price > y.price)
        {
            return 1;
        }
        else
        {
            if(x.timestamp < y.timestamp)
            {
                return -1;
            }
            else if(x.timestamp > y.timestamp)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}