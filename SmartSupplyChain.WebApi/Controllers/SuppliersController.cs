using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSupplyChain.Core.DTOs.Supplier;
using SmartSupplyChain.Core.Exceptions;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SuppliersController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpPut("upsert")]
    [HttpPut("upsert/{id:guid}")]
    public async Task<ActionResult<SupplierResponseDto>> Upsert([FromRoute] Guid? id, [FromBody] CreateSupplierDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _supplierService.UpsertAsync(id, dto, cancellationToken);
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
    public async Task<ActionResult<SupplierResponseDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _supplierService.GetByIdAsync(id, cancellationToken);
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
    public async Task<ActionResult<IEnumerable<SupplierResponseDto>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _supplierService.GetAllAsync(cancellationToken);
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
            await _supplierService.DeleteAsync(id, cancellationToken);
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