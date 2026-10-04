using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartSupplyChain.Core.DTOs.PurchaseOrder
{
    public class CreatePurchaseOrderItemDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Unit cost must be greater than zero.")]
        public decimal UnitCost { get; set; }
    }

    public class CreatePurchaseOrderDto
    {
        [Required]
        public Guid SupplierId { get; set; }

        [Required, StringLength(50)]
        public string Status { get; set; } = "Pending";

        [MinLength(1, ErrorMessage = "A purchase order must contain at least one item.")]
        public List<CreatePurchaseOrderItemDto> Items { get; set; } = new();
    }

    public class UpdatePurchaseOrderStatusDto
    {
        [Required, StringLength(50)]
        public string Status { get; set; } = string.Empty;
    }

    public class PurchaseOrderItemResponseDto
    {
        public Guid Id { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public string SKU { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal LineTotal => Quantity * UnitCost;
    }

    public class PurchaseOrderResponseDto
    {
        public Guid Id { get; set; }
        public Guid SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public string Status { get; set; } = string.Empty;
        
        // Computed purely in Application layer via Items.Sum()
        public decimal TotalAmount { get; set; } 
        
        public List<PurchaseOrderItemResponseDto> Items { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}