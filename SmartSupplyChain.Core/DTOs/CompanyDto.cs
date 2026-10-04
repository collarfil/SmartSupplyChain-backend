using System;
using System.ComponentModel.DataAnnotations;

namespace SmartSupplyChain.Core.DTOs.Company
{
   

    public class UpsertCompanyDto
    {
        public Guid? Id { get; set; } // Null or Guid.Empty means Insert; valid Guid means Update
        public string Name { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
    }
    public class CreateCompanyDto
    {
        [Required, StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required, StringLength(250)]
        public string Address { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(100)]
        public string ContactEmail { get; set; } = string.Empty;

        [Required, Phone, StringLength(20)]
        public string ContactPhone { get; set; } = string.Empty;
    }

    public class UpdateCompanyDto : CreateCompanyDto
    {
    }

    public class CompanyResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
       public DateTime CreatedAt { get; set; }
      public DateTime? UpdatedAt { get; set; }
    }
}