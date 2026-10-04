using System;
using SmartSupplyChain.Core.Common;

namespace SmartSupplyChain.Core.Entities;

public class QualityInspection : BaseAuditableEntity
{
    public Guid Id { get; set; }
    public string InspectionNumber { get; set; } = string.Empty;
    public Guid SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;

    public Guid? PurchaseOrderId { get; set; }
    public PurchaseOrder? PurchaseOrder { get; set; }

    public string InspectorName { get; set; } = string.Empty;
    public bool IsPassed { get; set; }
    public int SampleSize { get; set; }
    public int DefectCount { get; set; }
    public string Remarks { get; set; } = string.Empty;
    public DateTime InspectionDate { get; set; } = DateTime.UtcNow;
}