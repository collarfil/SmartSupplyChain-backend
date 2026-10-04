using System;
using System.ComponentModel.DataAnnotations;

namespace SmartSupplyChain.Core.DTOs.Warehouse
{
    public class CreateWarehouseDto
    {
        [Required]
        public Guid CompanyId { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(250)]
        public string Location { get; set; } = string.Empty;
    }

    public class UpdateWarehouseDto
    {
        [Required, StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(250)]
        public string Location { get; set; } = string.Empty;
    }

    public class WarehouseResponseDto
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
       public DateTime? UpdatedAt { get; set; }
    }
}