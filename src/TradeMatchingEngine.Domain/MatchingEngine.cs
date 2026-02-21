
using TradeMatchingEngine.Domain.Models;
using TradeMatchingEngine.Domain.Enums;

namespace TradeMatchingEngine.Domain;

public class MatchingEngine
{  
    private readonly OrderBook orderBook = new OrderBook();

    private List<Trade> tradeRecord { get; } = new List<Trade>();

    public IReadOnlyList<Trade> TradeHistory => tradeRecord.AsReadOnly();

    public void ProcessOrder(Order order)
    {
        if (order == null || order.quantity <= 0)
        {
            throw new ArgumentException("Order must be valid && quantity must be greater than zero.");
        }

        if (order.orderType == OrderType.Buy)
        {
            ProcessBuyOrder(order);
        }
        else
        {
            ProcessSellOrder(order);
        }
    }

    private void ProcessBuyOrder(Order buyOrder)
    {
       Order? bestSellOrder = orderBook.GetBestSellOrder();

        while(bestSellOrder != null && !buyOrder.IsEmpty() && buyOrder.price >= bestSellOrder.price)
        {
            decimal tradeQuantity = Math.Min(buyOrder.quantity, bestSellOrder.quantity);
            decimal tradePrice = bestSellOrder.price;
            
            Trade trade = new Trade(Guid.NewGuid(), buyOrder.id, bestSellOrder.id, tradeQuantity, tradePrice, DateTime.UtcNow);
            tradeRecord.Add(trade);

            if (buyOrder.quantity > tradeQuantity)
            {
                buyOrder.Fill(tradeQuantity);

                orderBook.CancelOrder(bestSellOrder);
                bestSellOrder = orderBook.GetBestSellOrder();
            }
            else if (bestSellOrder.quantity > tradeQuantity)
            {
                buyOrder.Clear();
                bestSellOrder.Fill(tradeQuantity);
            } 
            else
            {
                buyOrder.Clear();
                orderBook.CancelOrder(bestSellOrder);
            }
        }

        if(!buyOrder.IsEmpty())
        {
            orderBook.AddOrder(buyOrder);
        } 
    }

    private void ProcessSellOrder(Order sellOrder)
    {
        Order? bestBuyOrder = orderBook.GetBestBuyOrder();

        while(bestBuyOrder != null && !sellOrder.IsEmpty() && sellOrder.price <= bestBuyOrder.price)
        {
            decimal tradeQuantity = Math.Min(sellOrder.quantity, bestBuyOrder.quantity);
            decimal tradePrice = bestBuyOrder.price;
            
            Trade trade = new Trade(Guid.NewGuid(), bestBuyOrder.id, sellOrder.id, tradeQuantity, tradePrice, DateTime.UtcNow);
            tradeRecord.Add(trade);

            if (sellOrder.quantity > tradeQuantity)
            {
                sellOrder.Fill(tradeQuantity);

                orderBook.CancelOrder(bestBuyOrder);
                bestBuyOrder = orderBook.GetBestBuyOrder();
            }
            else if (bestBuyOrder.quantity > tradeQuantity)
            {
                sellOrder.Clear();
                bestBuyOrder.Fill(tradeQuantity);
            }  
            else
            {
                sellOrder.Clear();
                orderBook.CancelOrder(bestBuyOrder);
            }
        }

        if(!sellOrder.IsEmpty())
        {
            orderBook.AddOrder(sellOrder);
        }
    }
}