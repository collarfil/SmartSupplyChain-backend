using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSupplyChain.Core.Common;

namespace SmartSupplyChain.Core.Entities
{
    public class ShipmentTrackingEvent:BaseAuditableEntity
    {
        public Guid Id { get; set; }
        public Guid ShipmentId { get; set; }

        public string EventType { get; set; }
        public string Location { get; set; }
        public DateTime OccurredAt { get; set; }

        // Navigation
        public Shipment Shipment { get; set; }
    }
}
