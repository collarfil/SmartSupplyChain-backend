using System;

namespace SmartSupplyChain.Core.DTOs;

public record QualityInspectionDto(
    Guid Id,
    string InspectionNumber,
    Guid SupplierId,
    string SupplierName,
    Guid? PurchaseOrderId,
    string InspectorName,
    bool IsPassed,
    int SampleSize,
    int DefectCount,
    string Remarks,
    DateTime InspectionDate,
    DateTime CreatedAt
);

public record CreateQualityInspectionDto(
    string InspectionNumber,
    Guid SupplierId,
    Guid? PurchaseOrderId,
    string InspectorName,
    bool IsPassed,
    int SampleSize,
    int DefectCount,
    string Remarks,
    DateTime InspectionDate
);