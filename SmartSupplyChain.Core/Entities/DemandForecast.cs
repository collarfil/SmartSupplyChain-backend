using System;
using SmartSupplyChain.Core.Common;

namespace SmartSupplyChain.Core.Entities;

public class DemandForecast : BaseAuditableEntity
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public Guid WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;

    public int CurrentStockLevel { get; set; }
    public int PredictedDemandQuantity { get; set; }
    public int RecommendedReorderQuantity { get; set; }
    public decimal ConfidenceScore { get; set; } // e.g., 0.92 for 92% confidence
    public DateTime ForecastPeriodStart { get; set; }
    public DateTime ForecastPeriodEnd { get; set; }
    public string ModelVersion { get; set; } = "v1.0-AI";
}