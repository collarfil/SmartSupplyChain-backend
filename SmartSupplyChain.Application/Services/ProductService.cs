using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartSupplyChain.Core.DTOs.Product;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Exceptions;
using SmartSupplyChain.Core.Interface;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default)
    {
        var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(dto.WarehouseId, cancellationToken);
        if (warehouse == null)
            throw new NotFoundException(nameof(Warehouse), dto.WarehouseId);

        var existingSku = await _unitOfWork.Products.FindAsync(p => p.SKU == dto.SKU, cancellationToken);
        if (existingSku.Any())
            throw new InvalidOperationException($"Product with SKU '{dto.SKU}' already exists.");

        var product = new Product
        {
            Id = Guid.NewGuid(),
            WarehouseId = dto.WarehouseId,
            Name = dto.Name,
            SKU = dto.SKU,
            UnitCost = dto.UnitCost,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Products.AddAsync(product, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return await GetByIdAsync(product.Id, cancellationToken);
    }

    public async Task<ProductResponseDto> UpdateAsync(Guid id, UpdateProductDto dto, CancellationToken cancellationToken = default)
    {
        var products = await _unitOfWork.Products.FindAsync(
            p => p.Id == id,
            cancellationToken,
            p => p.Warehouse
        );

        var product = products.FirstOrDefault();
        if (product == null)
            throw new NotFoundException(nameof(Product), id);

        var duplicateSku = await _unitOfWork.Products.FindAsync(p => p.SKU == dto.SKU && p.Id != id, cancellationToken);
        if (duplicateSku.Any())
            throw new InvalidOperationException($"Product with SKU '{dto.SKU}' already exists.");

        product.Name = dto.Name;
        product.SKU = dto.SKU;
        product.UnitCost = dto.UnitCost;
        product.Description = dto.Description;
        product.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Products.Update(product);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return MapToResponseDto(product);
    }

    public async Task<ProductResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var products = await _unitOfWork.Products.FindAsync(
            p => p.Id == id,
            cancellationToken,
            p => p.Warehouse
        );

        var product = products.FirstOrDefault();
        if (product == null)
            throw new NotFoundException(nameof(Product), id);

        return MapToResponseDto(product);
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await _unitOfWork.Products.GetAllAsync(
            cancellationToken,
            p => p.Warehouse
        );

        return products.Select(MapToResponseDto);
    }

    public async Task<IEnumerable<ProductResponseDto>> GetByWarehouseAsync(Guid warehouseId, CancellationToken cancellationToken = default)
    {
        var products = await _unitOfWork.Products.FindAsync(
            p => p.WarehouseId == warehouseId,
            cancellationToken,
            p => p.Warehouse
        );

        return products.Select(MapToResponseDto);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id, cancellationToken);
        if (product == null)
            throw new NotFoundException(nameof(Product), id);

        _unitOfWork.Products.Remove(product);
        return await _unitOfWork.CompleteAsync(cancellationToken) > 0;
    }

    private static ProductResponseDto MapToResponseDto(Product product) => new()
    {
        Id = product.Id,
        WarehouseId = product.WarehouseId,
        WarehouseName = product.Warehouse?.Name ?? string.Empty,
        Name = product.Name,
        SKU = product.SKU,
        UnitCost = product.UnitCost,
        Description = product.Description,
        CreatedAt = product.CreatedAt,
        UpdatedAt = product.UpdatedAt
    };
}