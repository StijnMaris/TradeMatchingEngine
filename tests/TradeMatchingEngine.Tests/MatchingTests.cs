
using TradeMatchingEngine.Domain;
using TradeMatchingEngine.Domain.Enums;
using TradeMatchingEngine.Domain.Models;

namespace TradeMatchingEngine.Tests;

public class BasicMatchingTests
{
/// <summary>
/// Verifies a buy order fully filled against an equal sell order. (perfect trade)
/// </summary>
    [Fact]
    [Trait("Category", "BasicMatching")]
    public void ExactMatch()
    {
        var engine = new MatchingEngine();

        var sellOrder = Order.Create(OrderType.Sell, 100m, 50m);
        engine.ProcessOrder(sellOrder);

        var buyOrder = Order.Create(OrderType.Buy, 100m, 50m);
        engine.ProcessOrder(buyOrder);

        Assert.Single(engine.TradeHistory);

        var trade = engine.TradeHistory.First();
        Assert.Equal(100m, trade.price);
        Assert.Equal(50m, trade.quantity);
        Assert.Equal(sellOrder.id, trade.sellOrderId);
        Assert.Equal(buyOrder.id, trade.buyOrderId);

        Assert.Empty(engine.SellOrders);
        Assert.Empty(engine.BuyOrders);
    }

/// <summary>
/// Verifies a buy order partially fills against a smaller sell order, 
/// leaving remaining quantity on the buy order.
/// </summary>
    [Fact]
    [Trait("Category", "BasicMatching")]
    public void BuyPartialFillAgainstSmallerSellOrder()
    {
        var engine = new MatchingEngine();

        var sellOrder = Order.Create(OrderType.Sell, 100m, 50m);
        engine.ProcessOrder(sellOrder);

        var buyOrder = Order.Create(OrderType.Buy, 100m, 100m);
        engine.ProcessOrder(buyOrder);

        Assert.Single(engine.TradeHistory);
        Assert.Empty(engine.SellOrders);
        Assert.Single(engine.BuyOrders);

        var trade = engine.TradeHistory.First();
        Assert.Equal(100m, trade.price);
        Assert.Equal(50m, trade.quantity);
        Assert.Equal(sellOrder.id, trade.sellOrderId);
        Assert.Equal(buyOrder.id, trade.buyOrderId);
        
        var buy = engine.BuyOrders.First();
        Assert.Equal(100m, buy.price);
        Assert.Equal(50m, buy.quantity);
        Assert.Equal(buyOrder.id, buy.id);
    }

/// <summary>
/// Verifies a buy order fully fills against a bigger sell order, 
/// leaving remaining quantity on the sell order.
/// </summary>
    [Fact]
    [Trait("Category", "BasicMatching")]
    public void BuyFullFillAgainstBiggerSellOrder()
    {
        var engine = new MatchingEngine();

        var sellOrder = Order.Create(OrderType.Sell, 100m, 100m);
        engine.ProcessOrder(sellOrder);

        var buyOrder = Order.Create(OrderType.Buy, 100m, 50m);
        engine.ProcessOrder(buyOrder);

        Assert.Single(engine.TradeHistory);
        Assert.Single(engine.SellOrders);
        Assert.Empty(engine.BuyOrders);

        var trade = engine.TradeHistory.First();
        Assert.Equal(100m, trade.price);
        Assert.Equal(50m, trade.quantity);
        Assert.Equal(sellOrder.id, trade.sellOrderId);
        Assert.Equal(buyOrder.id, trade.buyOrderId);
        
        var sell = engine.SellOrders.First();
        Assert.Equal(100m, sell.price);
        Assert.Equal(50m, sell.quantity);
        Assert.Equal(sellOrder.id, sell.id);
    }

/// <summary>
/// Verifies a buy order doesn't match against a sell order with higher price, 
/// leaving both orders.
/// </summary>
    [Fact]
    [Trait("Category", "BasicMatching")]
    public void NoMatch()
    {
        var engine = new MatchingEngine();

        var sellOrder = Order.Create(OrderType.Sell, 100m, 100m);
        engine.ProcessOrder(sellOrder);

        var buyOrder = Order.Create(OrderType.Buy, 90m, 100m);
        engine.ProcessOrder(buyOrder);

        Assert.Empty(engine.TradeHistory);
        Assert.Single(engine.SellOrders);
        Assert.Single(engine.BuyOrders);
        
        var sell = engine.SellOrders.First();
        Assert.Equal(100m, sell.price);
        Assert.Equal(100m, sell.quantity);
        Assert.Equal(sellOrder.id, sell.id);

        var buy = engine.BuyOrders.First();
        Assert.Equal(90m, buy.price);
        Assert.Equal(100m, buy.quantity);
        Assert.Equal(buyOrder.id, buy.id);
    }

/// <summary>
/// Verifies a buy order against 2 sell orders with different price, 
/// leaving the sell order with higher price.
/// </summary>
    [Fact]
    [Trait("Category", "BasicMatching")]
    public void PricePriority()
    {
        var engine = new MatchingEngine();

        var firstSellOrder = Order.Create(OrderType.Sell, 110m, 10m);
        engine.ProcessOrder(firstSellOrder);

        var secondSellOrder = Order.Create(OrderType.Sell, 100m, 10m);
        engine.ProcessOrder(secondSellOrder);

        var buyOrder = Order.Create(OrderType.Buy, 120m, 10m);
        engine.ProcessOrder(buyOrder);

        Assert.Single(engine.TradeHistory);
        Assert.Single(engine.SellOrders);
        Assert.Empty(engine.BuyOrders);

        var trade = engine.TradeHistory.First();
        Assert.Equal(100m, trade.price);
        Assert.Equal(10m, trade.quantity);
        Assert.Equal(secondSellOrder.id, trade.sellOrderId);
        Assert.Equal(buyOrder.id, trade.buyOrderId);
        
        var sell = engine.SellOrders.First();
        Assert.Equal(110m, sell.price);
        Assert.Equal(10m, sell.quantity);
        Assert.Equal(firstSellOrder.id, sell.id);
    }

/// <summary>
/// Verifies a buy order against 2 sell orders with the same price, 
/// leaving the sell order that was added last.
/// </summary>
    [Fact]
    [Trait("Category", "BasicMatching")]
    public void TimePriority()
    {
        var engine = new MatchingEngine();

        var firstSellOrder = Order.Create(OrderType.Sell, 100m, 10m);
        engine.ProcessOrder(firstSellOrder);

        var secondSellOrder = Order.Create(OrderType.Sell, 100m, 10m);
        engine.ProcessOrder(secondSellOrder);

        var buyOrder = Order.Create(OrderType.Buy, 120m, 10m);
        engine.ProcessOrder(buyOrder);

        Assert.Single(engine.TradeHistory);
        Assert.Single(engine.SellOrders);
        Assert.Empty(engine.BuyOrders);

        var trade = engine.TradeHistory.First();
        Assert.Equal(100m, trade.price);
        Assert.Equal(10m, trade.quantity);
        Assert.Equal(firstSellOrder.id, trade.sellOrderId);
        Assert.Equal(buyOrder.id, trade.buyOrderId);
        
        var sell = engine.SellOrders.First();
        Assert.Equal(100m, sell.price);
        Assert.Equal(10m, sell.quantity);
        Assert.Equal(secondSellOrder.id, sell.id);
    }
}