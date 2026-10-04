using System;
using System.ComponentModel.DataAnnotations;

namespace SmartSupplyChain.Core.DTOs.RateUsageRecord
{
    public class CreateRateUsageRecordDto
    {
        [Required]
        public Guid RatePlanId { get; set; }

        [Required]
        public Guid ShipmentId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Total distance must be greater than zero.")]
        public decimal TotalDistanceKm { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Total weight must be greater than zero.")]
        public decimal TotalWeightKg { get; set; }
    }

    public class RateUsageRecordResponseDto
    {
        public Guid Id { get; set; }
        public Guid RatePlanId { get; set; }
        public string RatePlanName { get; set; } = string.Empty;
        public Guid ShipmentId { get; set; }
        public decimal TotalDistanceKm { get; set; }
        public decimal TotalWeightKg { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}