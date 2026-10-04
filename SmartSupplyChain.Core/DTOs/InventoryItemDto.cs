using System;
using System.ComponentModel.DataAnnotations;

namespace SmartSupplyChain.Core.DTOs.InventoryItem
{
    public class CreateInventoryItemDto
    {
        [Required]
        public Guid WarehouseId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int QuantityAvailable { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Reorder level cannot be negative.")]
        public int ReorderLevel { get; set; }
    }

    public class UpdateInventoryStockDto
    {
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int QuantityAvailable { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Reorder level cannot be negative.")]
        public int ReorderLevel { get; set; }
    }

    public class InventoryItemResponseDto
    {
        public Guid Id { get; set; }
        public Guid WarehouseId { get; set; }
        public string WarehouseName { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int QuantityAvailable { get; set; }
        public int ReorderLevel { get; set; }
        public bool NeedsReorder => QuantityAvailable <= ReorderLevel;
       public DateTime CreatedAt { get; set; }
       public DateTime? UpdatedAt { get; set; }
    }
}