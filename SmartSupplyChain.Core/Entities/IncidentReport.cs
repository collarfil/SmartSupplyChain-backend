using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSupplyChain.Core.Entities
{
    public class IncidentReport
    {
        public Guid Id { get; set; }
        public Guid ShipmentId { get; set; }

        public string Description { get; set; }
        public DateTime ReportedAt { get; set; }

        // Navigation
        public Shipment Shipment { get; set; }
    }
}
