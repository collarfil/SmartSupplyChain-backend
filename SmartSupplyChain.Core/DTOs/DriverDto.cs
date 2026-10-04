using System;
using System.ComponentModel.DataAnnotations;

namespace SmartSupplyChain.Core.DTOs.Driver
{
    public class CreateDriverDto
    {
        [Required]
        public Guid CompanyId { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string LicenseNumber { get; set; } = string.Empty;

        [Required, Phone, StringLength(20)]
        public string Phone { get; set; } = string.Empty;
    }

    public class UpdateDriverDto
    {
        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string LicenseNumber { get; set; } = string.Empty;

        [Required, Phone, StringLength(20)]
        public string Phone { get; set; } = string.Empty;
    }

    public class DriverResponseDto
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
       public DateTime CreatedAt { get; set; }
       public DateTime? UpdatedAt { get; set; }
    }
}