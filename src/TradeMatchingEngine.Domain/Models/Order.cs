
using TradeMatchingEngine.Domain.Enums;

namespace TradeMatchingEngine.Domain.Models;
public class Order
{

    public static Order Create(OrderType orderType, decimal price, decimal quantity)
    {
        if(price <= 0 || price > decimal.MaxValue || quantity <= 0 || quantity > decimal.MaxValue)
        {
            throw new ArgumentException("Price and quantity must be greater than zero.");
        }

        return new Order(Guid.NewGuid(), orderType, price, quantity, DateTime.UtcNow);
    }

    private Order(Guid id, OrderType orderType, decimal price, decimal quantity, DateTime timestamp)
    {
        this.id = id;
        this.orderType = orderType;
        this.price = price;
        this.quantity = quantity;
        this.timestamp = timestamp;
    }

    public void Fill(decimal usedQuantity)
    {
        quantity = Math.Max(0, quantity - usedQuantity);
    }

    public void Clear() 
    {
        quantity = 0;
    }

    public bool IsEmpty() 
    {
        return quantity == 0;
    }

    public Guid id { get; init; }
    public OrderType orderType { get; init; }
    public decimal price { get; init; }
    public decimal quantity { get; private set; }
    public DateTime timestamp { get; init; }
};