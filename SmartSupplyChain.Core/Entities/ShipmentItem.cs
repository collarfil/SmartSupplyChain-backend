using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSupplyChain.Core.Common;

namespace SmartSupplyChain.Core.Entities
{
    public class ShipmentItem:BaseAuditableEntity
    {
        public Guid Id { get; set; }
        public Guid ShipmentId { get; set; }
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        // Navigation
        public Shipment Shipment { get; set; }
        public Product Product { get; set; }
    }
}
