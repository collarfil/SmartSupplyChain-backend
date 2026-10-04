using System;

namespace SmartSupplyChain.Core.DTOs;

public record DemandForecastDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    Guid WarehouseId,
    string WarehouseName,
    int CurrentStockLevel,
    int PredictedDemandQuantity,
    int RecommendedReorderQuantity,
    decimal ConfidenceScore,
    DateTime ForecastPeriodStart,
    DateTime ForecastPeriodEnd,
    string ModelVersion,
    DateTime CreatedAt
);

public record CreateDemandForecastDto(
    Guid ProductId,
    Guid WarehouseId,
    int CurrentStockLevel,
    int PredictedDemandQuantity,
    int RecommendedReorderQuantity,
    decimal ConfidenceScore,
    DateTime ForecastPeriodStart,
    DateTime ForecastPeriodEnd,
    string ModelVersion
);