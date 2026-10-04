using System;

namespace SmartSupplyChain.Core.Common;

public abstract class BaseAuditableEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}