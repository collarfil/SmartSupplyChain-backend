using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSupplyChain.Core.Entities
{
    public class Driver
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }

        public string FullName { get; set; }
        public string LicenseNumber { get; set; }
        public string Phone { get; set; }

        // Navigation
        public Company Company { get; set; }
        public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}
