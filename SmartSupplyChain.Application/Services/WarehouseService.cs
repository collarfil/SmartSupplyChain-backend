using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartSupplyChain.Core.DTOs.Warehouse;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Exceptions;
using SmartSupplyChain.Core.Interface;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Application.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IUnitOfWork _unitOfWork;

    public WarehouseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<WarehouseResponseDto> UpsertAsync(Guid? id, CreateWarehouseDto dto, CancellationToken cancellationToken = default)
    {
        var company = await _unitOfWork.Companies.GetByIdAsync(dto.CompanyId, cancellationToken);
        if (company == null)
            throw new NotFoundException(nameof(Company), dto.CompanyId);

        var existingName = await _unitOfWork.Warehouses.FindAsync(
            w => w.CompanyId == dto.CompanyId && w.Name.ToLower() == dto.Name.ToLower() && (!id.HasValue || w.Id != id.Value),
            cancellationToken
        );

        if (existingName.Any())
            throw new InvalidOperationException($"Warehouse with name '{dto.Name}' already exists for this company.");

        Warehouse warehouse = null;

        if (id.HasValue && id.Value != Guid.Empty)
        {
            var warehouses = await _unitOfWork.Warehouses.FindAsync(w => w.Id == id.Value, cancellationToken, w => w.Company);
            warehouse = warehouses.FirstOrDefault();
        }

        if (warehouse == null)
        {
            warehouse = new Warehouse
            {
                Id = id.HasValue && id.Value != Guid.Empty ? id.Value : Guid.NewGuid(),
                CompanyId = dto.CompanyId,
                Name = dto.Name,
                Location = dto.Location,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Warehouses.AddAsync(warehouse, cancellationToken);
        }
        else
        {
            warehouse.CompanyId = dto.CompanyId;
            warehouse.Name = dto.Name;
            warehouse.Location = dto.Location;
            warehouse.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Warehouses.Update(warehouse);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);
        return await GetByIdAsync(warehouse.Id, cancellationToken);
    }

    public async Task<WarehouseResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var warehouses = await _unitOfWork.Warehouses.FindAsync(
            w => w.Id == id,
            cancellationToken,
            w => w.Company
        );

        var warehouse = warehouses.FirstOrDefault();
        if (warehouse == null)
            throw new NotFoundException(nameof(Warehouse), id);

        return MapToResponseDto(warehouse);
    }

    public async Task<IEnumerable<WarehouseResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var warehouses = await _unitOfWork.Warehouses.GetAllAsync(
            cancellationToken,
            w => w.Company
        );

        return warehouses.Select(MapToResponseDto);
    }

    public async Task<IEnumerable<WarehouseResponseDto>> GetByCompanyAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        var warehouses = await _unitOfWork.Warehouses.FindAsync(
            w => w.CompanyId == companyId,
            cancellationToken,
            w => w.Company
        );

        return warehouses.Select(MapToResponseDto);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(id, cancellationToken);
        if (warehouse == null)
            throw new NotFoundException(nameof(Warehouse), id);

        _unitOfWork.Warehouses.Remove(warehouse);
        return await _unitOfWork.CompleteAsync(cancellationToken) > 0;
    }

    private static WarehouseResponseDto MapToResponseDto(Warehouse warehouse) => new()
    {
        Id = warehouse.Id,
        CompanyId = warehouse.CompanyId,
        CompanyName = warehouse.Company?.Name ?? string.Empty,
        Name = warehouse.Name,
        Location = warehouse.Location,
        CreatedAt = warehouse.CreatedAt,
        UpdatedAt = warehouse.UpdatedAt
    };
}