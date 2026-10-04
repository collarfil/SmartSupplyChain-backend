using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartSupplyChain.Core.DTOs.Supplier;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Exceptions;
using SmartSupplyChain.Core.Interface;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly IUnitOfWork _unitOfWork;

    public SupplierService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<SupplierResponseDto> UpsertAsync(Guid? id, CreateSupplierDto dto, CancellationToken cancellationToken = default)
    {
        var existingEmail = await _unitOfWork.Suppliers.FindAsync(
            s => s.ContactEmail.ToLower() == dto.ContactEmail.ToLower() && (!id.HasValue || s.Id != id.Value),
            cancellationToken
        );

        if (existingEmail.Any())
            throw new InvalidOperationException($"Supplier with email '{dto.ContactEmail}' already exists.");

        Supplier supplier = null;

        if (id.HasValue && id.Value != Guid.Empty)
        {
            var suppliers = await _unitOfWork.Suppliers.FindAsync(s => s.Id == id.Value, cancellationToken);
            supplier = suppliers.FirstOrDefault();
        }

        if (supplier == null)
        {
            supplier = new Supplier
            {
                Id = id.HasValue && id.Value != Guid.Empty ? id.Value : Guid.NewGuid(),
                Name = dto.Name,
                ContactEmail = dto.ContactEmail,
                ContactPhone = dto.ContactPhone,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Suppliers.AddAsync(supplier, cancellationToken);
        }
        else
        {
            supplier.Name = dto.Name;
            supplier.ContactEmail = dto.ContactEmail;
            supplier.ContactPhone = dto.ContactPhone;
            supplier.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Suppliers.Update(supplier);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);
        return MapToResponseDto(supplier);
    }

    public async Task<SupplierResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var suppliers = await _unitOfWork.Suppliers.FindAsync(s => s.Id == id, cancellationToken);
        var supplier = suppliers.FirstOrDefault();

        if (supplier == null)
            throw new NotFoundException(nameof(Supplier), id);

        return MapToResponseDto(supplier);
    }

    public async Task<IEnumerable<SupplierResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var suppliers = await _unitOfWork.Suppliers.GetAllAsync(cancellationToken);
        return suppliers.Select(MapToResponseDto);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var supplier = await _unitOfWork.Suppliers.GetByIdAsync(id, cancellationToken);
        if (supplier == null)
            throw new NotFoundException(nameof(Supplier), id);

        _unitOfWork.Suppliers.Remove(supplier);
        return await _unitOfWork.CompleteAsync(cancellationToken) > 0;
    }

    private static SupplierResponseDto MapToResponseDto(Supplier supplier) => new()
    {
        Id = supplier.Id,
        Name = supplier.Name,
        ContactEmail = supplier.ContactEmail,
        ContactPhone = supplier.ContactPhone,
        CreatedAt = supplier.CreatedAt,
        UpdatedAt = supplier.UpdatedAt
    };
}