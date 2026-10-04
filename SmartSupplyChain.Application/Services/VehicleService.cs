using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartSupplyChain.Core.DTOs.Vehicle;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Exceptions;
using SmartSupplyChain.Core.Interface;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Application.Services;

public class VehicleService : IVehicleService
{
    private readonly IUnitOfWork _unitOfWork;

    public VehicleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<VehicleResponseDto> UpsertAsync(Guid? id, CreateVehicleDto dto, CancellationToken cancellationToken = default)
    {
        var company = await _unitOfWork.Companies.GetByIdAsync(dto.CompanyId, cancellationToken);
        if (company == null)
            throw new NotFoundException(nameof(Company), dto.CompanyId);

        var existingPlate = await _unitOfWork.Vehicles.FindAsync(
            v => v.PlateNumber.ToLower() == dto.PlateNumber.ToLower() && (!id.HasValue || v.Id != id.Value),
            cancellationToken
        );

        if (existingPlate.Any())
            throw new InvalidOperationException($"Vehicle with Plate Number '{dto.PlateNumber}' already exists.");

        Vehicle vehicle = null;

        if (id.HasValue && id.Value != Guid.Empty)
        {
            var vehicles = await _unitOfWork.Vehicles.FindAsync(v => v.Id == id.Value, cancellationToken, v => v.Company);
            vehicle = vehicles.FirstOrDefault();
        }

        if (vehicle == null)
        {
            vehicle = new Vehicle
            {
                Id = id.HasValue && id.Value != Guid.Empty ? id.Value : Guid.NewGuid(),
                CompanyId = dto.CompanyId,
                PlateNumber = dto.PlateNumber,
                Model = dto.Model,
                Year = dto.Year,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Vehicles.AddAsync(vehicle, cancellationToken);
        }
        else
        {
            vehicle.CompanyId = dto.CompanyId;
            vehicle.PlateNumber = dto.PlateNumber;
            vehicle.Model = dto.Model;
            vehicle.Year = dto.Year;
            vehicle.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Vehicles.Update(vehicle);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);
        return await GetByIdAsync(vehicle.Id, cancellationToken);
    }

    public async Task<VehicleResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var vehicles = await _unitOfWork.Vehicles.FindAsync(
            v => v.Id == id,
            cancellationToken,
            v => v.Company
        );

        var vehicle = vehicles.FirstOrDefault();
        if (vehicle == null)
            throw new NotFoundException(nameof(Vehicle), id);

        return MapToResponseDto(vehicle);
    }

    public async Task<IEnumerable<VehicleResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var vehicles = await _unitOfWork.Vehicles.GetAllAsync(
            cancellationToken,
            v => v.Company
        );

        return vehicles.Select(MapToResponseDto);
    }

    public async Task<IEnumerable<VehicleResponseDto>> GetByCompanyAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        var vehicles = await _unitOfWork.Vehicles.FindAsync(
            v => v.CompanyId == companyId,
            cancellationToken,
            v => v.Company
        );

        return vehicles.Select(MapToResponseDto);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(id, cancellationToken);
        if (vehicle == null)
            throw new NotFoundException(nameof(Vehicle), id);

        _unitOfWork.Vehicles.Remove(vehicle);
        return await _unitOfWork.CompleteAsync(cancellationToken) > 0;
    }

    private static VehicleResponseDto MapToResponseDto(Vehicle vehicle) => new()
    {
        Id = vehicle.Id,
        CompanyId = vehicle.CompanyId,
        CompanyName = vehicle.Company?.Name ?? string.Empty,
        PlateNumber = vehicle.PlateNumber,
        Model = vehicle.Model,
        Year = vehicle.Year,
        CreatedAt = vehicle.CreatedAt,
        UpdatedAt = vehicle.UpdatedAt
    };
}