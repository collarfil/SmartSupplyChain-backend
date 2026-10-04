using System;

namespace SmartSupplyChain.Core.DTOs;

public record CustomerDto(
    Guid Id,
    string Name,
    string ContactEmail,
    string PhoneNumber,
    string Address,
    string TaxIdentificationNumber,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record UpsertCustomerDto(
    string Name,
    string ContactEmail,
    string PhoneNumber,
    string Address,
    string TaxIdentificationNumber
);