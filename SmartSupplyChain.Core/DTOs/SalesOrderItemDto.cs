using System;

namespace SmartSupplyChain.Core.DTOs;

public record SalesOrderItemDto(
    Guid Id,
    Guid SalesOrderId,
    Guid ProductId,
    string ProductName,
    int QuantityOrdered,
    int QuantityShipped,
    decimal UnitPrice,
    decimal TotalPrice
);

public record CreateSalesOrderItemDto(
    Guid ProductId,
    int QuantityOrdered,
    decimal UnitPrice
);