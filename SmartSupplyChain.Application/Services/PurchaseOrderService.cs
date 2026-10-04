using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartSupplyChain.Core.DTOs.PurchaseOrder;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Exceptions;
using SmartSupplyChain.Core.Interface;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Application.Services;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public PurchaseOrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<PurchaseOrderResponseDto> CreateAsync(CreatePurchaseOrderDto dto, CancellationToken cancellationToken = default)
    {
        var supplier = await _unitOfWork.Suppliers.GetByIdAsync(dto.SupplierId, cancellationToken);
        if (supplier == null)
            throw new NotFoundException(nameof(Supplier), dto.SupplierId);

        var orderItems = new List<PurchaseOrderItem>();
        var purchaseOrderId = Guid.NewGuid();

        foreach (var itemDto in dto.Items)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(itemDto.ProductId, cancellationToken);
            if (product == null)
                throw new NotFoundException(nameof(Product), itemDto.ProductId);

            orderItems.Add(new PurchaseOrderItem
            {
                Id = Guid.NewGuid(),
                PurchaseOrderId = purchaseOrderId,
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                UnitCost = itemDto.UnitCost,
                CreatedAt = DateTime.UtcNow
            });
        }

        var order = new PurchaseOrder
        {
            Id = purchaseOrderId,
            SupplierId = dto.SupplierId,
            Status = dto.Status,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.PurchaseOrders.AddAsync(order, cancellationToken);
        await _unitOfWork.PurchaseOrderItems.AddRangeAsync(orderItems, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return await GetByIdAsync(order.Id, cancellationToken);
    }

    public async Task<PurchaseOrderResponseDto> UpdateStatusAsync(Guid id, UpdatePurchaseOrderStatusDto dto, CancellationToken cancellationToken = default)
    {
        var orders = await _unitOfWork.PurchaseOrders.FindAsync(
            o => o.Id == id,
            cancellationToken,
            o => o.Supplier,
            o => o.Items
        );

        var order = orders.FirstOrDefault();
        if (order == null)
            throw new NotFoundException(nameof(PurchaseOrder), id);

        order.Status = dto.Status;
        order.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.PurchaseOrders.Update(order);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return MapToResponseDto(order);
    }

    public async Task<PurchaseOrderResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var orders = await _unitOfWork.PurchaseOrders.FindAsync(
            o => o.Id == id,
            cancellationToken,
            o => o.Supplier,
            o => o.Items
        );

        var order = orders.FirstOrDefault();
        if (order == null)
            throw new NotFoundException(nameof(PurchaseOrder), id);

        return MapToResponseDto(order);
    }

    public async Task<IEnumerable<PurchaseOrderResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _unitOfWork.PurchaseOrders.GetAllAsync(
            cancellationToken,
            o => o.Supplier,
            o => o.Items
        );

        return orders.Select(MapToResponseDto);
    }

    public async Task<IEnumerable<PurchaseOrderResponseDto>> GetBySupplierAsync(Guid supplierId, CancellationToken cancellationToken = default)
    {
        var orders = await _unitOfWork.PurchaseOrders.FindAsync(
            o => o.SupplierId == supplierId,
            cancellationToken,
            o => o.Supplier,
            o => o.Items
        );

        return orders.Select(MapToResponseDto);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _unitOfWork.PurchaseOrders.GetByIdAsync(id, cancellationToken);
        if (order == null)
            throw new NotFoundException(nameof(PurchaseOrder), id);

        var items = await _unitOfWork.PurchaseOrderItems.FindAsync(i => i.PurchaseOrderId == id, cancellationToken);
        _unitOfWork.PurchaseOrderItems.RemoveRange(items);
        _unitOfWork.PurchaseOrders.Remove(order);

        return await _unitOfWork.CompleteAsync(cancellationToken) > 0;
    }

    private static PurchaseOrderResponseDto MapToResponseDto(PurchaseOrder order)
    {
        var itemsList = order.Items?.Select(item => new PurchaseOrderItemResponseDto
        {
            Id = item.Id,
            PurchaseOrderId = item.PurchaseOrderId,
            ProductId = item.ProductId,
            ProductName = item.Product?.Name,
            Quantity = item.Quantity,
            UnitCost = item.UnitCost
        }).ToList() ?? new List<PurchaseOrderItemResponseDto>();

        return new PurchaseOrderResponseDto
        {
            Id = order.Id,
            SupplierId = order.SupplierId,
            SupplierName = order.Supplier?.Name,
            Status = order.Status,
            TotalAmount = itemsList.Sum(i => i.LineTotal),
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            Items = itemsList
        };
    }
}