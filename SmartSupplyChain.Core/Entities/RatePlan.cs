using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSupplyChain.Core.Common;

namespace SmartSupplyChain.Core.Entities
{
    public class RatePlan:BaseAuditableEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public decimal CostPerKm { get; set; }
        public decimal CostPerKg { get; set; }

        public ICollection<RateUsageRecord> UsageRecords { get; set; } = new List<RateUsageRecord>();
    }
}
