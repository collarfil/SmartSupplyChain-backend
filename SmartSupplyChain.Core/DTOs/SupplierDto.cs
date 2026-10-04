using System;
using System.ComponentModel.DataAnnotations;

namespace SmartSupplyChain.Core.DTOs.Supplier
{
    public class CreateSupplierDto
    {
        [Required, StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(100)]
        public string ContactEmail { get; set; } = string.Empty;

        [Required, Phone, StringLength(20)]
        public string ContactPhone { get; set; } = string.Empty;
    }

    public class UpdateSupplierDto : CreateSupplierDto
    {
    }

    public class SupplierResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
       public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}