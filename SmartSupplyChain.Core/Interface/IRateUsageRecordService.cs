using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartSupplyChain.Core.DTOs.RateUsageRecord;

namespace SmartSupplyChain.Core.Interfaces;

public interface IRateUsageRecordService
{
    Task<RateUsageRecordResponseDto> CreateAsync(CreateRateUsageRecordDto dto, CancellationToken cancellationToken = default);
    Task<RateUsageRecordResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<RateUsageRecordResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<RateUsageRecordResponseDto>> GetByShipmentAsync(Guid shipmentId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}