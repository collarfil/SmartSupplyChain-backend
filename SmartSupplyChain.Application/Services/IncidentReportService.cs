using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartSupplyChain.Core.DTOs.IncidentReport;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Exceptions;
using SmartSupplyChain.Core.Interface;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.Application.Services;

public class IncidentReportService : IIncidentReportService
{
    private readonly IRepository<IncidentReport> _incidentRepository;
    private readonly IRepository<Shipment> _shipmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public IncidentReportService(
        IRepository<IncidentReport> incidentRepository,
        IRepository<Shipment> shipmentRepository,
        IUnitOfWork unitOfWork)
    {
        _incidentRepository = incidentRepository;
        _shipmentRepository = shipmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IncidentReportResponseDto> CreateAsync(CreateIncidentReportDto dto, CancellationToken cancellationToken = default)
    {
        var shipment = await _shipmentRepository.GetByIdAsync(dto.ShipmentId);
        if (shipment == null)
            throw new NotFoundException(nameof(Shipment), dto.ShipmentId);

        var report = new IncidentReport
        {
            Id = Guid.NewGuid(),
            ShipmentId = dto.ShipmentId,
            Description = dto.Description,
            ReportedAt = dto.ReportedAt == default ? DateTime.UtcNow : dto.ReportedAt
        };

        await _incidentRepository.AddAsync(report);
        await _unitOfWork.CompleteAsync();

        return MapToResponseDto(report);
    }

    public async Task<IncidentReportResponseDto> UpsertAsync(Guid shipmentId, CreateIncidentReportDto dto, CancellationToken cancellationToken = default)
    {
        var shipment = await _shipmentRepository.GetByIdAsync(shipmentId);
        if (shipment == null)
            throw new NotFoundException(nameof(Shipment), shipmentId);

        var existingReports = await _incidentRepository.FindAsync(r => r.ShipmentId == shipmentId);
        var existingReport = existingReports.FirstOrDefault();

        if (existingReport != null)
        {
            existingReport.Description = dto.Description;
            existingReport.ReportedAt = dto.ReportedAt == default ? DateTime.UtcNow : dto.ReportedAt;

            _incidentRepository.Update(existingReport);
            await _unitOfWork.CompleteAsync();
            return MapToResponseDto(existingReport);
        }

        return await CreateAsync(dto, cancellationToken);
    }

    public async Task<IncidentReportResponseDto> UpdateAsync(Guid id, UpdateIncidentReportDto dto, CancellationToken cancellationToken = default)
    {
        var report = await _incidentRepository.GetByIdAsync(id);
        if (report == null)
            throw new NotFoundException(nameof(IncidentReport), id);

        report.Description = dto.Description;

        _incidentRepository.Update(report);
        await _unitOfWork.CompleteAsync();

        return MapToResponseDto(report);
    }

    public async Task<IncidentReportResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var report = await _incidentRepository.GetByIdAsync(id);
        if (report == null)
            throw new NotFoundException(nameof(IncidentReport), id);

        return MapToResponseDto(report);
    }

    public async Task<IEnumerable<IncidentReportResponseDto>> GetByShipmentIdAsync(Guid shipmentId, CancellationToken cancellationToken = default)
    {
        var reports = await _incidentRepository.FindAsync(r => r.ShipmentId == shipmentId);
        return reports.Select(MapToResponseDto);
    }

    public async Task<IEnumerable<IncidentReportResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var reports = await _incidentRepository.GetAllAsync();
        return reports.Select(MapToResponseDto);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var report = await _incidentRepository.GetByIdAsync(id);
        if (report == null)
            throw new NotFoundException(nameof(IncidentReport), id);

        _incidentRepository.Remove(report);
        return await _unitOfWork.CompleteAsync() > 0;
    }

    private static IncidentReportResponseDto MapToResponseDto(IncidentReport report) => new()
    {
        Id = report.Id,
        ShipmentId = report.ShipmentId,
        Description = report.Description,
        ReportedAt = report.ReportedAt,
        CreatedAt = report.CreatedAt,
        UpdatedAt = report.UpdatedAt
    };
}