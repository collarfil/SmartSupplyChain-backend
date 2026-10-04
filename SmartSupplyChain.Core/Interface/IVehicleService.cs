using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartSupplyChain.Core.DTOs.Vehicle;

namespace SmartSupplyChain.Core.Interfaces;

public interface IVehicleService
{
    Task<VehicleResponseDto> UpsertAsync(Guid? id, CreateVehicleDto dto, CancellationToken cancellationToken = default);
    Task<VehicleResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<VehicleResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<VehicleResponseDto>> GetByCompanyAsync(Guid companyId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}