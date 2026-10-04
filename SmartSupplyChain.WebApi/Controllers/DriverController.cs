using Microsoft.AspNetCore.Mvc;
using SmartSupplyChain.Core.DTOs.Driver;
using SmartSupplyChain.Core.Interface.Services;

namespace SmartSupplyChain.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverService;
        private readonly ILogger<DriverController> _logger;

        public DriverController(IDriverService driverService, ILogger<DriverController> logger)
        {
            _driverService = driverService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverResponseDto>>> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var drivers = await _driverService.GetAllAsync(cancellationToken);
                return Ok(drivers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching drivers.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred processing your request." });
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<DriverResponseDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var driver = await _driverService.GetByIdAsync(id, cancellationToken);
                if (driver == null)
                    return NotFound(new { message = $"Driver with ID '{id}' was not found." });

                return Ok(driver);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching driver with ID {DriverId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred processing your request." });
            }
        }

        [HttpGet("company/{companyId:guid}")]
        public async Task<ActionResult<IEnumerable<DriverResponseDto>>> GetByCompanyId(Guid companyId, CancellationToken cancellationToken)
        {
            try
            {
                var drivers = await _driverService.GetByCompanyIdAsync(companyId, cancellationToken);
                return Ok(drivers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching drivers for company {CompanyId}.", companyId);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred processing your request." });
            }
        }

        /// <summary>
        /// Upserts a driver record. If ID is provided, updates existing driver; otherwise creates a new record.
        /// </summary>
        [HttpPut("{id:guid?}")]
        public async Task<ActionResult<DriverResponseDto>> Upsert(
            Guid? id, 
            [FromBody] CreateDriverDto dto, 
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _driverService.UpsertAsync(id, dto, cancellationToken);

                if (id.HasValue && id.Value != Guid.Empty)
                {
                    return Ok(result);
                }

                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Upsert failed: Target driver or associated CompanyId was not found.");
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Business logic validation failed during driver upsert.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during driver upsert operation.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred processing your request." });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var deleted = await _driverService.DeleteAsync(id, cancellationToken);
                if (!deleted)
                    return NotFound(new { message = $"Driver with ID '{id}' was not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting driver with ID {DriverId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred processing your request." });
            }
        }
    }
}