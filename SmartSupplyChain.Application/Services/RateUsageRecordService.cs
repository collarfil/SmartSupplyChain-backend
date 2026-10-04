using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartSupplyChain.Core.DTOs.RateUsageRecord;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Exceptions;
using SmartSupplyChain.Core.Interface;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Application.Services;

public class RateUsageRecordService : IRateUsageRecordService
{
    private readonly IUnitOfWork _unitOfWork;

    public RateUsageRecordService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<RateUsageRecordResponseDto> CreateAsync(CreateRateUsageRecordDto dto, CancellationToken cancellationToken = default)
    {
        var ratePlan = await _unitOfWork.RatePlans.GetByIdAsync(dto.RatePlanId, cancellationToken);
        if (ratePlan == null)
            throw new NotFoundException(nameof(RatePlan), dto.RatePlanId);

        var shipment = await _unitOfWork.Shipments.GetByIdAsync(dto.ShipmentId, cancellationToken);
        if (shipment == null)
            throw new NotFoundException(nameof(Shipment), dto.ShipmentId);

        decimal distanceCost = dto.TotalDistanceKm * ratePlan.CostPerKm;
        decimal weightCost = dto.TotalWeightKg * ratePlan.CostPerKg;
        decimal calculatedTotalCost = Math.Round(distanceCost + weightCost, 2, MidpointRounding.AwayFromZero);

        var record = new RateUsageRecord
        {
            Id = Guid.NewGuid(),
            RatePlanId = dto.RatePlanId,
            ShipmentId = dto.ShipmentId,
            TotalDistanceKm = dto.TotalDistanceKm,
            TotalWeightKg = dto.TotalWeightKg,
            TotalCost = calculatedTotalCost,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.RateUsageRecords.AddAsync(record, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return await GetByIdAsync(record.Id, cancellationToken);
    }

    public async Task<RateUsageRecordResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var records = await _unitOfWork.RateUsageRecords.FindAsync(
            r => r.Id == id,
            cancellationToken,
            r => r.RatePlan,
            r => r.Shipment
        );

        var record = records.FirstOrDefault();
        if (record == null)
            throw new NotFoundException(nameof(RateUsageRecord), id);

        return MapToResponseDto(record);
    }

    public async Task<IEnumerable<RateUsageRecordResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _unitOfWork.RateUsageRecords.GetAllAsync(
            cancellationToken,
            r => r.RatePlan,
            r => r.Shipment
        );

        return records.Select(MapToResponseDto);
    }

    public async Task<IEnumerable<RateUsageRecordResponseDto>> GetByShipmentAsync(Guid shipmentId, CancellationToken cancellationToken = default)
    {
        var records = await _unitOfWork.RateUsageRecords.FindAsync(
            r => r.ShipmentId == shipmentId,
            cancellationToken,
            r => r.RatePlan,
            r => r.Shipment
        );

        return records.Select(MapToResponseDto);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var record = await _unitOfWork.RateUsageRecords.GetByIdAsync(id, cancellationToken);
        if (record == null)
            throw new NotFoundException(nameof(RateUsageRecord), id);

        _unitOfWork.RateUsageRecords.Remove(record);
        return await _unitOfWork.CompleteAsync(cancellationToken) > 0;
    }

    private static RateUsageRecordResponseDto MapToResponseDto(RateUsageRecord record) => new()
    {
        Id = record.Id,
        RatePlanId = record.RatePlanId,
        RatePlanName = record.RatePlan?.Name ?? string.Empty,
        ShipmentId = record.ShipmentId,
        TotalDistanceKm = record.TotalDistanceKm,
        TotalWeightKg = record.TotalWeightKg,
        TotalCost = record.TotalCost,
        CreatedAt = record.CreatedAt,
        UpdatedAt = record.UpdatedAt
    };
}