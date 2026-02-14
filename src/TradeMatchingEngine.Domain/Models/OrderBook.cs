using TradeMatchingEngine.Domain.Enums;
using TradeMatchingEngine.Domain.OrderComparers;

namespace TradeMatchingEngine.Domain.Models;

public class OrderBook
{
    public SortedSet<Order> buyOrderRecord { get; } = new SortedSet<Order>(new BuyOrderComparer());
    public SortedSet<Order> sellOrderRecord { get; } = new SortedSet<Order>(new SellOrderComparer());

    public bool AddOrder(Order order)
    {
        if(order.orderType == OrderType.Buy)
        {
            return buyOrderRecord.Add(order);
        }
        else
        {
            return sellOrderRecord.Add(order);
        }
    }

    public bool CancelOrder(Order order)
    {
        if(order.orderType == OrderType.Buy)
        {
            return buyOrderRecord.Remove(order);
        }
        else
        {
            return sellOrderRecord.Remove(order);
        }
    }

    public Order? GetBestBuyOrder()
    {
        if(buyOrderRecord.Count > 0)
        {
            return buyOrderRecord.First();
        }
        else
        {
            return null;
        }
    }

    public Order? GetBestSellOrder()
    {
        if(sellOrderRecord.Count > 0)
        {
            return sellOrderRecord.First();
        }
        else
        {
            return null;
        }
    }
}