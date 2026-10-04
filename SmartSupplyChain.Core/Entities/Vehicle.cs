using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSupplyChain.Core.Common;

namespace SmartSupplyChain.Core.Entities
{
    public class Vehicle:BaseAuditableEntity
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }

        public string PlateNumber { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }

        // Navigation
        public Company Company { get; set; }
        public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}
