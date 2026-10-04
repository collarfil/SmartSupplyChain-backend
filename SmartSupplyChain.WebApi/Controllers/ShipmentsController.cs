using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSupplyChain.Core.DTOs.Shipment;
using SmartSupplyChain.Core.Exceptions;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShipmentsController : ControllerBase
{
    private readonly IShipmentService _shipmentService;

    public ShipmentsController(IShipmentService shipmentService)
    {
        _shipmentService = shipmentService;
    }

    [HttpPut("upsert")]
    [HttpPut("upsert/{id:guid}")]
    public async Task<ActionResult<ShipmentResponseDto>> Upsert([FromRoute] Guid? id, [FromBody] CreateShipmentDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _shipmentService.UpsertAsync(id, dto, cancellationToken);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred processing your request.", details = ex.Message });
        }
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<ShipmentResponseDto>> UpdateStatus(Guid id, [FromBody] UpdateShipmentStatusDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _shipmentService.UpdateStatusAsync(id, dto, cancellationToken);
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

    [HttpPost("{id:guid}/tracking")]
    public async Task<ActionResult<ShipmentResponseDto>> AddTrackingEvent(Guid id, [FromBody] CreateShipmentTrackingEventDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _shipmentService.AddTrackingEventAsync(id, dto, cancellationToken);
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

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ShipmentResponseDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _shipmentService.GetByIdAsync(id, cancellationToken);
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
    public async Task<ActionResult<IEnumerable<ShipmentResponseDto>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _shipmentService.GetAllAsync(cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred processing your request.", details = ex.Message });
        }
    }

    [HttpGet("company/{companyId:guid}")]
    public async Task<ActionResult<IEnumerable<ShipmentResponseDto>>> GetByCompany(Guid companyId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _shipmentService.GetByCompanyAsync(companyId, cancellationToken);
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
            await _shipmentService.DeleteAsync(id, cancellationToken);
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