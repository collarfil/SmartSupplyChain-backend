using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartSupplyChain.Core.DTOs.Product;

namespace SmartSupplyChain.Core.Interfaces;

public interface IProductService
{
    Task<ProductResponseDto> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default);
    Task<ProductResponseDto> UpdateAsync(Guid id, UpdateProductDto dto, CancellationToken cancellationToken = default);
    Task<ProductResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductResponseDto>> GetByWarehouseAsync(Guid warehouseId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}