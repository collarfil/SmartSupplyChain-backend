using System;

namespace SmartSupplyChain.Core.DTOs;

public record SupplyChainAnomalyDto(
    Guid Id,
    string AnomalyType,
    string Severity,
    string Description,
    bool IsResolved,
    DateTime? ResolvedAt,
    string? ResolutionNotes,
    Guid? ShipmentId,
    Guid? InventoryItemId,
    DateTime CreatedAt
);

public record CreateSupplyChainAnomalyDto(
    string AnomalyType,
    string Severity,
    string Description,
    Guid? ShipmentId,
    Guid? InventoryItemId
);

public record ResolveAnomalyDto(
    string ResolutionNotes
);