using SmartSupplyChain.Core.DTOs.Driver;

namespace SmartSupplyChain.Core.Interface.Services
{
    public interface IDriverService
    {
        Task<IEnumerable<DriverResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<DriverResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<DriverResponseDto>> GetByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default);
        Task<DriverResponseDto> UpsertAsync(Guid? id, CreateDriverDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}