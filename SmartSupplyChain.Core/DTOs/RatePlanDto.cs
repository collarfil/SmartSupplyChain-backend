using System;
using System.ComponentModel.DataAnnotations;

namespace SmartSupplyChain.Core.DTOs.RatePlan
{
    public class CreateRatePlanDto
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0.00, double.MaxValue, ErrorMessage = "Cost per Km cannot be negative.")]
        public decimal CostPerKm { get; set; }

        [Range(0.00, double.MaxValue, ErrorMessage = "Cost per Kg cannot be negative.")]
        public decimal CostPerKg { get; set; }
    }

    public class UpdateRatePlanDto : CreateRatePlanDto
    {
    }

    public class RatePlanResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal CostPerKm { get; set; }
        public decimal CostPerKg { get; set; }
       public DateTime CreatedAt { get; set; }
       public DateTime? UpdatedAt { get; set; }
    }
}