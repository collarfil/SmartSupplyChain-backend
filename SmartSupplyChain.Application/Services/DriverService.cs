using SmartSupplyChain.Core.DTOs.Driver;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Interface;
using SmartSupplyChain.Core.Interface.Services;

namespace SmartSupplyChain.Application.Services
{
    public class DriverService : IDriverService
    {
        private readonly IRepository<Driver> _driverRepository;
        private readonly IRepository<Company> _companyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DriverService(
            IRepository<Driver> driverRepository, 
            IRepository<Company> companyRepository, 
            IUnitOfWork unitOfWork)
        {
            _driverRepository = driverRepository;
            _companyRepository = companyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DriverResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            // Load drivers with navigation property 'Company' included
            var drivers = await _driverRepository.GetAllAsync(cancellationToken, d => d.Company);
            return drivers.Select(MapToResponseDto);
        }

        public async Task<DriverResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var driver = await _driverRepository.GetByIdAsync(id, cancellationToken, d => d.Company);
            return driver == null ? null : MapToResponseDto(driver);
        }

        public async Task<IEnumerable<DriverResponseDto>> GetByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default)
        {
            var drivers = await _driverRepository.FindAsync(d => d.CompanyId == companyId, cancellationToken, d => d.Company);
            return drivers.Select(MapToResponseDto);
        }

        public async Task<DriverResponseDto> UpsertAsync(Guid? id, CreateDriverDto dto, CancellationToken cancellationToken = default)
        {
            // Verify that the linked company exists
            var company = await _companyRepository.GetByIdAsync(dto.CompanyId, cancellationToken)
                ?? throw new KeyNotFoundException($"Company with ID '{dto.CompanyId}' was not found.");

            Driver driver;

            if (id.HasValue && id.Value != Guid.Empty)
            {
                // UPDATE branch
                driver = await _driverRepository.GetByIdAsync(id.Value, cancellationToken)
                    ?? throw new KeyNotFoundException($"Driver with ID '{id.Value}' was not found.");

                driver.CompanyId = dto.CompanyId;
                driver.FullName = dto.FullName;
                driver.LicenseNumber = dto.LicenseNumber;
                driver.Phone = dto.Phone;
                driver.UpdatedAt = DateTime.UtcNow;

                _driverRepository.Update(driver);
            }
            else
            {
                // INSERT branch
                driver = new Driver
                {
                    Id = Guid.NewGuid(),
                    CompanyId = dto.CompanyId,
                    FullName = dto.FullName,
                    LicenseNumber = dto.LicenseNumber,
                    Phone = dto.Phone,
                    CreatedAt = DateTime.UtcNow
                };

                await _driverRepository.AddAsync(driver, cancellationToken);
            }

            await _unitOfWork.CompleteAsync();

            // Attach company navigation entity for response mapping
            driver.Company = company;

            return MapToResponseDto(driver);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var driver = await _driverRepository.GetByIdAsync(id, cancellationToken);
            if (driver == null) return false;

            _driverRepository.Remove(driver);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        private static DriverResponseDto MapToResponseDto(Driver driver)
        {
            return new DriverResponseDto
            {
                Id = driver.Id,
                CompanyId = driver.CompanyId,
                CompanyName = driver.Company?.Name ?? string.Empty,
                FullName = driver.FullName,
                LicenseNumber = driver.LicenseNumber,
                Phone = driver.Phone,
                CreatedAt = driver.CreatedAt,
                UpdatedAt = driver.UpdatedAt
            };
        }
    }
}