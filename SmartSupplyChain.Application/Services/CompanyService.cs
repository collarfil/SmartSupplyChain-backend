using SmartSupplyChain.Core.DTOs.Company;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Interface;
using SmartSupplyChain.Core.Interface.Services;

namespace SmartSupplyChain.Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IRepository<Company> _companyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CompanyService(IRepository<Company> companyRepository, IUnitOfWork unitOfWork)
        {
            _companyRepository = companyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CompanyResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var companies = await _companyRepository.GetAllAsync(cancellationToken);
            return companies.Select(MapToResponseDto);
        }

        public async Task<CompanyResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var company = await _companyRepository.GetByIdAsync(id, cancellationToken);
            return company == null ? null : MapToResponseDto(company);
        }

      public async Task<CompanyResponseDto> UpsertAsync(Guid? id, UpsertCompanyDto dto, CancellationToken cancellationToken = default)
{
    // Determine the target ID (either from route/argument or from the JSON body DTO)
    var targetId = (id.HasValue && id.Value != Guid.Empty) ? id.Value : (dto.Id ?? Guid.Empty);

    // FIX: Put the lambda expression FIRST (Argument 1), and cancellationToken SECOND (Argument 2)
    var existingEmail = await _unitOfWork.Companies.FindAsync(
        c => c.ContactEmail.ToLower() == dto.ContactEmail.ToLower() && c.Id != targetId,
        cancellationToken
    );

    if (existingEmail.Any())
        throw new InvalidOperationException($"Company with email '{dto.ContactEmail}' already exists.");

    Company? company = null;

    if (targetId != Guid.Empty)
    {
        // FIX: Same here. Lambda first, cancellationToken second.
        var companies = await _unitOfWork.Companies.FindAsync(
            c => c.Id == targetId,
            cancellationToken
        );
        company = companies.FirstOrDefault();
    }

    if (company == null)
    {
        // Insert branch
        company = new Company
        {
            Id = targetId != Guid.Empty ? targetId : Guid.NewGuid(),
            Name = dto.Name,
            RegistrationNumber = dto.RegistrationNumber,
            Address = dto.Address,
            ContactEmail = dto.ContactEmail,
            ContactPhone = dto.ContactPhone,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Companies.AddAsync(company, cancellationToken);
    }
    else
    {
        // Update branch
        company.Name = dto.Name;
        company.RegistrationNumber = dto.RegistrationNumber;
        company.Address = dto.Address;
        company.ContactEmail = dto.ContactEmail;
        company.ContactPhone = dto.ContactPhone;
        company.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Companies.Update(company);
    }

    await _unitOfWork.CompleteAsync(cancellationToken);
    return MapToResponseDto(company);
}
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var company = await _companyRepository.GetByIdAsync(id, cancellationToken);
            if (company == null) return false;

            _companyRepository.Remove(company);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        private static CompanyResponseDto MapToResponseDto(Company company)
        {
            return new CompanyResponseDto
            {
                Id = company.Id,
                Name = company.Name,
                RegistrationNumber = company.RegistrationNumber,
                Address = company.Address,
                ContactEmail = company.ContactEmail,
                ContactPhone = company.ContactPhone,
                CreatedAt = company.CreatedAt,
                UpdatedAt = company.UpdatedAt
            };
        }
    }
}