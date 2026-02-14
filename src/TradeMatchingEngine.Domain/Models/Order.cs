
using TradeMatchingEngine.Domain.Enums;

namespace TradeMatchingEngine.Domain.Models;
public record Order
(
    Guid id,
    OrderType orderType,
    decimal price,
    decimal quantity,
    DateTime timestamp
);