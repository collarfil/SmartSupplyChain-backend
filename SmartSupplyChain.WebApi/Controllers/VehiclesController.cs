using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSupplyChain.Core.DTOs.Vehicle;
using SmartSupplyChain.Core.Exceptions;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpPut("upsert")]
    [HttpPut("upsert/{id:guid}")]
    public async Task<ActionResult<VehicleResponseDto>> Upsert([FromRoute] Guid? id, [FromBody] CreateVehicleDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _vehicleService.UpsertAsync(id, dto, cancellationToken);
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

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<VehicleResponseDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _vehicleService.GetByIdAsync(id, cancellationToken);
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
    public async Task<ActionResult<IEnumerable<VehicleResponseDto>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _vehicleService.GetAllAsync(cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred processing your request.", details = ex.Message });
        }
    }

    [HttpGet("company/{companyId:guid}")]
    public async Task<ActionResult<IEnumerable<VehicleResponseDto>>> GetByCompany(Guid companyId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _vehicleService.GetByCompanyAsync(companyId, cancellationToken);
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
            await _vehicleService.DeleteAsync(id, cancellationToken);
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