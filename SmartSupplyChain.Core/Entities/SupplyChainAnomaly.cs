using System;
using SmartSupplyChain.Core.Common;

namespace SmartSupplyChain.Core.Entities;

public class SupplyChainAnomaly : BaseAuditableEntity
{
    public Guid Id { get; set; }
    public string AnomalyType { get; set; } = string.Empty; // e.g., "TemperatureSpike", "RouteDeviation", "DelayRisk"
    public string Severity { get; set; } = "Medium";       // Low, Medium, High, Critical
    public string Description { get; set; } = string.Empty;
    public bool IsResolved { get; set; } = false;
    public DateTime? ResolvedAt { get; set; }
    public string? ResolutionNotes { get; set; }

    // Optional cross-references
    public Guid? ShipmentId { get; set; }
    public Shipment? Shipment { get; set; }
    public Guid? InventoryItemId { get; set; }
    public InventoryItem? InventoryItem { get; set; }
}