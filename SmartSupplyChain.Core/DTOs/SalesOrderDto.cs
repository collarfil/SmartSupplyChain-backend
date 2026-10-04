using System;
using System.Collections.Generic;

namespace SmartSupplyChain.Core.DTOs;

public record SalesOrderDto(
    Guid Id,
    string OrderNumber,
    Guid CustomerId,
    string CustomerName,
    Guid WarehouseId,
    string Status,
    decimal TotalAmount,
    DateTime OrderDate,
    DateTime? ExpectedDeliveryDate,
    List<SalesOrderItemDto> SalesOrderItems,
    DateTime CreatedAt
);

public record CreateSalesOrderDto(
    string OrderNumber,
    Guid CustomerId,
    Guid WarehouseId,
    DateTime? ExpectedDeliveryDate,
    List<CreateSalesOrderItemDto> SalesOrderItems
);