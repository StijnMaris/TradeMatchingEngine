
using TradeMatchingEngine.Domain;
using TradeMatchingEngine.Domain.Enums;
using TradeMatchingEngine.Domain.Models;

namespace TradeMatchingEngine.Tests;

public class BasicFunctionalityTests
{
/// <summary>
/// Verifies a buy order added to buyOrder list.
/// </summary>
    [Fact]
    [Trait("Category", "BasicFunctionality")]
    public void SingleBuyOrder()
    {
        var engine = new MatchingEngine();

        var buyOrder = Order.Create(OrderType.Buy, 100m, 50m);
        engine.ProcessOrder(buyOrder);

        Assert.Single(engine.BuyOrders);
        var buy = engine.BuyOrders.First();
        Assert.Equal(100m, buy.price);
        Assert.Equal(50m, buy.quantity);
        Assert.Equal(buyOrder.id, buy.id);        
    }

/// <summary>
/// Verifies a sell order added to sellOrder list.
/// </summary>
    [Fact]
    [Trait("Category", "BasicFunctionality")]
    public void SingleSellOrder()
    {
        var engine = new MatchingEngine();

        var sellOrder = Order.Create(OrderType.Sell, 100m, 50m);
        engine.ProcessOrder(sellOrder);

        Assert.Single(engine.SellOrders);
        var sell = engine.SellOrders.First();
        Assert.Equal(100m, sell.price);
        Assert.Equal(50m, sell.quantity);
        Assert.Equal(sellOrder.id, sell.id);        
    }
}
