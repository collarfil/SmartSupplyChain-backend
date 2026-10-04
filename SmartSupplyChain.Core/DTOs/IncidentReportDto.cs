using System;
using System.ComponentModel.DataAnnotations;

namespace SmartSupplyChain.Core.DTOs.IncidentReport
{
    public class CreateIncidentReportDto
    {
        [Required]
        public Guid ShipmentId { get; set; }

        [Required, StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;
    }

    public class UpdateIncidentReportDto
    {
        [Required, StringLength(1000)]
        public string Description { get; set; } = string.Empty;
    }

    public class IncidentReportResponseDto
    {
        public Guid Id { get; set; }
        public Guid ShipmentId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ReportedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}