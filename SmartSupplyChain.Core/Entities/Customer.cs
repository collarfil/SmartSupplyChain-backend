using System;
using System.Collections.Generic;
using SmartSupplyChain.Core.Common;

namespace SmartSupplyChain.Core.Entities;

public class Customer : BaseAuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string TaxIdentificationNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public ICollection<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();
}