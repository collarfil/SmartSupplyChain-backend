using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartSupplyChain.Core.DTOs.InventoryItem;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Exceptions;
using SmartSupplyChain.Core.Interface;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Application.Services;

public class InventoryItemService : IInventoryItemService
{
    private readonly IUnitOfWork _unitOfWork;

    public InventoryItemService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<InventoryItemResponseDto> CreateAsync(CreateInventoryItemDto dto, CancellationToken cancellationToken = default)
    {
        await ValidateWarehouseAndProductAsync(dto.WarehouseId, dto.ProductId, cancellationToken);

        var item = new InventoryItem
        {
            Id = Guid.NewGuid(),
            WarehouseId = dto.WarehouseId,
            ProductId = dto.ProductId,
            QuantityAvailable = dto.QuantityAvailable,
            ReorderLevel = dto.ReorderLevel
        };

        await _unitOfWork.InventoryItems.AddAsync(item, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return await GetByIdAsync(item.Id, cancellationToken);
    }

    public async Task<InventoryItemResponseDto> UpsertStockAsync(CreateInventoryItemDto dto, CancellationToken cancellationToken = default)
    {
        await ValidateWarehouseAndProductAsync(dto.WarehouseId, dto.ProductId, cancellationToken);

        var existingItems = await _unitOfWork.InventoryItems.FindAsync(
            i => i.WarehouseId == dto.WarehouseId && i.ProductId == dto.ProductId,
            cancellationToken,
            i => i.Warehouse,
            i => i.Product
        );

        var item = existingItems.FirstOrDefault();

        if (item != null)
        {
            item.QuantityAvailable = dto.QuantityAvailable;
            item.ReorderLevel = dto.ReorderLevel;

            _unitOfWork.InventoryItems.Update(item);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return MapToResponseDto(item);
        }

        return await CreateAsync(dto, cancellationToken);
    }

    public async Task<InventoryItemResponseDto> UpdateStockAsync(Guid id, UpdateInventoryStockDto dto, CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.InventoryItems.FindAsync(
            i => i.Id == id,
            cancellationToken,
            i => i.Warehouse,
            i => i.Product
        );

        var item = items.FirstOrDefault();
        if (item == null)
            throw new NotFoundException(nameof(InventoryItem), id);

        item.QuantityAvailable = dto.QuantityAvailable;
        item.ReorderLevel = dto.ReorderLevel;

        _unitOfWork.InventoryItems.Update(item);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return MapToResponseDto(item);
    }

    public async Task<InventoryItemResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.InventoryItems.FindAsync(
            i => i.Id == id,
            cancellationToken,
            i => i.Warehouse,
            i => i.Product
        );

        var item = items.FirstOrDefault();
        if (item == null)
            throw new NotFoundException(nameof(InventoryItem), id);

        return MapToResponseDto(item);
    }

    public async Task<IEnumerable<InventoryItemResponseDto>> GetByWarehouseAsync(Guid warehouseId, CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.InventoryItems.FindAsync(
            i => i.WarehouseId == warehouseId,
            cancellationToken,
            i => i.Warehouse,
            i => i.Product
        );

        return items.Select(MapToResponseDto);
    }

    public async Task<IEnumerable<InventoryItemResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.InventoryItems.GetAllAsync(
            cancellationToken,
            i => i.Warehouse,
            i => i.Product
        );

        return items.Select(MapToResponseDto);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _unitOfWork.InventoryItems.GetByIdAsync(id, cancellationToken);
        if (item == null)
            throw new NotFoundException(nameof(InventoryItem), id);

        _unitOfWork.InventoryItems.Remove(item);
        return await _unitOfWork.CompleteAsync(cancellationToken) > 0;
    }

    private async Task ValidateWarehouseAndProductAsync(Guid warehouseId, Guid productId, CancellationToken cancellationToken)
    {
        var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(warehouseId, cancellationToken);
        if (warehouse == null)
            throw new NotFoundException(nameof(Warehouse), warehouseId);

        var product = await _unitOfWork.Products.GetByIdAsync(productId, cancellationToken);
        if (product == null)
            throw new NotFoundException(nameof(Product), productId);
    }

    private static InventoryItemResponseDto MapToResponseDto(InventoryItem item) => new()
    {
        Id = item.Id,
        WarehouseId = item.WarehouseId,
        WarehouseName = item.Warehouse?.Name ?? string.Empty,
        ProductId = item.ProductId,
        ProductName = item.Product?.Name ?? string.Empty,
        QuantityAvailable = item.QuantityAvailable,
        ReorderLevel = item.ReorderLevel,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt
    };
}