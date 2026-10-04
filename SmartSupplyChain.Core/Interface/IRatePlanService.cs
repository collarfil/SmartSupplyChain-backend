using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartSupplyChain.Core.DTOs.RatePlan;

namespace SmartSupplyChain.Core.Interfaces;

public interface IRatePlanService
{
    Task<RatePlanResponseDto> UpsertAsync(Guid? id, CreateRatePlanDto dto, CancellationToken cancellationToken = default);
    Task<RatePlanResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<RatePlanResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}