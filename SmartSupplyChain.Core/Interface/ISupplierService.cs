using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartSupplyChain.Core.DTOs.Supplier;

namespace SmartSupplyChain.Core.Interfaces;

public interface ISupplierService
{
    Task<SupplierResponseDto> UpsertAsync(Guid? id, CreateSupplierDto dto, CancellationToken cancellationToken = default);
    Task<SupplierResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<SupplierResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}