using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartSupplyChain.Core.DTOs.IncidentReport;

namespace SmartSupplyChain.Core.Interfaces;

public interface IIncidentReportService
{
    Task<IncidentReportResponseDto> CreateAsync(CreateIncidentReportDto dto, CancellationToken cancellationToken = default);
    Task<IncidentReportResponseDto> UpsertAsync(Guid shipmentId, CreateIncidentReportDto dto, CancellationToken cancellationToken = default);
    Task<IncidentReportResponseDto> UpdateAsync(Guid id, UpdateIncidentReportDto dto, CancellationToken cancellationToken = default);
    Task<IncidentReportResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<IncidentReportResponseDto>> GetByShipmentIdAsync(Guid shipmentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<IncidentReportResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}