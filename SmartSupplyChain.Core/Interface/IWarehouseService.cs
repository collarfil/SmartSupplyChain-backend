using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartSupplyChain.Core.DTOs.Warehouse;

namespace SmartSupplyChain.Core.Interfaces;

public interface IWarehouseService
{
    Task<WarehouseResponseDto> UpsertAsync(Guid? id, CreateWarehouseDto dto, CancellationToken cancellationToken = default);
    Task<WarehouseResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<WarehouseResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<WarehouseResponseDto>> GetByCompanyAsync(Guid companyId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}