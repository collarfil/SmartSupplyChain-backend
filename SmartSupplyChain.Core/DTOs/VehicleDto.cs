using System;
using System.ComponentModel.DataAnnotations;

namespace SmartSupplyChain.Core.DTOs.Vehicle
{
    public class CreateVehicleDto
    {
        [Required]
        public Guid CompanyId { get; set; }

        [Required, StringLength(20)]
        public string PlateNumber { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Model { get; set; } = string.Empty;

        [Range(1900, 2100, ErrorMessage = "Please provide a valid manufacturing year.")]
        public int Year { get; set; }
    }

    public class UpdateVehicleDto
    {
        [Required, StringLength(20)]
        public string PlateNumber { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Model { get; set; } = string.Empty;

        [Range(1900, 2100, ErrorMessage = "Please provide a valid manufacturing year.")]
        public int Year { get; set; }
    }

    public class VehicleResponseDto
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string PlateNumber { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public DateTime CreatedAt { get; set; }
       public DateTime? UpdatedAt { get; set; }
    }
}