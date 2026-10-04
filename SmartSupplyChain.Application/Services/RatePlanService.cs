using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartSupplyChain.Core.DTOs.RatePlan;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Exceptions;
using SmartSupplyChain.Core.Interface;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Application.Services;

public class RatePlanService : IRatePlanService
{
    private readonly IUnitOfWork _unitOfWork;

    public RatePlanService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<RatePlanResponseDto> UpsertAsync(Guid? id, CreateRatePlanDto dto, CancellationToken cancellationToken = default)
    {
        RatePlan? ratePlan = null;

        if (id.HasValue && id.Value != Guid.Empty)
        {
            ratePlan = await _unitOfWork.RatePlans.GetByIdAsync(id.Value, cancellationToken);
        }

        if (ratePlan != null)
        {
            ratePlan.Name = dto.Name;
            ratePlan.CostPerKm = dto.CostPerKm;
            ratePlan.CostPerKg = dto.CostPerKg;
            ratePlan.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.RatePlans.Update(ratePlan);
        }
        else
        {
            ratePlan = new RatePlan
            {
                Id = id ?? Guid.NewGuid(),
                Name = dto.Name,
                CostPerKm = dto.CostPerKm,
                CostPerKg = dto.CostPerKg,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.RatePlans.AddAsync(ratePlan, cancellationToken);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);
        return MapToResponseDto(ratePlan);
    }

    public async Task<RatePlanResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var ratePlan = await _unitOfWork.RatePlans.GetByIdAsync(id, cancellationToken);
        if (ratePlan == null)
            throw new NotFoundException(nameof(RatePlan), id);

        return MapToResponseDto(ratePlan);
    }

    public async Task<IEnumerable<RatePlanResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var ratePlans = await _unitOfWork.RatePlans.GetAllAsync(cancellationToken);
        return ratePlans.Select(MapToResponseDto);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var ratePlan = await _unitOfWork.RatePlans.GetByIdAsync(id, cancellationToken);
        if (ratePlan == null)
            throw new NotFoundException(nameof(RatePlan), id);

        _unitOfWork.RatePlans.Remove(ratePlan);
        return await _unitOfWork.CompleteAsync(cancellationToken) > 0;
    }

    private static RatePlanResponseDto MapToResponseDto(RatePlan ratePlan) => new()
    {
        Id = ratePlan.Id,
        Name = ratePlan.Name,
        CostPerKm = ratePlan.CostPerKm,
        CostPerKg = ratePlan.CostPerKg,
        CreatedAt = ratePlan.CreatedAt,
        UpdatedAt = ratePlan.UpdatedAt
    };
}