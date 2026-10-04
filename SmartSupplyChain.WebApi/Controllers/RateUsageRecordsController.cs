using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSupplyChain.Core.DTOs.RateUsageRecord;
using SmartSupplyChain.Core.Exceptions;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RateUsageRecordsController : ControllerBase
{
    private readonly IRateUsageRecordService _rateUsageRecordService;

    public RateUsageRecordsController(IRateUsageRecordService rateUsageRecordService)
    {
        _rateUsageRecordService = rateUsageRecordService;
    }

    [HttpPost]
    public async Task<ActionResult<RateUsageRecordResponseDto>> Create([FromBody] CreateRateUsageRecordDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _rateUsageRecordService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred processing your request.", details = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RateUsageRecordResponseDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _rateUsageRecordService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred processing your request.", details = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RateUsageRecordResponseDto>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _rateUsageRecordService.GetAllAsync(cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred processing your request.", details = ex.Message });
        }
    }

    [HttpGet("shipment/{shipmentId:guid}")]
    public async Task<ActionResult<IEnumerable<RateUsageRecordResponseDto>>> GetByShipment(Guid shipmentId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _rateUsageRecordService.GetByShipmentAsync(shipmentId, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred processing your request.", details = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _rateUsageRecordService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred processing your request.", details = ex.Message });
        }
    }
}