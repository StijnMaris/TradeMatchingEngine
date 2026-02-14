
using TradeMatchingEngine.Domain.Models;

namespace TradeMatchingEngine.Domain.OrderComparers;

public class SellOrderComparer : IComparer<Order>
{
    public int Compare(Order x, Order y)
    {
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