using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartSupplyChain.Core.DTOs.InventoryItem;

namespace SmartSupplyChain.Core.Interfaces;

public interface IInventoryItemService
{
    Task<InventoryItemResponseDto> CreateAsync(CreateInventoryItemDto dto, CancellationToken cancellationToken = default);
    Task<InventoryItemResponseDto> UpsertStockAsync(CreateInventoryItemDto dto, CancellationToken cancellationToken = default);
    Task<InventoryItemResponseDto> UpdateStockAsync(Guid id, UpdateInventoryStockDto dto, CancellationToken cancellationToken = default);
    Task<InventoryItemResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<InventoryItemResponseDto>> GetByWarehouseAsync(Guid warehouseId, CancellationToken cancellationToken = default);
    Task<IEnumerable<InventoryItemResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}