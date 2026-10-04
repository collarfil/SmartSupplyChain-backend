using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartSupplyChain.Core.DTOs.Shipment
{
    public class CreateShipmentItemDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
    }

    public class CreateShipmentTrackingEventDto
    {
        [Required, StringLength(50)]
        public string EventType { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string Location { get; set; } = string.Empty;

        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    }

    public class CreateShipmentDto
    {
        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        public Guid DriverId { get; set; }

        [Required]
        public Guid VehicleId { get; set; }

        public DateTime PickupDate { get; set; } = DateTime.UtcNow;

        public DateTime? DeliveryDate { get; set; }

        [Required, StringLength(50)]
        public string Status { get; set; } = "Pending";

        [MinLength(1, ErrorMessage = "Shipment must contain at least one item.")]
        public List<CreateShipmentItemDto> Items { get; set; } = new();
    }

    public class UpdateShipmentStatusDto
    {
        [Required, StringLength(50)]
        public string Status { get; set; } = string.Empty;

        public DateTime? DeliveryDate { get; set; }
    }

    public class ShipmentItemResponseDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }

    public class ShipmentTrackingEventResponseDto
    {
        public Guid Id { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime OccurredAt { get; set; }
    }

    public class ShipmentResponseDto
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public Guid DriverId { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public Guid VehicleId { get; set; }
        public string VehiclePlateNumber { get; set; } = string.Empty;
        public DateTime PickupDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string Status { get; set; } = string.Empty;

        public List<ShipmentItemResponseDto> Items { get; set; } = new();
        public List<ShipmentTrackingEventResponseDto> TrackingEvents { get; set; } = new();

        public DateTime CreatedAt { get; set; }
       public DateTime? UpdatedAt { get; set; }
    }
}