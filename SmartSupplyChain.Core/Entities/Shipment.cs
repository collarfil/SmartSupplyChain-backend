using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSupplyChain.Core.Enums;

namespace SmartSupplyChain.Core.Entities
{
    public class Shipment
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid DriverId { get; set; }
        public Guid VehicleId { get; set; }

        public DateTime PickupDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string Status { get; set; }

        // Navigation
        public Company Company { get; set; }
        public Driver Driver { get; set; }
        public Vehicle Vehicle { get; set; }

        public ICollection<ShipmentTrackingEvent> TrackingEvents { get; set; } = new List<ShipmentTrackingEvent>();
        public ICollection<IncidentReport> IncidentReports { get; set; } = new List<IncidentReport>();
        public ICollection<ShipmentItem> Items { get; set; } = new List<ShipmentItem>();
    }
}
