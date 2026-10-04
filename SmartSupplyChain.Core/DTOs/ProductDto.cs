using System;
using System.ComponentModel.DataAnnotations;

namespace SmartSupplyChain.Core.DTOs.Product
{
    public class CreateProductDto
    {
        [Required]
        public Guid WarehouseId { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string SKU { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Unit cost must be greater than zero.")]
        public decimal UnitCost { get; set; }

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateProductDto
    {
        [Required, StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string SKU { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Unit cost must be greater than zero.")]
        public decimal UnitCost { get; set; }

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
    }

    public class ProductResponseDto
    {
        public Guid Id { get; set; }
        public Guid WarehouseId { get; set; }
        public string WarehouseName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public decimal UnitCost { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
       public DateTime? UpdatedAt { get; set; }
    }
}