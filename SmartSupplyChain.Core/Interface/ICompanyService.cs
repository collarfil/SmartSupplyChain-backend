using SmartSupplyChain.Core.DTOs.Company;

namespace SmartSupplyChain.Core.Interface.Services
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<CompanyResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        
        // Change CreateCompanyDto to UpsertCompanyDto here:
        Task<CompanyResponseDto> UpsertAsync(Guid? id, UpsertCompanyDto dto, CancellationToken cancellationToken = default);
        
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}