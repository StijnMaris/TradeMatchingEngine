
namespace TradeMatchingEngine.Domain.Models;

public record Trade
(
    Guid id,
    Guid buyOrderId,
    Guid sellOrderId,
    decimal quantity,
    decimal price,
    DateTime timestamp
);