using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSupplyChain.Core.DTOs.RatePlan;
using SmartSupplyChain.Core.Exceptions;
using SmartSupplyChain.Core.Interfaces;

namespace SmartSupplyChain.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RatePlansController : ControllerBase
{
    private readonly IRatePlanService _ratePlanService;

    public RatePlansController(IRatePlanService ratePlanService)
    {
        _ratePlanService = ratePlanService;
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<RatePlanResponseDto>> Upsert(Guid id, [FromBody] CreateRatePlanDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _ratePlanService.UpsertAsync(id, dto, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred processing your request.", details = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<RatePlanResponseDto>> Create([FromBody] CreateRatePlanDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _ratePlanService.UpsertAsync(null, dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred processing your request.", details = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RatePlanResponseDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _ratePlanService.GetByIdAsync(id, cancellationToken);
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
    public async Task<ActionResult<IEnumerable<RatePlanResponseDto>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _ratePlanService.GetAllAsync(cancellationToken);
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
            await _ratePlanService.DeleteAsync(id, cancellationToken);
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