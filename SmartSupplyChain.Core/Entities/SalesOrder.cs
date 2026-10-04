using System;
using System.Collections.Generic;
using SmartSupplyChain.Core.Common;

namespace SmartSupplyChain.Core.Entities;

public class SalesOrder : BaseAuditableEntity
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public Guid WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;

    public string Status { get; set; } = "Pending"; // Pending, Processing, Shipped, Delivered, Cancelled
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime? ExpectedDeliveryDate { get; set; }

    public ICollection<SalesOrderItem> SalesOrderItems { get; set; } = new List<SalesOrderItem>();
}

public class SalesOrderItem : BaseAuditableEntity
{
    public Guid Id { get; set; }
    public Guid SalesOrderId { get; set; }
    public SalesOrder SalesOrder { get; set; } = null!;

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int QuantityOrdered { get; set; }
    public int QuantityShipped { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}