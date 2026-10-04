using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartSupplyChain.Core.DTOs.Shipment;

namespace SmartSupplyChain.Core.Interfaces;

public interface IShipmentService
{
    Task<ShipmentResponseDto> UpsertAsync(Guid? id, CreateShipmentDto dto, CancellationToken cancellationToken = default);
    Task<ShipmentResponseDto> UpdateStatusAsync(Guid id, UpdateShipmentStatusDto dto, CancellationToken cancellationToken = default);
    Task<ShipmentResponseDto> AddTrackingEventAsync(Guid id, CreateShipmentTrackingEventDto dto, CancellationToken cancellationToken = default);
    Task<ShipmentResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ShipmentResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ShipmentResponseDto>> GetByCompanyAsync(Guid companyId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}