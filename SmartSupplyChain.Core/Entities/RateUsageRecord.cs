using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSupplyChain.Core.Entities
{
    public class RateUsageRecord
    {
        public Guid Id { get; set; }
        public Guid RatePlanId { get; set; }
        public Guid ShipmentId { get; set; }

        public decimal TotalDistanceKm { get; set; }
        public decimal TotalWeightKg { get; set; }
        public decimal TotalCost { get; set; }

        // Navigation
        public RatePlan RatePlan { get; set; }
        public Shipment Shipment { get; set; }
    }
}
