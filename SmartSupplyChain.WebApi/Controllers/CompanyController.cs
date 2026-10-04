using Microsoft.AspNetCore.Mvc;
using SmartSupplyChain.Core.DTOs.Company;
using SmartSupplyChain.Core.Interface.Services;

namespace SmartSupplyChain.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ICompanyService companyService, ILogger<CompanyController> logger)
        {
            _companyService = companyService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyResponseDto>>> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var companies = await _companyService.GetAllAsync(cancellationToken);
                return Ok(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all companies.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred processing your request." });
            }
        }

        [HttpGet("{id:guid}", Name = "GetCompanyById")]
        public async Task<ActionResult<CompanyResponseDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var company = await _companyService.GetByIdAsync(id, cancellationToken);
                if (company == null)
                {
                    return NotFound(new { message = $"Company with ID '{id}' was not found." });
                }

                return Ok(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching company with ID {CompanyId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred processing your request." });
            }
        }

        /// <summary>
        /// Upserts a company record. 
        /// If dto.Id is null/empty, a new record is created (Returns 201 Created).
        /// If dto.Id is provided, the existing record is updated (Returns 200 OK).
        /// </summary>
        [HttpPut]
        public async Task<ActionResult<CompanyResponseDto>> Upsert(
            [FromBody] UpsertCompanyDto dto, 
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Determine if this is a creation or update based on the ID presence
                bool isNew = !dto.Id.HasValue || dto.Id.Value == Guid.Empty;

                var result = await _companyService.UpsertAsync(dto.Id, dto, cancellationToken);

                if (isNew)
                {
                    // Returns 201 Created with Location header pointing to GetById
                    return CreatedAtRoute("GetCompanyById", new { id = result.Id }, result);
                }

                // Returns 200 OK for updates
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Upsert failed because the company ID {CompanyId} was not found.", dto.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Business validation failed during company upsert: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during company upsert operation.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred processing your request." });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var deleted = await _companyService.DeleteAsync(id, cancellationToken);
                if (!deleted)
                {
                    return NotFound(new { message = $"Company with ID '{id}' was not found." });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting company with ID {CompanyId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred processing your request." });
            }
        }
    }
}