using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartSupplyChain.Core.DTOs.PurchaseOrder;

namespace SmartSupplyChain.Core.Interfaces;

public interface IPurchaseOrderService
{
    Task<PurchaseOrderResponseDto> CreateAsync(CreatePurchaseOrderDto dto, CancellationToken cancellationToken = default);
    Task<PurchaseOrderResponseDto> UpdateStatusAsync(Guid id, UpdatePurchaseOrderStatusDto dto, CancellationToken cancellationToken = default);
    Task<PurchaseOrderResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PurchaseOrderResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<PurchaseOrderResponseDto>> GetBySupplierAsync(Guid supplierId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}