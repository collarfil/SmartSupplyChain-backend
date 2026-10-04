using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartSupplyChain.Core.DTOs.Shipment;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Exceptions;
using SmartSupplyChain.Core.Interface;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Application.Services;

public class ShipmentService : IShipmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public ShipmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<ShipmentResponseDto> UpsertAsync(Guid? id, CreateShipmentDto dto, CancellationToken cancellationToken = default)
    {
        var company = await _unitOfWork.Companies.GetByIdAsync(dto.CompanyId, cancellationToken);
        if (company == null)
            throw new NotFoundException(nameof(Company), dto.CompanyId);

        var driver = await _unitOfWork.Drivers.GetByIdAsync(dto.DriverId, cancellationToken);
        if (driver == null)
            throw new NotFoundException(nameof(Driver), dto.DriverId);

        var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(dto.VehicleId, cancellationToken);
        if (vehicle == null)
            throw new NotFoundException(nameof(Vehicle), dto.VehicleId);

      Shipment? shipment = null;

        if (id.HasValue && id.Value != Guid.Empty)
        {
            var shipments = await _unitOfWork.Shipments.FindAsync(
                s => s.Id == id.Value,
                cancellationToken,
                s => s.Items,
                s => s.TrackingEvents
            );
            shipment = shipments.FirstOrDefault();
        }

        if (shipment == null)
        {
            shipment = new Shipment
            {
                Id = id.HasValue && id.Value != Guid.Empty ? id.Value : Guid.NewGuid(),
                CompanyId = dto.CompanyId,
                DriverId = dto.DriverId,
                VehicleId = dto.VehicleId,
                PickupDate = dto.PickupDate,
                DeliveryDate = dto.DeliveryDate,
                Status = dto.Status,
                CreatedAt = DateTime.UtcNow,
                Items = dto.Items.Select(item => new ShipmentItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    CreatedAt = DateTime.UtcNow
                }).ToList()
            };

            await _unitOfWork.Shipments.AddAsync(shipment, cancellationToken);
        }
        else
        {
            shipment.CompanyId = dto.CompanyId;
            shipment.DriverId = dto.DriverId;
            shipment.VehicleId = dto.VehicleId;
            shipment.PickupDate = dto.PickupDate;
            shipment.DeliveryDate = dto.DeliveryDate;
            shipment.Status = dto.Status;
            shipment.UpdatedAt = DateTime.UtcNow;

            shipment.Items.Clear();
            foreach (var item in dto.Items)
            {
                shipment.Items.Add(new ShipmentItem
                {
                    Id = Guid.NewGuid(),
                    ShipmentId = shipment.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    CreatedAt = DateTime.UtcNow
                });
            }

            _unitOfWork.Shipments.Update(shipment);
        }

        await _unitOfWork.CompleteAsync(cancellationToken);
        return await GetByIdAsync(shipment.Id, cancellationToken);
    }

    public async Task<ShipmentResponseDto> UpdateStatusAsync(Guid id, UpdateShipmentStatusDto dto, CancellationToken cancellationToken = default)
    {
        var shipments = await _unitOfWork.Shipments.FindAsync(s => s.Id == id, cancellationToken);
        var shipment = shipments.FirstOrDefault();
        if (shipment == null)
            throw new NotFoundException(nameof(Shipment), id);

        shipment.Status = dto.Status;
        if (dto.DeliveryDate.HasValue)
            shipment.DeliveryDate = dto.DeliveryDate;

        shipment.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Shipments.Update(shipment);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<ShipmentResponseDto> AddTrackingEventAsync(Guid id, CreateShipmentTrackingEventDto dto, CancellationToken cancellationToken = default)
    {
        var shipments = await _unitOfWork.Shipments.FindAsync(s => s.Id == id, cancellationToken, s => s.TrackingEvents);
        var shipment = shipments.FirstOrDefault();
        if (shipment == null)
            throw new NotFoundException(nameof(Shipment), id);

        shipment.TrackingEvents.Add(new ShipmentTrackingEvent
        {
            Id = Guid.NewGuid(),
            ShipmentId = id,
            EventType = dto.EventType,
            Location = dto.Location,
            OccurredAt = dto.OccurredAt,
            CreatedAt = DateTime.UtcNow
        });

        _unitOfWork.Shipments.Update(shipment);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<ShipmentResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var shipments = await _unitOfWork.Shipments.FindAsync(
            s => s.Id == id,
            cancellationToken,
            s => s.Company,
            s => s.Driver,
            s => s.Vehicle,
            s => s.Items,
            s => s.TrackingEvents
        );

        var shipment = shipments.FirstOrDefault();
        if (shipment == null)
            throw new NotFoundException(nameof(Shipment), id);

        return MapToResponseDto(shipment);
    }

    public async Task<IEnumerable<ShipmentResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var shipments = await _unitOfWork.Shipments.GetAllAsync(
            cancellationToken,
            s => s.Company,
            s => s.Driver,
            s => s.Vehicle,
            s => s.Items,
            s => s.TrackingEvents
        );

        return shipments.Select(MapToResponseDto);
    }

    public async Task<IEnumerable<ShipmentResponseDto>> GetByCompanyAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        var shipments = await _unitOfWork.Shipments.FindAsync(
            s => s.CompanyId == companyId,
            cancellationToken,
            s => s.Company,
            s => s.Driver,
            s => s.Vehicle,
            s => s.Items,
            s => s.TrackingEvents
        );

        return shipments.Select(MapToResponseDto);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var shipment = await _unitOfWork.Shipments.GetByIdAsync(id, cancellationToken);
        if (shipment == null)
            throw new NotFoundException(nameof(Shipment), id);

        _unitOfWork.Shipments.Remove(shipment);
        return await _unitOfWork.CompleteAsync(cancellationToken) > 0;
    }

    private static ShipmentResponseDto MapToResponseDto(Shipment shipment) => new()
    {
        Id = shipment.Id,
        CompanyId = shipment.CompanyId,
        CompanyName = shipment.Company?.Name ?? string.Empty,
        DriverId = shipment.DriverId,
        DriverName = shipment.Driver?.FullName ?? string.Empty,
        VehicleId = shipment.VehicleId,
        VehiclePlateNumber = shipment.Vehicle?.PlateNumber ?? string.Empty,
        PickupDate = shipment.PickupDate,
        DeliveryDate = shipment.DeliveryDate,
        Status = shipment.Status ?? string.Empty,
        Items = shipment.Items?.Select(i => new ShipmentItemResponseDto
        {
            Id = i.Id,
            ProductId = i.ProductId,
            ProductName = i.Product?.Name ?? string.Empty,
            SKU = i.Product?.SKU ?? string.Empty,
            Quantity = i.Quantity
        }).ToList() ?? new List<ShipmentItemResponseDto>(),
        TrackingEvents = shipment.TrackingEvents?.Select(t => new ShipmentTrackingEventResponseDto
        {
            Id = t.Id,
            EventType = t.EventType ?? string.Empty,
            Location = t.Location ?? string.Empty,
            OccurredAt = t.OccurredAt
        }).ToList() ?? new List<ShipmentTrackingEventResponseDto>(),
        CreatedAt = shipment.CreatedAt,
        UpdatedAt = shipment.UpdatedAt
    };
}